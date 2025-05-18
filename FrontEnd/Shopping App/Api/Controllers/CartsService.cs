using System.Threading.Tasks;
using ShoppingApp.Api.Models;

namespace ShoppingApp.Api.Controllers
{
    public class CartsService : ApiClient
    {
        private const string GET_USER_CART_ENDPOINT = "/api/Carts/GetUserCart";
        private const string UPDATE_CART_ENDPOINT = "/api/Carts/UpdateCart";
        private const string ADD_CART_ENDPOINT = "/api/Carts/AddCart";
        private const string DELETE_CART_ENDPOINT = "/api/Carts/DeleteCart";


        /// <summary>
        /// Gets the cart for a specific user
        /// </summary>
        /// <param name="userId">User ID to get cart for</param>
        /// <returns>User's cart information</returns>
        public async Task<CartDto> GetUserCartAsync(int userId)
        {
            string queryString = $"UserId={userId}";

            try
            {
                return await GetAsync<CartDto>(GET_USER_CART_ENDPOINT, queryString);
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == 404)
                {
                    throw new ApiException(404, $"Cart for user {userId} not found");
                }
                throw;
            }
        }

        /// <summary>
        /// Updates an existing cart
        /// </summary>
        /// <param name="cart">Updated cart data</param>
        /// <returns>Updated cart information</returns>
        public async Task<CartDto> UpdateCartAsync(CartDto cart)
        {
            try
            {
                return await PutAsync<CartDto>(UPDATE_CART_ENDPOINT, cart);
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == 401)
                {
                    throw new ApiException(401, "Unauthorized to update this cart");
                }
                throw;
            }
        }

        /// <summary>
        /// Creates a new cart
        /// </summary>
        /// <param name="cart">Cart data to create</param>
        /// <returns>Created cart information</returns>
        public async Task<CartDto> AddCartAsync(CartDto cart)
        {
            try
            {
                return await PostAsync<CartDto>(ADD_CART_ENDPOINT, cart);
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == 401)
                {
                    throw new ApiException(401, "Unauthorized to create a cart");
                }
                throw;
            }
        }

        /// <summary>
        /// Deletes a cart by ID
        /// </summary>
        /// <param name="cartId">Cart ID to delete</param>
        public async Task DeleteCartAsync(int cartId)
        {
            string queryString = $"cartId={cartId}";

            try
            {
                await DeleteAsync(DELETE_CART_ENDPOINT, queryString);
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == 404)
                {
                    throw new ApiException(404, $"Cart with ID {cartId} not found");
                }
                throw;
            }
        }
    }
}