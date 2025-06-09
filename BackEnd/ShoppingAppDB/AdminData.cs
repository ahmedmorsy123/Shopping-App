using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShoppingAppDB.Data;
using ShoppingAppDB.Entities;
using ShoppingAppDB.Models;
using ShoppingAppDB.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ShoppingAppDB.Enums.Enums;

namespace ShoppingAppDB
{
    public class AdminData
    {
        private ILogger<AdminData> _logger;
        private Password _passwordService;
        private const string _prefix = "AdminDA ";
        
        public AdminData(ILogger<AdminData> logger, Password passwordService)
        {
            _logger = logger;
            _passwordService = passwordService;
        }

        public async Task<UserDto?> AddAdminAsync(UserDto admin)
        {
            _logger.LogInformation($"{_prefix}Adding admin");
            if (admin == null)
            {
                _logger.LogWarning($"{_prefix}admin is null");
                return null;
            }

            using (var context = new AppDbContext())
            {
                if (context.Users.Any(u => u.Email == admin.Email))
                {
                    _logger.LogWarning($"{_prefix}admin with email {admin.Email} already exists");
                    return null;
                }
                if (context.Users.Any(u => u.Name == admin.Name))
                {
                    _logger.LogWarning($"{_prefix}admin with UserName {admin.Email} already exists");
                    return null;
                }

                User adminToAdd = new User();

                adminToAdd.Name = admin.Name;
                adminToAdd.Email = admin.Email;
                adminToAdd.PasswordHash = _passwordService.HashPassword(admin.Password!);
                adminToAdd.CreatedAt = DateTime.Now;
                adminToAdd.LastLogin = DateTime.Now;
                adminToAdd.Role = "Admin";
                await context.Users.AddAsync(adminToAdd);

                await context.SaveChangesAsync();
                _logger.LogInformation($"{_prefix}Admin added successfully with id {adminToAdd.Id}");

                return new UserDto(adminToAdd.Id, adminToAdd.Name, adminToAdd.Email, adminToAdd.PasswordHash);
            }
        }
        public async Task<bool> RemoveAdminAsync(int adminId)
        {
            _logger.LogInformation($"{_prefix}Deleted admin with id {adminId}");
            using (var context = new AppDbContext())
            {
                var userToDelete = await context.Users
                    .FirstOrDefaultAsync(u => u.Role == "Admin" && u.Id == adminId);

                if (userToDelete != null)
                {
                    context.Users.Remove(userToDelete);
                    await context.SaveChangesAsync();
                    _logger.LogInformation($"{_prefix}Admin with id {adminId} deleted");
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> MakeAdminAsync(int userId)
        {
            _logger.LogInformation($"{_prefix}Making user with id {userId} an admin");
            using(var context = new AppDbContext())
            {
                var userToUpdate = await context.Users.FindAsync(userId);
                if (userToUpdate != null)
                {
                    userToUpdate.Role = "Admin";
                    await context.SaveChangesAsync();
                    _logger.LogInformation($"{_prefix}User with id {userId} is now an admin");
                    return true;
                }
                else
                {
                    _logger.LogWarning($"{_prefix}User with id {userId} not found");
                    return false;
                }
            }
        }
        public async Task<List<UserDto>> ListAdminsAsync()
        {
            _logger.LogInformation($"{_prefix}Listing all admins");
            using (var context = new AppDbContext())
            {
                var admins = await context.Users.AsNoTracking()
                    .Where(u => u.Role == "Admin")
                    .Select(u => new UserDto(u.Id, u.Name, u.Email, null))
                    .ToListAsync();
                
                if (admins.Count == 0)
                {
                    _logger.LogWarning($"{_prefix}No admins found");
                    return new List<UserDto>();
                }
                _logger.LogInformation($"{_prefix}Found {admins.Count} admins");
                return admins;
            }
        }
    }
}
