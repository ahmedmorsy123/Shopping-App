using Microsoft.Extensions.Logging;
using ShoppingAppDB;
using ShoppingAppDB.Models;

namespace ShoppingAppBussiness
{
    public class CartsService
    {
        private ILogger<CartsService> _logger;
        private readonly CartData _cartData;
        private const string _prefix = "CartsBL ";

        public CartsService(ILogger<CartsService> logger, CartData cartData)
        {
            _logger = logger;
            _cartData = cartData;
        }

        public async Task<CartDto?> GetUserCartAsync(int userId)
        {
            _logger.LogInformation($"{_prefix}Get User Cart");
            return await _cartData.GetUserCartAsync(userId);
        }

        public async Task<bool> UpdateCartAsync(CartDto cart)
        {
            _logger.LogInformation($"{_prefix}Update Cart");
            return await _cartData.UpdateCartAsync(cart);
        }

        public async Task<int> AddCartAsync(CartDto cart)
        {
            _logger.LogInformation($"{_prefix}Add Cart");
            return await _cartData.AddCartAsync(cart);
        }

        public async Task<bool> DeleteCartAsync(int cartId)
        {
            _logger.LogInformation($"{_prefix}Delete Cart");
            return await _cartData.DeleteCartAsync(cartId);
        }

        public int GetCartIdByUserId(int userId)
        {
            _logger.LogInformation($"{_prefix}Get Cart Id By User Id");
            return _cartData.GetCartIdByUserId(userId);
        }

        public async Task<int> GetCartsCountAsync()
        {
            _logger.LogInformation($"{_prefix}GetCartsCountAsync called");
            return await _cartData.GetCartsCountAsync();
        }
    }
}