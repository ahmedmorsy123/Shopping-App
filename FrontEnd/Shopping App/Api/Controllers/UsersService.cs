using System.IO;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Serilog;
using ShoppingApp.Api.Models;
using Shopping_App.Hellpers;

namespace ShoppingApp.Api.Controllers
{
    public class UsersService : ApiClient
    {
        public UsersService(HttpClient httpClient) : base(httpClient)
        {
        }
        public async Task<UserDto> GetUserAsync(int userId)
        {
            Log.Information("Getting user with ID: {UserId}", userId);
            string queryString = $"id={userId}";

            try
            {
                return await GetAsync<UserDto>(Config.GetApiEndpoint("Users", "GetUser"), queryString);
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
        public async Task<UserDto> UpdateUserAsync(UpdateUserDto user)
        {
            Log.Information("Updating user with ID: {UserId}", user.Id);

            try
            {
                return await PutAsync<UserDto>(Config.GetApiEndpoint("Users", "UpdateUser"), user);
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
        public async Task DeleteUserAsync(int userId)
        {
            Log.Information("Deleting user with ID: {UserId}", userId);
            string queryString = $"id={userId}";

            try
            {
                await DeleteAsync(Config.GetApiEndpoint("Users", "DeleteUser"), queryString);
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
        public async Task<UserDto> AddUserAsync(UserDto user)
        {
            Log.Information("Adding new user with username: {Username}", user.Name);
            try
            {
                return await PostAsync<UserDto>(Config.GetApiEndpoint("Users", "AddUser"), user);

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