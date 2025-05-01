using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using ShoppingAppDB;
using static ShoppingAppDB.CartData;

namespace ShoppingAppBussiness
{
    public class Carts
    {
        private ILogger<Carts> _logger;
        private readonly CartData _cartData;
        private const string _prefix = "CartsBL ";
        public Carts(ILogger<Carts> logger, CartData cartData)
        {
            _logger = logger;
            _cartData = cartData;
        }
        public CartDto? GetCurrentUserCart()
        {
            _logger.LogInformation($"{_prefix}Get Current User Cart");
            return _cartData.GetCurrentUserCart();
        }

        public bool UpdateCart(CartDto cart)
        {
            _logger.LogInformation($"{_prefix}Update Cart");
            return _cartData.UpdateCart(cart);
        }

        public int AddCart(CartDto cart)
        {
            _logger.LogInformation($"{_prefix}Add Cart");
            return _cartData.AddCart(cart);
        }

        public bool DeleteCart(int cartId)
        {
            _logger.LogInformation($"{_prefix}Delete Cart");
            return _cartData.DeleteCart(cartId);
        }

        public int GetCartIdByUserId(int userId)
        {
            _logger.LogInformation($"{_prefix}Get Cart Id By User Id");
            return _cartData.GetCartIdByUserId(userId);
        }

        public bool CurrentUserHaveCart()
        {
            _logger.LogInformation($"{_prefix}Current User Have Cart");
            return _cartData.CurrentUserHaveCart();
        }
    }
}
