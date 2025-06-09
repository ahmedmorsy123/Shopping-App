using Microsoft.Extensions.Logging;
using ShoppingAppDB;
using ShoppingAppDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ShoppingAppDB.Enums.Enums;

namespace ShoppingAppBussiness
{
    public class AdminService
    {
        private readonly AdminData _adminData;
        private readonly ILogger _logger;
        private const string _prefix = "AdminBL ";

        public AdminService(ILogger<AdminData> logger, AdminData adminData)
        {
            _logger = logger;
            _adminData = adminData;
        }

        public async Task<UserDto?> AddAdminAsync(UserDto admin)
        {
            _logger.LogInformation($"{_prefix}AddAdminAsync called for admin: {admin.Name}");
            return await _adminData.AddAdminAsync(admin);
        }

        public async Task<bool> RemoveAdminAsync(int adminId)
        {
            _logger.LogInformation($"{_prefix}RemoveAdminAsync called for adminId: {adminId}");
            return await _adminData.RemoveAdminAsync(adminId);
        }

        public async Task<bool> MakeAdminAsync(int userId)
        {
            _logger.LogInformation($"{_prefix}MakeAdminAsync called for userId: {userId}");
            return await _adminData.MakeAdminAsync(userId);
        }

        public async Task<List<UserDto>> ListAdminsAsync()
        {
            _logger.LogInformation($"{_prefix}ListAdminsAsync called");
            return await _adminData.ListAdminsAsync();
        }
    }
}
