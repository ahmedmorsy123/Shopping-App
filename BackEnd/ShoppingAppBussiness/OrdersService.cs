using Microsoft.Extensions.Logging;
using ShoppingAppDB;
using ShoppingAppDB.Models;
using static ShoppingAppDB.Enums.Enums;

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

        public async Task<List<OrderDto>?> GetUserOrdersAsync(int UserId)
        {
            _logger.LogInformation($"{_prefix}GetUserOrders");
            return await _orderData.GetUserOrdersAsync(UserId);
        }

        public async Task<OrderDto?> AddOrderAsync(int userId, string shippingAddress, string paymentMethod)
        {
            _logger.LogInformation($"{_prefix}AddOrder");
            return await _orderData.AddOrderAsync(userId, shippingAddress, paymentMethod);
        }

        public async Task<OrderDto?> GetOrderByIdAsync(int OrderId)
        {
            _logger.LogInformation($"{_prefix}GetOrderById");
            return await _orderData.GetOrderByIdAsync(OrderId);
        }

        public async Task CancelOrderAsync(int id)
        {
            _logger.LogInformation($"{_prefix}CancelOrder");
            await _orderData.CancelOrderAsync(id);
        }

        public async Task<bool> ProcessOrderAsync(int orderId)
        {
            _logger.LogInformation($"{_prefix}ProcessOrderAsync called for orderId: {orderId}");
            return await _orderData.ProcessOrderAsync(orderId);
        }

        public async Task<bool> ShipOrderAsync(int orderId)
        {
            _logger.LogInformation($"{_prefix}ShipOrderAsync called for orderId: {orderId}");
            return await _orderData.ShipOrderAsync(orderId);
        }

        public async Task<bool> DeliverOrderAsync(int orderId)
        {
            _logger.LogInformation($"{_prefix}DeliverOrderAsync called for orderId: {orderId}");
            return await _orderData.DeliverOrderAsync(orderId);
        }


        public async Task<List<OrderDto>> GetOrdersByDurationAndStatusAsync(TimeDuration duration, OrderStatus status)
        {
            _logger.LogInformation($"{_prefix}GetOrdersByDurationAndStatusAsync called with duration: {duration}, status: {status}");
            return await _orderData.GetOrdersByDurationAndStatusAsync(duration, status);
        }

        public async Task<OrderStatus> GetOrderStatusAsync(int orderId)
        {
            return await _orderData.GetOrderStatusAsync(orderId);
        }
    }
}