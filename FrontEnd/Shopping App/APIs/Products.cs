using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shopping_App.APIs
{
    public class Products
    {
        public class Product
        {
            public int Id { get; set; }
            public string ProductName { get; set; }
            public string ProductCategory { get; set; }
            public string ProductDescription { get; set; }
            public int Quantity { get; set; }
            public decimal Weight { get; set; }
            public decimal Price { get; set; }
        }

        static readonly HttpClient httpClient = new HttpClient();

        public static async Task<List<Product>> GetProductsAsPaginated(int pageNumber)
        {
            Log.Information("Get Products As Paginated");
            List<Product> products = new List<Product>();

            try
            {
                Log.Information("Making get request");
                var response = await httpClient.GetAsync($"http://localhost:5002/api/Products/GetAllProductsPaginated?page={pageNumber}");
                var responseBody = await response.Content.ReadAsStringAsync();

                products = JsonSerializer.Deserialize<List<Product>>(responseBody, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true 
                });

            
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

            return products;
        }
    }
}

