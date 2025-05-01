using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ShoppingAppDB.Data;
using ShoppingAppDB.Entities;
using ShoppingAppDB.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingAppDB
{
    public class UserData
    {
        public class UserDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string? Email { get; set; }
            public string? Password { get; set; } = null;

            public UserDto(int id, string name, string? email, string? password = null)
            {
                Id = id;
                Name = name;
                Email = email;
                Password = password;
            }
        }


        public static UserDto? _currentUser;
        private ILogger<UserData> _logger;
        private const string _prefix = "UserDA ";

        public UserData(ILogger<UserData> logger)
        {
            _logger = logger;
        }
        public UserDto? GetCurrentUser()
        {
            _logger.LogDebug($"{_prefix}Getting current user");
            return _currentUser;
        }

        public void SetCurrentUser(UserDto user)
        {
            _logger.LogInformation($"{_prefix}Set the current user");
            _currentUser = user;
        }

        public void ClearCurrentUser()
        {
            _logger.LogInformation($"{_prefix}Clearing the current user");
            _currentUser = null;
        }

        public  UserDto? GetUserById(int id)
        {
            _logger.LogInformation($"{_prefix}Getting user by id");
            UserDto? userDto;
            using (var context = new AppDbContext())
            {
                userDto = context.Users.AsNoTracking()
                    .Where(u => u.Id == id) 
                    .Select(u => new UserDto(id, u.Name, u.Email,null))
                    .FirstOrDefault(); 
            }
            _logger.LogDebug($"{_prefix}Is userDto null: {userDto == null}");
            return userDto;
        }

        public int AddUser(UserDto user)
        {
            _logger.LogInformation($"{_prefix}Adding user");
            using (var context = new AppDbContext())
            {
                User userToAdd = new User();

                userToAdd.Name = user.Name;
                userToAdd.Email = user.Email;
                userToAdd.PasswordHash = HashPassword(user.Password);
                userToAdd.CreatedAt = DateTime.Now;
                userToAdd.LastLogin = DateTime.Now;
                context.Users.Add(userToAdd);

                context.SaveChanges();
                _logger.LogInformation($"{_prefix}User added successfully with id {userToAdd.Id}");

                return userToAdd.Id;

            }
        }

        public bool UpdateUser(UserDto user, string oldPassword)
        {
            _logger.LogInformation($"{_prefix}Updated user: {user.Name}");
            if(_currentUser == null)
            {
                _logger.LogWarning($"{_prefix}Current user is null");
                return false;
            }
            if(!VerifyPassword(oldPassword, _currentUser.Password))
            {
                _logger.LogWarning($"{_prefix}Old password is incorrect");
                return false;
            }

            using (var context = new AppDbContext())
            {
                var userToUpdate = context.Users.Where(u => u.Id == user.Id).FirstOrDefault();
                if (userToUpdate != null)
                {
                    userToUpdate.Name = user.Name;
                    userToUpdate.Email = user.Email;
                    userToUpdate.PasswordHash = HashPassword(user.Password);
                    context.SaveChanges();
                    _logger.LogInformation($"{_prefix}User updated successfully with id {userToUpdate.Id}");

                    SetCurrentUser(new UserDto(userToUpdate.Id, userToUpdate.Name, userToUpdate.Email, userToUpdate.PasswordHash));
                    _logger.LogInformation($"{_prefix}Current user updated successfully with id {userToUpdate.Id}");
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

        public bool Login(string username, string password)
        {
            _logger.LogInformation($"{_prefix}Login");
            using (var context = new AppDbContext())
            {
                var user = context.Users.AsEnumerable().FirstOrDefault(u => u.Name == username);
                if (user != null && VerifyPassword(password, user.PasswordHash))
                {
                    user.LastLogin = DateTime.Now;
                    context.SaveChanges();
                    _logger.LogInformation($"{_prefix}User logged in successfully with id {user.Id}");
                    SetCurrentUser(new UserDto(user.Id, user.Name, user.Email, user.PasswordHash));
                    _logger.LogInformation($"{_prefix}Current user logged in successfully with id {user.Id}");
                    return true;
                }
                _logger.LogWarning($"{_prefix}Login failed");
                return false;
            }
        }

        public bool IsCorrectPassword(string password)
        {
            _logger.LogInformation($"{_prefix}Is correct password");
            return VerifyPassword(password, _currentUser.Password);
        }

        private const int WorkFactor = 10;
        public string HashPassword(string plainTextPassword)
        {
            _logger.LogInformation($"{_prefix}Hashing password");
            return BCrypt.Net.BCrypt.HashPassword(plainTextPassword, WorkFactor);
        }

        public bool VerifyPassword(string plainTextPassword, string hashedPassword)
        {
            _logger.LogInformation($"{_prefix}Verifying password");
            return BCrypt.Net.BCrypt.Verify(plainTextPassword, hashedPassword);
        }
    }
}
