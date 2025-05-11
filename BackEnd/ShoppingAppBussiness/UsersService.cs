using Microsoft.Extensions.Logging;
using ShoppingAppDB;
using ShoppingAppDB.Models;
using ShoppingAppDB.Services;

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

        public UserDto? GetUserById(int id)
        {
            _logger.LogInformation($"{_prefix}GetUserById");
            return _userData.GetUserById(id);
        }

        public UserDto? AddUser(UserDto user)
        {
            _logger.LogInformation($"{_prefix}AddUser");
            return _userData.AddUser(user);
        }

        public bool UpdateUser(UserDto user, string oldPassword)
        {
            _logger.LogInformation($"{_prefix}UpdateUser");
            return _userData.UpdateUser(user, oldPassword);
        }

        public bool DeleteUser(int userId)
        {
            _logger.LogInformation($"{_prefix}DeleteUser");
            return _userData.DeleteUser(userId);
        }
    }
}