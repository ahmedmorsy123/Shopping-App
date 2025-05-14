using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ShoppingAppDB.Data;
using ShoppingAppDB.Entities;
using ShoppingAppDB.Models;
using ShoppingAppDB.Services;

namespace ShoppingAppDB
{
    public class UserData
    {
        private ILogger<UserData> _logger;
        private Password _passwordService;
        private const string _prefix = "UserDA ";

        public UserData(ILogger<UserData> logger, Password passwordService)
        {
            _logger = logger;
            _passwordService = passwordService;
        }

        public UserDto? GetUserById(int id)
        {
            _logger.LogInformation($"{_prefix}Getting user by id");
            UserDto? userDto;
            using (var context = new AppDbContext())
            {
                userDto = context.Users.AsNoTracking()
                    .Where(u => u.Id == id)
                    .Select(u => new UserDto(id, u.Name, u.Email, null))
                    .FirstOrDefault();
            }
            _logger.LogDebug($"{_prefix}Is userDto null: {userDto == null}");
            return userDto;
        }

        public UserDto? AddUser(UserDto user)
        {
            _logger.LogInformation($"{_prefix}Adding user");
            if (user == null)
            {
                _logger.LogWarning($"{_prefix}User is null");
                return null;
            }
            using (var context = new AppDbContext())
            {
                if (context.Users.Any(u => u.Email == user.Email || u.Name == user.Name))
                {
                    _logger.LogWarning($"{_prefix}User with email {user.Email} already exists");
                    return null;
                }

                User userToAdd = new User();

                userToAdd.Name = user.Name;
                userToAdd.Email = user.Email;
                userToAdd.PasswordHash = _passwordService.HashPassword(user.Password!);
                userToAdd.CreatedAt = DateTime.Now;
                userToAdd.LastLogin = DateTime.Now;
                context.Users.Add(userToAdd);

                context.SaveChanges();
                _logger.LogInformation($"{_prefix}User added successfully with id {userToAdd.Id}");

                return new UserDto(userToAdd.Id, userToAdd.Name, userToAdd.Email, userToAdd.PasswordHash);
            }
        }

        public bool UpdateUser(UserDto user, string oldPassword)
        {
            using (var context = new AppDbContext())
            {
                var userToUpdate = context.Users.Where(u => u.Id == user.Id).FirstOrDefault();
                if (userToUpdate != null && _passwordService.VerifyPassword(oldPassword, userToUpdate.PasswordHash))
                {
                    userToUpdate.Name = string.IsNullOrEmpty(user.Name) ? userToUpdate.Name : user.Name;
                    userToUpdate.Email = string.IsNullOrEmpty(user.Email) ? userToUpdate.Email : user.Email;
                    userToUpdate.PasswordHash = string.IsNullOrEmpty(user.Password) ? userToUpdate.PasswordHash : _passwordService.HashPassword(user.Password);
                    context.SaveChanges();
                    _logger.LogInformation($"{_prefix}User updated successfully with id {userToUpdate.Id}");

                    return true;
                }
                return false;
            }
        }

        public bool DeleteUser(int userId)
        {
            _logger.LogInformation($"{_prefix}Deleted user with id {userId}");
            using (var context = new AppDbContext())
            {
                var userToDelete = context.Users.Find(userId);
                if (userToDelete != null)
                {
                    context.Users.Remove(userToDelete);
                    context.SaveChanges();
                    _logger.LogInformation($"{_prefix}user with id {userId} deleted");
                    return true;
                }
            }
            return false;
        }
    }
}