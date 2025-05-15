using Microsoft.Extensions.Logging;
using ShoppingAppDB;
using ShoppingAppDB.Models;

namespace ShoppingAppBussiness
{
    public class UsersService
    {
        private ILogger<UsersService> _logger;
        private UserData _userData;
        private const string _prefix = "UsersBL ";

        public UsersService(ILogger<UsersService> logger, UserData userData)
        {
            _logger = logger;
            _userData = userData;
        }

        public async Task<UserDto?> GetUserById(int id)
        {
            _logger.LogInformation($"{_prefix}GetUserById");
            return await _userData.GetUserById(id);
        }

        public async Task<UserDto?> AddUser(UserDto user)
        {
            _logger.LogInformation($"{_prefix}AddUser");
            return await _userData.AddUser(user);
        }

        public async Task<UserDto?> UpdateUser(UserDto user, string oldPassword)
        {
            _logger.LogInformation($"{_prefix}UpdateUser");
            return await _userData.UpdateUser(user, oldPassword);
        }

        public async Task<bool> DeleteUser(int userId)
        {
            _logger.LogInformation($"{_prefix}DeleteUser");
            return await _userData.DeleteUser(userId);
        }
    }
}