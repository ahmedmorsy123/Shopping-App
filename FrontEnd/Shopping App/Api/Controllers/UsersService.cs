using System.Threading.Tasks;
using ShoppingApp.Api.Models;

namespace ShoppingApp.Api.Controllers
{
    public class UsersService : ApiClient
    {
        private const string GET_USER_ENDPOINT = "/api/Users/getUser";
        private const string UPDATE_USER_ENDPOINT = "/api/Users/UpdateUser";
        private const string DELETE_USER_ENDPOINT = "/api/Users/DeleteUser";
        private const string ADD_USER_ENDPOINT = "/api/Users/AddUser";


        /// <summary>
        /// Gets user information by ID
        /// </summary>
        /// <param name="userId">User ID to retrieve</param>
        /// <returns>User data if found</returns>
        public async Task<UserDto> GetUserAsync(int userId)
        {
            string queryString = $"id={userId}";

            try
            {
                return await GetAsync<UserDto>(GET_USER_ENDPOINT, queryString);
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == 404)
                {
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
        public async Task<UserDto> UpdateUserAsync(UserDto user, string oldPassword = null)
        {
            string queryString = string.IsNullOrEmpty(oldPassword) ? "" : $"oldPassword={oldPassword}";

            try
            {
                return await PutAsync<UserDto>(UPDATE_USER_ENDPOINT, user, queryString);
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == 404)
                {
                    throw new ApiException(404, $"User with ID {user.Id} not found");
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
            string queryString = $"id={userId}";

            try
            {
                await DeleteAsync(DELETE_USER_ENDPOINT, queryString);
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == 404)
                {
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
            try
            {
                return await PostAsync<UserDto>(ADD_USER_ENDPOINT, user);
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == 400)
                {
                    throw new ApiException(400, "Invalid user data. Please check your input.");
                }
                throw;
            }
        }
    }
}