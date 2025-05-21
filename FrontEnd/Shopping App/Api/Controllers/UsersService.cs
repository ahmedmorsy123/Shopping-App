using System.Net.Http;
using System.Threading.Tasks;
using Serilog;
using ShoppingApp.Api.Models;

namespace ShoppingApp.Api.Controllers
{
    public class UsersService : ApiClient
    {
        private const string GET_USER_ENDPOINT = "/api/Users/getUser";
        private const string UPDATE_USER_ENDPOINT = "/api/Users/UpdateUser";
        private const string DELETE_USER_ENDPOINT = "/api/Users/DeleteUser";
        private const string ADD_USER_ENDPOINT = "/api/Users/AddUser";

        // set the http client
        public UsersService(HttpClient httpClient) : base(httpClient)
        {
        }

        /// <summary>
        /// Gets user information by ID
        /// </summary>
        /// <param name="userId">User ID to retrieve</param>
        /// <returns>User data if found</returns>
        public async Task<UserDto> GetUserAsync(int userId)
        {
            Log.Information("Getting user with ID: {UserId}", userId);
            string queryString = $"id={userId}";

            try
            {
                return await GetAsync<UserDto>(GET_USER_ENDPOINT, queryString);
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == 404)
                {
                    Log.Error("User not found with ID: {UserId}", userId);
                    throw new ApiException(404, $"User with ID {userId} not found");
                }
                throw;
            }
        }

        /// <summary>
        /// Updates user information
        /// </summary>
        /// <param name="user">Updated user data</param>
        /// <param name="oldPassword">Current password for verification</param>
        /// <returns>Updated user data</returns>
        public async Task<UserDto> UpdateUserAsync(UpdateUserDto user)
        {
            Log.Information("Updating user with ID: {UserId}", user.Id);

            try
            {
                return await PutAsync<UserDto>(UPDATE_USER_ENDPOINT, user);
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == 404)
                {
                    Log.Error("Faild to update user with id: {UserId} wrong password", user.Id);
                    throw new ApiException(400, $"Wrong Password");
                }
                throw;
            }
        }

        /// <summary>
        /// Deletes a user by ID
        /// </summary>
        /// <param name="userId">User ID to delete</param>
        public async Task DeleteUserAsync(int userId)
        {
            Log.Information("Deleting user with ID: {UserId}", userId);
            string queryString = $"id={userId}";

            try
            {
                await DeleteAsync(DELETE_USER_ENDPOINT, queryString);
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == 404)
                {
                    Log.Error("User not found with ID: {UserId}", userId);
                    throw new ApiException(404, $"User with ID {userId} not found");
                }
                throw;
            }
        }

        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="user">User data to create</param>
        /// <returns>Created user data with ID</returns>
        public async Task<UserDto> AddUserAsync(UserDto user)
        {
            Log.Information("Adding new user with username: {Username}", user.Name);
            try
            {
                return await PostAsync<UserDto>(ADD_USER_ENDPOINT, user);

            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == 400)
                {
                    Log.Error("UserName or Email already exists for username: {Username}", user.Name);
                    throw new ApiException(400, "UserName or Email already exists");
                }
                throw;
            }
        }
    }
}