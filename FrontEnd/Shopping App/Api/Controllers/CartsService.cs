using System.IO;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Serilog;
using ShoppingApp.Api.Models;
using Shopping_App.Hellpers;

namespace ShoppingApp.Api.Controllers
{
    public class CartsService : ApiClient
    {
        public CartsService(HttpClient httpClient) : base(httpClient)
        {
        }
        public async Task<CartDto> GetUserCartAsync(int userId)
        {
            Log.Information("Getting cart for user ID: {UserId}", userId);
            string queryString = $"UserId={userId}";

            try
            {
                return await GetAsync<CartDto>(Config.GetApiEndpoint("Carts", "GetUserCart"), queryString);
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == 404)
                {
                    Log.Error("Cart not found for user ID: {UserId}", userId);
                    throw new ApiException(404, $"Cart for user {userId} not found");
                }else if (ex.StatusCode == 401)
                {
                    Log.Error("Unauthorized to access cart for user ID: {UserId}", userId);
                    throw new ApiException(401, "Unauthorized to access this cart");
                }
                else
                {
                    throw;
                }
            }
        }
        public async Task<CartDto> UpdateCartAsync(CartDto cart)
        {
            Log.Information("Updating cart with ID: {CartId}", cart.CartId);
            try
            {
                return await PutAsync<CartDto>(Config.GetApiEndpoint("Carts", "UpdateCart"), cart);
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == 401)
                {
                    Log.Error("Unauthorized to update cart with ID: {CartId}", cart.CartId);
                    throw new ApiException(401, "Unauthorized to update this cart");
                }
                else
                {
                    throw;
                }
            }
        }
        public async Task<CartDto> AddCartAsync(CartDto cart)
        {
            Log.Information("Adding new cart for user ID: {UserId}", cart.UserId);
            try
            {
                return await PostAsync<CartDto>(Config.GetApiEndpoint("Carts", "AddCart"), cart);
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == 401)
                {
                    Log.Error("Unauthorized to create cart for user ID: {UserId}", cart.UserId);
                    throw new ApiException(401, "Unauthorized to create a cart");
                }
                else
                {
                    throw;
                }
            }
        }
        public async Task DeleteCartAsync(int cartId)
        {
            Log.Information("Deleting cart with ID: {CartId}", cartId);
            string queryString = $"cartId={cartId}";

            try
            {
                await DeleteAsync(Config.GetApiEndpoint("Carts", "DeleteCart"), queryString);
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == 404)
                {
                    Log.Error("Cart not found for ID: {CartId}", cartId);
                    throw new ApiException(404, $"Cart with ID {cartId} not found");
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<int> GetCartsCountAsync()
        {
            Log.Information("Getting total number of carts");
            try
            {
                string endpoint = Config.GetApiEndpoint("Carts", "GetCartsCount");
                return await GetAsync<int>(endpoint);
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == 401)
                {
                    Log.Error("Unauthorized to access carts count");
                    throw new ApiException(401, "Unauthorized to access carts count");
                } else if(ex.StatusCode == 403)
                {
                    Log.Error("Forbidden access to carts count");
                    throw new ApiException(403, "Forbidden access to carts count");
                }
                else
                {
                    throw;
                }
            }
        }
    }
}