using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Serilog;
using Shopping_App.Hellpers;
using ShoppingApp.Api.Models;
using static ShoppingAppDB.Enums.Enums;

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
                    return new List<OrderDto>();
                }
                else if (ex.StatusCode == 401)
                {
                    Log.Error("Unauthorized access to orders for user ID: {UserId}", userId);
                    throw new ApiException(401, "Unauthorized access to this user's orders");
                }
                else
                {
                    throw;
                }
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
                } else if(ex.StatusCode == 401)
                {
                    Log.Error("Unauthorized access to order with ID: {OrderId}", orderId);
                    throw new ApiException(401, "Unauthorized access to this order");
                }
                else
                {
                    throw;
                }
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
                else if (ex.StatusCode == 401)
                {
                    Log.Error("Unauthorized to create order for user ID: {UserId}", userId);
                    throw new ApiException(401, "Unauthorized to create this order");
                }
                else if (ex.StatusCode == 404)
                {
                    Log.Error("Cart not found for user ID: {UserId}", userId);
                    throw new ApiException(404, $"Cart for user {userId} not found. Please add items to your cart before ordering.");
                }
                else
                {
                    throw;
                }
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
                } else if(ex.StatusCode == 401)
                {
                    Log.Error("Unauthorized to cancel order with ID: {OrderId}", orderId);
                    throw new ApiException(401, "Unauthorized to cancel this order");
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task ProcessOrderAsync(int orderId)
        {
            Log.Information("Processing order with ID: {OrderId}", orderId);
            string queryString = $"orderId={orderId}";
            try
            {
                await PutAsync(Config.GetApiEndpoint("Orders", "ProcessOrder"), queryString);
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == 404)
                {
                    Log.Error("Order not found with ID: {OrderId}", orderId);
                    throw new ApiException(404, $"Order with ID {orderId} not found");
                }
                else if (ex.StatusCode == 401)
                {
                    Log.Error("Unauthorized to process order with ID: {OrderId}", orderId);
                    throw new ApiException(401, "Unauthorized to process this order");
                } else if (ex.StatusCode == 403)
                {
                    Log.Error("Forbidden to process order with ID: {OrderId}", orderId);
                    throw new ApiException(403, "You do not have permission to process this order");
                } else if(ex.StatusCode == 400)
                {
                    Log.Error("Invalid order Id {OrderId}, It must be posstive or or order status not pending", orderId);
                    throw new ApiException(400, $"Invalid order Id {orderId}, It must be posstive or or order status not pending");
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task ShipOrderAsync(int orderId)
        {
            Log.Information("Shipping order with ID: {OrderId}", orderId);
            string queryString = $"orderId={orderId}";
            try
            {
                await PutAsync(Config.GetApiEndpoint("Orders", "ShipOrder"), queryString);
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == 404)
                {
                    Log.Error("Order not found with ID: {OrderId}", orderId);
                    throw new ApiException(404, $"Order with ID {orderId} not found");
                }
                else if (ex.StatusCode == 401)
                {
                    Log.Error("Unauthorized to ship order with ID: {OrderId}", orderId);
                    throw new ApiException(401, "Unauthorized to ship this order");
                }
                else if (ex.StatusCode == 403)
                {
                    Log.Error("Forbidden to ship order with ID: {OrderId}", orderId);
                    throw new ApiException(403, "You do not have permission to ship this order");
                }
                else if (ex.StatusCode == 400)
                {
                    Log.Error("Invalid order Id {OrderId}, It must be posstive or or order status not processed", orderId);
                    throw new ApiException(400, $"Invalid order Id {orderId}, It must be posstive or or order status not processed");
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task DeliverOrderAsync(int orderId)
        {
            Log.Information("Delivering order with ID: {OrderId}", orderId);
            string queryString = $"orderId={orderId}";
            try
            {
                await PutAsync(Config.GetApiEndpoint("Orders", "DeliverOrder"), queryString);
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == 404)
                {
                    Log.Error("Order not found with ID: {OrderId}", orderId);
                    throw new ApiException(404, $"Order with ID {orderId} not found");
                }
                else if (ex.StatusCode == 401)
                {
                    Log.Error("Unauthorized to deliver order with ID: {OrderId}", orderId);
                    throw new ApiException(401, "Unauthorized to deliver this order");
                }
                else if (ex.StatusCode == 403)
                {
                    Log.Error("Forbidden to deliver order with ID: {OrderId}", orderId);
                    throw new ApiException(403, "You do not have permission to deliver this order");
                }
                else if (ex.StatusCode == 400)
                {
                    Log.Error("Invalid order Id {OrderId}, It must be posstive or or order status not shipped", orderId);
                    throw new ApiException(400, $"Invalid order Id {orderId}, It must be posstive or or order status not shipped");
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<List<OrderDto>> GetOrdersByDurationAndStatusAsync(TimeDuration duration, OrderStatus status)
        {
            Log.Information("Getting orders by duration: {Duration} and status: {Status}", duration, status);
            string queryString = $"duration={duration}&status={status}";

            try
            {
                return await GetAsync<List<OrderDto>>(Config.GetApiEndpoint("Orders", "GetOrdersByDurationAndStatus"), queryString);
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == 404)
                {
                    Log.Error("No orders found for duration: {Duration} and status: {Status}", duration, status);
                    return new List<OrderDto>();
                }
                else if (ex.StatusCode == 401)
                {
                    Log.Error("Unauthorized access to orders for duration: {Duration} and status: {Status}", duration, status);
                    throw new ApiException(401, "Unauthorized access to these orders");
                }
                else if (ex.StatusCode == 403)
                {
                    Log.Error("Forbidden access to orders for duration: {Duration} and status: {Status}", duration, status);
                    throw new ApiException(403, "You do not have permission to access these orders");
                }
                else
                {
                    throw;
                }
            }
        }
    }
}