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

        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            _logger.LogInformation($"{_prefix}GetUserById");
            return await _userData.GetUserByIdAsync(id);
        }

        public async Task<UserDto?> AddUserAsync(UserDto user)
        {
            _logger.LogInformation($"{_prefix}AddUser");
            return await _userData.AddUserAsync(user);
        }

        public async Task<UserDto?> UpdateUserAsync(UpdateUserDto user)
        {
            _logger.LogInformation($"{_prefix}UpdateUser");
            return await _userData.UpdateUserAsync(user);
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            _logger.LogInformation($"{_prefix}DeleteUser");
            return await _userData.DeleteUserAsync(userId);
        }
        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            _logger.LogInformation($"{_prefix}GetAllUsersAsync called");
            return await _userData.GetAllUsersAsync();
        }
    }
}