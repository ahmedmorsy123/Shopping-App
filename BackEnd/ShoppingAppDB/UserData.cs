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

        public async Task<UserDto?> GetUserById(int id)
        {
            _logger.LogInformation($"{_prefix}Getting user by id");
            UserDto? userDto;
            using (var context = new AppDbContext())
            {
                userDto = await context.Users.AsNoTracking()
                    .Where(u => u.Id == id)
                    .Select(u => new UserDto(id, u.Name, u.Email, null))
                    .FirstOrDefaultAsync();
            }
            _logger.LogDebug($"{_prefix}Is userDto null: {userDto == null}");
            return userDto;
        }

        public async Task<UserDto?> AddUser(UserDto user)
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
                await context.Users.AddAsync(userToAdd);

                await context.SaveChangesAsync();
                _logger.LogInformation($"{_prefix}User added successfully with id {userToAdd.Id}");

                return new UserDto(userToAdd.Id, userToAdd.Name, userToAdd.Email, userToAdd.PasswordHash);
            }
        }

        public async Task<UserDto?> UpdateUser(UserDto user, string oldPassword)
        {
            using (var context = new AppDbContext())
            {
                var userToUpdate = await context.Users.Where(u => u.Id == user.Id).FirstOrDefaultAsync();
                if (userToUpdate != null && _passwordService.VerifyPassword(oldPassword, userToUpdate.PasswordHash))
                {
                    userToUpdate.Name = string.IsNullOrEmpty(user.Name) ? userToUpdate.Name : user.Name;
                    userToUpdate.Email = string.IsNullOrEmpty(user.Email) ? userToUpdate.Email : user.Email;
                    userToUpdate.PasswordHash = string.IsNullOrEmpty(user.Password) ? userToUpdate.PasswordHash : _passwordService.HashPassword(user.Password);

                    await context.SaveChangesAsync();
                    _logger.LogInformation($"{_prefix}User updated successfully with id {userToUpdate.Id}");

                    return new UserDto(userToUpdate.Id, userToUpdate.Name, userToUpdate.Email, null);
                }
                return null;
            }
        }

        public async Task<bool> DeleteUser(int userId)
        {
            _logger.LogInformation($"{_prefix}Deleted user with id {userId}");
            using (var context = new AppDbContext())
            {
                var userToDelete = await context.Users.FindAsync(userId);
                if (userToDelete != null)
                {
                    context.Users.Remove(userToDelete);
                    await context.SaveChangesAsync();
                    _logger.LogInformation($"{_prefix}user with id {userId} deleted");
                    return true;
                }
            }
            return false;
        }
    }
}