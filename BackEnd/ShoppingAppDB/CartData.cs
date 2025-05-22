using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShoppingAppDB.Data;
using ShoppingAppDB.Entities;
using ShoppingAppDB.Models;

namespace ShoppingAppDB
{
    public class CartData
    {
        private ILogger<CartData> _logger;
        private const string _prefix = "CartDA ";

        public CartData(ILogger<CartData> logger)
        {
            _logger = logger;
        }

        public async Task<CartDto?> GetUserCartAsync(int userId)
        {
            _logger.LogInformation($"{_prefix}Get User Cart");
            using (var context = new AppDbContext())
            {
                return await context.Carts.AsNoTracking()
                    .Where(c => c.UserId == userId)
                    .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                    .Select(c => new CartDto()
                    {
                        CartId = c.Id,
                        CreatedAt = c.CreatedAt,
                        UpdatedAt = c.UpdatedAt,
                        Products = c.CartItems.Select(ci => new ProductDto()
                        {
                            Id = ci.Product.Id,
                            productName = ci.Product.Name,
                            quantity = ci.Quantity,
                            maxQuantity = ci.Product.Quantity,
                            price = ci.Product.Price
                        }).ToList()
                    })
                    .FirstOrDefaultAsync();
            }
        }

        public async Task<bool> UpdateCartAsync(CartDto newCart)
        {
            _logger.LogInformation($"{_prefix}Update Cart");
            using (var context = new AppDbContext())
            {
                var cart = await context.Carts.FindAsync(newCart.CartId);
                if (cart != null)
                {
                    cart.UpdatedAt = DateTime.Now;

                    var cartItems = await context.CartItems.Where(ci => ci.CartId == newCart.CartId).ToListAsync();
                    context.CartItems.RemoveRange(cartItems);

                    if (newCart.Products.Count == 0)
                    {
                        await DeleteCartAsync(newCart.CartId);
                        _logger.LogInformation($"{_prefix}New Cart Have no Products so deleted");
                        return true;
                    }

                    foreach (var item in newCart.Products)
                    {
                        var cartItem = new CartItem()
                        {
                            CartId = newCart.CartId,
                            ProductId = item.Id,
                            Quantity = item.quantity
                        };
                        await context.CartItems.AddAsync(cartItem);
                    }
                    await context.SaveChangesAsync();
                    _logger.LogInformation($"{_prefix}Cart Updated");
                    return true;
                }
                else
                {
                    _logger.LogWarning($"{_prefix}Cart Not Found");
                    return false;
                }
            }
        }

        public async Task<bool> DeleteCartAsync(int CartId)
        {
            _logger.LogInformation($"{_prefix}Delete Cart");
            using (var context = new AppDbContext())
            {
                var cart = await context.Carts.FindAsync(CartId);
                if (cart != null)
                {
                    context.Carts.Remove(cart);
                    await context.SaveChangesAsync();
                    _logger.LogInformation($"{_prefix}Cart Deleted");
                    return true;
                }
                _logger.LogWarning($"{_prefix}Cart Not Found");
                return false;
            }
        }

        public async Task<int> AddCartAsync(CartDto cart)
        {
            _logger.LogInformation($"{_prefix}Add Cart");
            using (var context = new AppDbContext())
            {
                var cartToAdd = new Cart();
                cartToAdd.CreatedAt = DateTime.Now;
                cartToAdd.UserId = cart.UserId;
                context.Carts.Add(cartToAdd);

                await context.SaveChangesAsync();
                _logger.LogInformation($"{_prefix}Cart Added");

                foreach (var item in cart.Products)
                {
                    var cartItem = new CartItem();
                    cartItem.CartId = cartToAdd.Id;
                    cartItem.ProductId = item.Id;
                    cartItem.Quantity = item.quantity;
                    cartToAdd.CartItems.Add(cartItem);
                }

                await context.SaveChangesAsync();
                _logger.LogInformation($"{_prefix}Cart Items Added");

                return cartToAdd.Id;
            }
        }

        public int GetCartIdByUserId(int UserId)
        {
            _logger.LogInformation($"{_prefix}Get Cart Id By User Id");
            using (var context = new AppDbContext())
            {
                return context.Carts.AsNoTracking().Where(c => c.UserId == UserId).Select(c => c.Id).FirstOrDefault();
            }
        }
    }
}