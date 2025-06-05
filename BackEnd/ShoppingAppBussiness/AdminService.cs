using Microsoft.Extensions.Logging;
using ShoppingAppDB;
using ShoppingAppDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            _logger.LogInformation($"{_prefix}GetAllUsersAsync called");
            return await _adminData.GetAllUsersAsync();
        }

        public async Task<bool> ProcessOrderAsync(int orderId)
        {
            _logger.LogInformation($"{_prefix}ProcessOrderAsync called for orderId: {orderId}");
            return await _adminData.ProcessOrderAsync(orderId);
        }

        public async Task<bool> ShipOrderAsync(int orderId)
        {
            _logger.LogInformation($"{_prefix}ShipOrderAsync called for orderId: {orderId}");
            return await _adminData.ShipOrderAsync(orderId);
        }

        public async Task<bool> DeliverOrderAsync(int orderId)
        {
            _logger.LogInformation($"{_prefix}DeliverOrderAsync called for orderId: {orderId}");
            return await _adminData.DeliverOrderAsync(orderId);
        }

        public async Task<List<ProductDto>> GetOutOfStockProductsAsync()
        {
            _logger.LogInformation($"{_prefix}GetOutOfStockProductsAsync called");
            return await _adminData.GetOutOfStockProductsAsync();
        }

        public async Task<List<ProductDto>> GetLowStockProductsAsync(int threshold)
        {
            _logger.LogInformation($"{_prefix}GetLowStockProductsAsync called with threshold: {threshold}");
            return await _adminData.GetLowStockProductsAsync(threshold);
        }

        public async Task<bool> StockProductAsync(int productId, int quantity)
        {
            _logger.LogInformation($"{_prefix}StockProductAsync called for productId: {productId}, quantity: {quantity}");
            return await _adminData.StockProductAsync(productId, quantity);
        }

        public async Task<List<OrderDto>> GetOrdersByDurationAndStatusAsync(AdminData.TimeDeuration duration, AdminData.OrderStatus status)
        {
            _logger.LogInformation($"{_prefix}GetOrdersByDurationAndStatusAsync called with duration: {duration}, status: {status}");
            return await _adminData.GetOrdersByDurationAndStatusAsync(duration, status);
        }

        public async Task<int> GetLoginCountByDurationAsync(AdminData.TimeDeuration duration)
        {
            _logger.LogInformation($"{_prefix}GetLoginCountByDurationAsync called with duration: {duration}");
            return await _adminData.GetLoginCountByDurationAsync(duration);
        }

        public async Task<int> GetRegisterationCountByDurationAsync(AdminData.TimeDeuration duration)
        {
            _logger.LogInformation($"{_prefix}GetRegisterationCountByDurationAsync called with duration: {duration}");
            return await _adminData.GetRegisterationCountByDurationAsync(duration);
        }

        public async Task<int> GetCartsCountAsync()
        {
            _logger.LogInformation($"{_prefix}GetCartsCountAsync called");
            return await _adminData.GetCartsCountAsync();
        }
    }
}
