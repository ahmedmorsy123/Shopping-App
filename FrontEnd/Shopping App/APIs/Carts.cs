using System.Collections.Generic;
using System;
using System.Net.Http;
using static Shopping_App.APIs.Products;
using System.Threading.Tasks;
using System.Text.Json;
using System.Windows.Forms;
using System.Text;

namespace Shopping_App.APIs
{
    public class Carts
    {
        public class Cart
        {
            public int CartId { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime UpdatedAt { get; set; }
            public List<Product> Products { get; set; }
        }
        static readonly HttpClient httpClient = new HttpClient();

        public static async Task<Cart> GetCurrentUserCart()
        {
            Cart CurrentCart = null;
            try
            {
                var response = await httpClient.GetAsync("http://localhost:5002/api/Carts/GetCurrentUserCart");
                var responseBody = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    CurrentCart = JsonSerializer.Deserialize<Cart>(responseBody, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                else
                {
                    Console.WriteLine(responseBody);
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"HTTP Request Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            catch (JsonException ex)
            {
                MessageBox.Show($"Error deserializing JSON: {ex.Message}", "JSON Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            return CurrentCart;
        }

        public static async Task<Cart> UpdateCart(Cart cart)
        {
            Cart updatedCart = null;
            try
            {
                string jsonPayload = JsonSerializer.Serialize(cart);

                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                var response = await httpClient.PutAsync("http://localhost:5002/api/Carts/UpdateCart", content);

                var responseBody = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    updatedCart = JsonSerializer.Deserialize<Cart>(responseBody, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                else
                {
                    Console.WriteLine(responseBody);
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"HTTP Request Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (JsonException ex)
            {
                MessageBox.Show($"Error deserializing JSON: {ex.Message}", "JSON Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return updatedCart;
        }

        public static async Task<Cart> AddCart(Cart cart)
        {
            Cart addedCart = null;
            try
            {
                string jsonPayload = JsonSerializer.Serialize(cart);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync("http://localhost:5002/api/Carts/AddCart", content);

                var responseBody = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    addedCart = JsonSerializer.Deserialize<Cart>(responseBody, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                else
                {
                    Console.WriteLine(responseBody);
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"HTTP Request Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (JsonException ex)
            {
                MessageBox.Show($"Error deserializing JSON: {ex.Message}", "JSON Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return addedCart;
        }

        public static async Task DeleteCart(int cartId)
        {
            try
            {
                var response = await httpClient.DeleteAsync($"http://localhost:5002/api/Carts/DeleteCart?cartId={cartId}");
                var responseBody = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine(responseBody);
                    return;
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"HTTP Request Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
