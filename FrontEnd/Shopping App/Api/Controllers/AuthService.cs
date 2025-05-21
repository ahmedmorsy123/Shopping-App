using System.Text;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using ShoppingApp.Api.Models;
using Serilog;
using Shopping_App;
using System.Net.Http;

namespace ShoppingApp.Api.Controllers
{
    public class AuthService : ApiClient
    {
        private const string LOGIN_ENDPOINT = "/api/Auth/Login";
        private const string REFRESH_TOKEN_ENDPOINT = "/api/Auth/refresh-token";
        private const string LOGOUT_ENDPOINT = "/api/Auth/logout";

        public AuthService(HttpClient httpClient) : base(httpClient)
        {
        }

        /// <summary>
        /// Authenticates a user with their username and password
        /// </summary>
        /// <param name="username">Username for login</param>
        /// <param name="password">Password for login</param>
        /// <returns>User information if authentication is successful</returns>
        public async Task<TokenResponseDto> LoginAsync(string username, string password)
        {
            Log.Information("Logging in user with username: {Username}", username);
            var request = new LoginRequestDto
            {
                UserName = username,
                Password = password
            };

            try
            {
                var response = await PostAsync<TokenResponseDto>(LOGIN_ENDPOINT, request);
                SetAuthorizationToken(response.AccessToken);
                SetRefereshToken(response.RefreshToken);
                SetCurrentUserId(response.AccessToken);
                Log.Information("User logged in successfully with ID: {UserId}", ApiManger.CurrentLoggedInUser.Id);
                return response;
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == 400)
                {
                    Log.Error("Invalid username or password for user: {Username}", username);
                    throw new ApiException(400, "Invalid username or password");
                }
                throw;
            }
        }

        /// <summary>
        /// Refreshes the authentication token
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="refreshToken">Refresh token from previous login</param>
        /// <returns>New access token and refresh token</returns>
        public async Task<TokenResponseDto> RefreshTokenAsync()
        {
            Log.Information("Refreshing token for user ID: {UserId}", ApiManger.CurrentLoggedInUser.Id);
            var request = new RefreshTokenRequestDto
            {
                UserId = ApiManger.CurrentLoggedInUser.Id,
                RefreshToken = RefreshToken
            };

            try
            {
                var response = await PostAsync<TokenResponseDto>(REFRESH_TOKEN_ENDPOINT, request);
                if (response != null)
                {
                    SetAuthorizationToken(response.AccessToken);

                    Log.Information("Token refreshed successfully for user ID: {UserId}", ApiManger.CurrentLoggedInUser.Id);
                }
                else
                {
                    throw new ApiException(400, "Failed to refresh token");
                }
                return response;
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == 400)
                {
                    Log.Error("Failed to refresh token for user ID: {UserId}", ApiManger.CurrentLoggedInUser.Id);
                    throw new ApiException(400, "Invalid or expired refresh token");
                }
                throw;
            }
        }

        /// <summary>
        /// Logs out the current user
        /// </summary>
        public async Task LogoutAsync()
        {
            Log.Information("Logging out user with ID: {UserId}", ApiManger.CurrentLoggedInUser.Id);
            try
            {
                await PostAsync(LOGOUT_ENDPOINT);
                ClearAuthorizationToken();
                ClearRefereshToken();
                ApiManger.CurrentLoggedInUser.Id = 0;
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == 401)
                {
                    Log.Error("User with ID: {UserId} is already logged out or token expired", ApiManger.CurrentLoggedInUser.Id);
                    // Already logged out or token expired
                    ClearAuthorizationToken();
                    return;
                } 
                else if (ex.StatusCode == 400)
                {
                    Log.Error("Invalid Auth token in logout request for user ID: {UserId}", ApiManger.CurrentLoggedInUser.Id);
                    throw new ApiException(400, "Invalid Auth token in logout request");
                }
                else if (ex.StatusCode == 404)
                {
                    Log.Error("User not found or referesh token expired for user ID: {UserId}", ApiManger.CurrentLoggedInUser.Id);
                    throw new ApiException(404, "User not found or referesh token expired");
                }

                throw;
            }
        }


        private async void SetCurrentUserId(string jwt)
        {
            Log.Information("Setting current user ID from JWT");
            string userId = GetUserIdFromJwt(jwt);
            int id = int.Parse(userId);
            ApiManger.CurrentLoggedInUser = await ApiManger.Instance.UserService.GetUserAsync(id);

        }

        private string GetUserIdFromJwt(string jwt)
        {
            Log.Information("Extracting user ID from JWT");
            try
            {
                string[] parts = jwt.Split('.');
                if (parts.Length != 3)
                {
                    return null;
                }

                string payloadBase64Url = parts[1];
                string payloadJson = Base64UrlDecode(payloadBase64Url);

                JObject payload = JObject.Parse(payloadJson);

                string userId = (string)payload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"];

                return userId;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private string Base64UrlDecode(string base64Url)
        {
            string base64 = base64Url.Replace('-', '+').Replace('_', '/');
            switch (base64.Length % 4)
            {
                case 2:
                    base64 += "==";
                    break;
                case 3:
                    base64 += "=";
                    break;
            }
            byte[] bytes = Convert.FromBase64String(base64);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}