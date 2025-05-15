using Microsoft.Extensions.Logging;
using ShoppingAppDB;
using ShoppingAppDB.Models;

namespace ShoppingAppBussiness
{
    public class OrdersService
    {
        private ILogger<OrdersService> _logger;
        private readonly OrderData _orderData;
        private const string _prefix = "OrdersBL ";

        public OrdersService(ILogger<OrdersService> logger, OrderData orderData)
        {
            _logger = logger;
            _orderData = orderData;
        }

        public async Task<List<OrderDto>?> GetUserOrders(int UserId)
        {
            _logger.LogInformation($"{_prefix}GetUserOrders");
            return await _orderData.GetUserOrders(UserId);
        }

        public async Task<OrderDto> AddOrder(int userId, string shippingAddress, string paymentMethod)
        {
            _logger.LogInformation($"{_prefix}AddOrder");
            return await _orderData.AddOrder(userId, shippingAddress, paymentMethod);
        }

        public async Task<OrderDto?> GetOrderById(int OrderId)
        {
            _logger.LogInformation($"{_prefix}GetOrderById");
            return await _orderData.GetOrderById(OrderId);
        }

        public async Task CancelOrder(int id)
        {
            _logger.LogInformation($"{_prefix}CancelOrder");
            await _orderData.CancelOrder(id);
        }
    }
}