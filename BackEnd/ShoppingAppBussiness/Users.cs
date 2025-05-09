using Microsoft.Extensions.Logging;
using ShoppingAppDB;
using ShoppingAppDB.Models;
using ShoppingAppDB.Services;

namespace ShoppingAppBussiness
{
    public class Users
    {
        private ILogger<Users> _logger;
        private UserData _userData;
        private AuthService _authService;
        private const string _prefix = "UsersBL ";

        public Users(ILogger<Users> logger, UserData userData, AuthService authService)
        {
            _logger = logger;
            _userData = userData;
            _authService = authService;
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

        public TokenResponseDto? Login(string username, string password)
        {
            _logger.LogInformation($"{_prefix}Login");
            return _userData.Login(username, password);
        }

        public bool Logout(int userId)
        {
            _logger.LogInformation($"{_prefix}Logout");
            return _authService.Logout(userId);
        }
    }
}