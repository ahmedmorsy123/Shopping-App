﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using ShoppingAppDB;
using static ShoppingAppDB.OrderData;

namespace ShoppingAppBussiness
{
    public class Orders
    {
        private ILogger<Orders> _logger;
        private readonly OrderData _orderData;
        private const string _prefix = "OrdersBL ";
        public Orders(ILogger<Orders> logger, OrderData orderData)
        {
            _logger = logger;
            _orderData = orderData;
        }
        public List<OrderDto>? GetUserOrders(int UserId)
        {
            _logger.LogInformation($"{_prefix}GetUserOrders");
            return _orderData.GetUserOrders(UserId);
        }

        public OrderDto AddOrder(string shippingAddress, string paymentMethod)
        {
            _logger.LogInformation($"{_prefix}AddOrder");
            return _orderData.AddOrder(shippingAddress, paymentMethod);
        }

        public OrderDto? GetOrderById(int id)
        {
            _logger.LogInformation($"{_prefix}GetOrderById");
            return _orderData.GetOrderById(id);
        }

        public void CancelOrder(int id)
        {
            _logger.LogInformation($"{_prefix}CancelOrder");
            _orderData.CancelOrder(id);
        }

    }
}
