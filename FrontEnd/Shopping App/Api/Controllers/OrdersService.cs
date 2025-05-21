using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Serilog;
using ShoppingApp.Api.Models;

namespace ShoppingApp.Api.Controllers
{
    public class OrdersService : ApiClient
    {
        private const string GET_USER_ORDERS_ENDPOINT = "/api/Orders/GetUserOrders";
        private const string GET_ORDER_BY_ID_ENDPOINT = "/api/Orders/GetOrderById";
        private const string MAKE_ORDERS_ENDPOINT = "/api/Orders/MakeOrders";
        private const string CANCEL_ORDER_ENDPOINT = "/api/Orders/CancelOrder";

        public OrdersService(HttpClient httpClient) : base(httpClient)
        {
        }

        /// <summary>
        /// Gets all orders for a specific user
        /// </summary>
        /// <param name="userId">User ID to get orders for</param>
        /// <returns>List of user's orders</returns>
        public async Task<List<OrderDto>> GetUserOrdersAsync(int userId)
        {
            Log.Information("Getting orders for user ID: {UserId}", userId);
            string queryString = $"userId={userId}";

            try
            {
                return await GetAsync<List<OrderDto>>(GET_USER_ORDERS_ENDPOINT, queryString);
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

        /// <summary>
        /// Gets a specific order by ID
        /// </summary>
        /// <param name="orderId">Order ID to retrieve</param>
        /// <returns>Order information</returns>
        public async Task<OrderDto> GetOrderByIdAsync(int orderId)
        {
            Log.Information("Getting order by ID: {OrderId}", orderId);
            string queryString = $"orderId={orderId}";

            try
            {
                return await GetAsync<OrderDto>(GET_ORDER_BY_ID_ENDPOINT, queryString);
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

        /// <summary>
        /// Creates a new order
        /// </summary>
        /// <param name="userId">User ID placing the order</param>
        /// <param name="shippingAddress">Shipping address for the order</param>
        /// <param name="paymentMethod">Payment method for the order</param>
        /// <returns>Created order information</returns>
        public async Task<OrderDto> MakeOrderAsync(int userId, string shippingAddress, string paymentMethod)
        {
            Log.Information("Creating order for user ID: {UserId}", userId);
            string queryString = $"userId={userId}&shippingAddress={shippingAddress}&paymentMethod={paymentMethod}";

            try
            {
                return await PostAsync<OrderDto>(MAKE_ORDERS_ENDPOINT + "?" + queryString, null);
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
        }

        /// <summary>
        /// Cancels an existing order
        /// </summary>
        /// <param name="orderId">Order ID to cancel</param>
        public async Task CancelOrderAsync(int orderId)
        {
            Log.Information("Cancelling order with ID: {OrderId}", orderId);
            string queryString = $"orderId={orderId}";

            try
            {
                await DeleteAsync(CANCEL_ORDER_ENDPOINT, queryString);
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