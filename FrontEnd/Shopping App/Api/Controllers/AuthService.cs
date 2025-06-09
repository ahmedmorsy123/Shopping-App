using System.Text;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using ShoppingApp.Api.Models;
using Serilog;
using Shopping_App;
using System.Net.Http;
using System.Xml.Linq;
using System.IO;
using Shopping_App.Hellpers;
using Shopping_App.ViewData;
using static ShoppingAppDB.Enums.Enums;

namespace ShoppingApp.Api.Controllers
{
    public class AuthService : ApiClient
    {
        public AuthService(HttpClient httpClient) : base(httpClient)
        {
        }
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
                var response = await PostAsync<TokenResponseDto>(Config.GetApiEndpoint("Auth", "LogIn"), request);
                SetTokens(response.AccessToken, response.RefreshToken);
                await SetCurrentUserId(response.AccessToken);
                Log.Information("User logged in successfully with ID: {UserId}", Config.GetCurrentUserId());
                return response;
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == 400)
                {
                    Log.Error("Invalid username or password for user: {Username}", username);
                    throw new ApiException(400, "Invalid username or password");
                }
                else
                {
                    throw;
                }
            }
        }
        public async Task<TokenResponseDto> RefreshTokenAsync()
        {
            // delete this line
            Log.Information("Refreshing token for user ID: {UserId}", Config.GetRememberedUserId());
            var request = new RefreshTokenRequestDto
            {
                UserId = Config.GetRememberedUserId(),
                RefreshToken = Config.GetRememberedRefreshToken()

            };


            try
            {
                var response = await PostAsync<TokenResponseDto>(Config.GetApiEndpoint("Auth", "RefreshToken"), request);
                if (response != null)
                {
                    Config.SetRememberMe(response.RefreshToken, Config.GetRememberedUserId());
                    SetTokens(response.AccessToken, response.RefreshToken);
                    UserDto user = await ApiManger.Instance.UserService.GetUserAsync(Config.GetRememberedUserId());
                    Config.SetCurrentUser(user);

                    Log.Information("Token refreshed successfully for user ID: {UserId}", Config.GetCurrentUserId());
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
                    Log.Error("Failed to refresh token for user ID: {UserId}", Config.GetCurrentUserId());
                    throw new ApiException(400, "Invalid or expired refresh token");
                }
                else
                {
                    throw;
                }
            }
        }
        public async Task LogoutAsync()
        {
            Log.Information("Logging out user with ID: {UserId}", Config.GetCurrentUserId());
            try
            {
                await PostAsync(Config.GetApiEndpoint("Auth", "LogOut"));
                Config.ClearCurrentUser();


            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == 401)
                {
                    Log.Error("User with ID: {UserId} is already logged out or token expired", Config.GetCurrentUserId());
                    // Already logged out or token expired
                    return;
                } 
                else if (ex.StatusCode == 400)
                {
                    Log.Error("Invalid Auth token in logout request for user ID: {UserId}", Config.GetCurrentUserId());
                    throw new ApiException(400, "Invalid Auth token in logout request");
                }
                else if (ex.StatusCode == 404)
                {
                    Log.Error("User not found or referesh token expired for user ID: {UserId}", Config.GetCurrentUserId());
                    throw new ApiException(404, "User not found or referesh token expired");
                }
                else
                {
                    throw;
                }

                
            }
        }

        public async Task<int> GetLoginCountByDurationAsync(TimeDuration duration)
        {
            Log.Information("Getting login count in duration: {Duration}", duration);
            string queryString = $"duration={duration}";
            try
            {
                return await GetAsync<int>(Config.GetApiEndpoint("Auth", "GetLoginCountByDuration"), queryString);
            }
            catch (ApiException ex)
            {
                if(ex.StatusCode == 401)
                {
                    Log.Error("Unauthorized access to login count for user ID: {UserId}", Config.GetCurrentUserId());
                    throw new ApiException(401, "Unauthorized access to login count");
                } else if(ex.StatusCode == 403)
                {
                    Log.Error("Forbidden access to login count for user ID: {UserId}", Config.GetCurrentUserId());
                    throw new ApiException(403, "Forbidden access to login count");
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<int> GetRegisterationCountByDurationAsync(TimeDuration duration)
        {
            Log.Information("Getting registration count in duration: {Duration}", duration);
            string queryString = $"duration={duration}";
            try
            {
                return await GetAsync<int>(Config.GetApiEndpoint("Auth", "GetRegisterationCountByDuration"), queryString);
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == 401)
                {
                    Log.Error("Unauthorized access to registration count for user ID: {UserId}", Config.GetCurrentUserId());
                    throw new ApiException(401, "Unauthorized access to registration count");
                }
                else if (ex.StatusCode == 403)
                {
                    Log.Error("Forbidden access to registration count for user ID: {UserId}", Config.GetCurrentUserId());
                    throw new ApiException(403, "Forbidden access to registration count");
                }
                else
                {
                    throw;
                }
            }
        }
        private async Task SetCurrentUserId(string jwt)
        {
            Log.Information("Setting current user ID from JWT");
            int? id = HellpersMethodes.GetUserIdFromJwt(jwt);
            UserDto user = await ApiManger.Instance.UserService.GetUserAsync(id.Value);
            Config.SetCurrentUser(user);
        }
        
    }
}