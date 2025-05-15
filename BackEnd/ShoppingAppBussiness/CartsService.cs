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

        public async Task<CartDto?> GetUserCart(int userId)
        {
            _logger.LogInformation($"{_prefix}Get User Cart");
            return await _cartData.GetUserCart(userId);
        }

        public async Task<bool> UpdateCart(CartDto cart)
        {
            _logger.LogInformation($"{_prefix}Update Cart");
            return await _cartData.UpdateCart(cart);
        }

        public async Task<int> AddCart(CartDto cart)
        {
            _logger.LogInformation($"{_prefix}Add Cart");
            return await _cartData.AddCart(cart);
        }

        public async Task<bool> DeleteCart(int cartId)
        {
            _logger.LogInformation($"{_prefix}Delete Cart");
            return await _cartData.DeleteCart(cartId);
        }

        public int GetCartIdByUserId(int userId)
        {
            _logger.LogInformation($"{_prefix}Get Cart Id By User Id");
            return _cartData.GetCartIdByUserId(userId);
        }
    }
}