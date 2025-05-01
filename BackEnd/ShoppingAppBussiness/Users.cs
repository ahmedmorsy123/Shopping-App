using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using ShoppingAppDB;
using static ShoppingAppDB.UserData;

namespace ShoppingAppBussiness
{
    public class Users
    {
        private ILogger<Users> _logger;
        private readonly UserData _userData;
        private const string _prefix = "UsersBL ";
        public Users(ILogger<Users> logger, UserData userData)
        {
            _logger = logger;
            _userData = userData;
        }
        public UserDto? GetUserById(int id)
        {
            _logger.LogInformation($"{_prefix}GetUserById");
            return _userData.GetUserById(id);
        }

        public int AddUser(UserDto user)
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
        public bool Login(string username, string password)
        {
            _logger.LogInformation($"{_prefix}Login");
            return _userData.Login(username, password);
        }

        public void Logout()
        {
            _logger.LogInformation($"{_prefix}Logout");
            _userData.ClearCurrentUser();
        }

        public UserDto? GetCurrentUser()
        {
            _logger.LogInformation($"{_prefix}GetCurrentUser");
            return _userData.GetCurrentUser();
        }
    }


}
