using System.Collections.Generic;
using System;
using System.Net.Http;
using static Shopping_App.APIs.Products;
using System.Threading.Tasks;
using System.Text.Json;
using System.Windows.Forms;
using Serilog;

namespace Shopping_App.APIs
{
    public class Orders
    {
        public class Order
        {
            public int Id { get; set; }
            public DateTime CreatedAt { get; set; }
            public decimal TotalPrice { get; set; }
            public string Status { get; set; }
            public string ShippingAddress { get; set; }
            public string PaymentMethod { get; set; }
            public List<Product> OrderItems { get; set; }

        }
        static readonly HttpClient httpClient = new HttpClient();

        public static async Task<List<Order>> GetCurrentUserOrders()
        {
            Log.Information("Getting current user orders");
            List<Order> orders = null;
            try
            {
                Log.Information("Making get request");
                var response = await httpClient.GetAsync($"http://localhost:5002/api/Orders/GetCurrentUserOrders");
                var responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    Log.Information("Success status code");
                    orders = JsonSerializer.Deserialize<List<Order>>(responseBody, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    Console.WriteLine($"orders is null: {orders == null}");
                }
                else
                {
                    Log.Information("Faild status code");
                    Console.WriteLine(responseBody);
                }
            }
            catch (HttpRequestException ex)
            {
                Log.Error(ex, "Http request error");
                MessageBox.Show($"HTTP Request Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            catch (JsonException ex)
            {
                Log.Error(ex, "Json deserialization error");
                MessageBox.Show($"Error deserializing JSON: {ex.Message}", "JSON Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            return orders;
        }

        public static async Task<Order> GetOrderById(int orderId)
        {
            Log.Information("Get Order By Id");
            Order order = null;
            try
            {
                Log.Information("Making Get request");
                var response = await httpClient.GetAsync($"http://localhost:5002/api/Orders/GetOrderById?orderId={orderId}");
                var responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    Log.Information("Success status code");
                    order = JsonSerializer.Deserialize<Order>(responseBody, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                else
                {
                    Log.Information("Faild status code");
                    Console.WriteLine(responseBody);
                }
            }
            catch (HttpRequestException ex)
            {
                Log.Error(ex, "Http request exceptoin");
                MessageBox.Show($"HTTP Request Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            catch (JsonException ex)
            {
                Log.Error(ex, "Json deserialization error");
                MessageBox.Show($"Error deserializing JSON: {ex.Message}", "JSON Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            return order;
        }

        public static async Task<Order> MakeOrder(string shippingAddress, string paymentMethod)
        {
            Log.Information("Making order");
            Order order = null;
            try
            {
                Log.Information("Making post request");
                var response = await httpClient.PostAsync($"http://localhost:5002/api/Orders/MakeOrders?shippingAddress={shippingAddress}&paymentMethod={paymentMethod}", null);
                var responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode) {
                    Log.Information("Success status code");
                    order = JsonSerializer.Deserialize<Order>(responseBody, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                else
                {
                    Log.Information("Faild status code");
                    Console.WriteLine(responseBody);
                }
            }
            catch (HttpRequestException ex) {
                Log.Error(ex, "Http request exceptoin");
                MessageBox.Show($"HTTP Request Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            catch (JsonException ex)
            {
                Log.Error(ex, "Json deserialization error");
                MessageBox.Show($"Error deserializing JSON: {ex.Message}", "JSON Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            return order;
        }

        public static async Task CancelOrder(int orderId)
        {
            Log.Information("Canceling order");
            try
            {
                Log.Information("Making delete request");
                var response = await httpClient.DeleteAsync($"http://localhost:5002/api/Orders/CancelOrder?orderId={orderId}");
                var responseBody = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    Log.Information("Faild status code");
                    Console.WriteLine(responseBody);
                    return;
                }
            }
            catch (HttpRequestException ex)
            {
                Log.Error(ex, "Http request exception");
                MessageBox.Show($"HTTP Request Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
