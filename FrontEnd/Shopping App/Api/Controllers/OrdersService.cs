using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Serilog;
using Shopping_App.Hellpers;
using ShoppingApp.Api.Models;

namespace ShoppingApp.Api.Controllers
{
    public class OrdersService : ApiClient
    {
        public OrdersService(HttpClient httpClient) : base(httpClient)
        {
        }
        public async Task<List<OrderDto>> GetUserOrdersAsync(int userId)
        {
            Log.Information("Getting orders for user ID: {UserId}", userId);
            string queryString = $"userId={userId}";

            try
            {
                return await GetAsync<List<OrderDto>>(Config.GetApiEndpoint("Orders", "GetUserOrders"), queryString);
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == 404)
                {
                    Log.Error("No orders found for user ID: {UserId}", userId);
                    throw new ApiException(404, $"No orders found for user {userId}");
                }
                throw;
            }
        }
        public async Task<OrderDto> GetOrderByIdAsync(int orderId)
        {
            Log.Information("Getting order by ID: {OrderId}", orderId);
            string queryString = $"orderId={orderId}";

            try
            {
                return await GetAsync<OrderDto>(Config.GetApiEndpoint("Orders", "GetOrderById"), queryString);
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == 404)
                {
                    Log.Error("Order not found with ID: {OrderId}", orderId);
                    throw new ApiException(404, $"Order with ID {orderId} not found");
                }
                throw;
            }
        }
        public async Task<OrderDto> MakeOrderAsync(int userId, string shippingAddress, string paymentMethod)
        {
            Log.Information("Creating order for user ID: {UserId}", userId);
            string queryString = $"userId={userId}&shippingAddress={shippingAddress}&paymentMethod={paymentMethod}";

            OrderDto order;
            try
            {
                order = await PostAsync<OrderDto>(Config.GetApiEndpoint("Orders", "MakeOrder") + "?" + queryString, null);
                Config.SetCurrentUserCartId(0);
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == 400)
                {
                    Log.Error("Invalid order data for user ID: {UserId}", userId);
                    throw new ApiException(400, "Invalid order data. Please check your input.");
                }
                throw;
            }

            return order;
        }
        public async Task CancelOrderAsync(int orderId)
        {
            Log.Information("Cancelling order with ID: {OrderId}", orderId);
            string queryString = $"orderId={orderId}";

            try
            {
                await DeleteAsync(Config.GetApiEndpoint("Orders", "CancelOrder"), queryString);
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == 404)
                {
                    Log.Error("Order not found with ID: {OrderId}", orderId);
                    throw new ApiException(404, $"Order with ID {orderId} not found");
                }
                throw;
            }
        }
    }
}