using Microsoft.EntityFrameworkCore;
using ShoppingAppDB.Data;
using ShoppingAppDB.Entities;
using static ShoppingAppDB.ProductData;

namespace ShoppingAppDB
{
    public class CartData
    {
        public class CartDto
        {
            public int CartId { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime UpdatedAt { get; set; }
            public List<ProductDto> Products { get; set; }
        }

        public static CartDto? GetCurrentUserCart()
        {
            if (UsersData._currentUser == null) return null;
            CartDto? cart;

            using (var context = new AppDbContext())
            {
                cart = context.Carts.AsNoTracking()
                    .Where(c => c.UserId == UsersData._currentUser.Id)
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
                            price = ci.Product.Price
                        }).ToList()
                    })
                    .FirstOrDefault();
            }

            return cart;
        }

        public static bool UpdateCart(CartDto newCart)
        {
            using (var context = new AppDbContext())
            {
                var cart = context.Carts.Find(newCart.CartId);
                if (cart != null)
                {
                    cart.UpdatedAt = DateTime.Now;

                    var cartItems = context.CartItems.Where(ci => ci.CartId == newCart.CartId).ToList();
                    context.CartItems.RemoveRange(cartItems);

                    if (newCart.Products.Count == 0)
                    {
                        DeleteCart(newCart.CartId);
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
                        context.CartItems.Add(cartItem);
                    }
                    context.SaveChanges();
                    return true;
                }
                else return false;
            }
        }

        public static bool DeleteCart(int CartId)
        {
            using (var context = new AppDbContext())
            {
                var cart = context.Carts.Find(CartId);
                if (cart != null)
                {
                    context.Carts.Remove(cart);
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public static int AddCart(CartDto cart)
        {
            using(var context = new AppDbContext())
            {
                var cartToAdd = new Cart();
                cartToAdd.CreatedAt = DateTime.Now;
                cartToAdd.UserId = UsersData._currentUser.Id;
                context.Carts.Add(cartToAdd);
                context.SaveChanges();

                foreach(var item in cart.Products)
                {
                    var cartItem = new CartItem();
                    cartItem.CartId = cartToAdd.Id;
                    cartItem.ProductId = item.Id;
                    cartItem.Quantity = item.quantity;
                    cartToAdd.CartItems.Add(cartItem);
                    context.SaveChanges();
                }

                return cartToAdd.Id;
            }
        }

        public static int GetCartIdByUserId(int UserId)
        {
            using (var context = new AppDbContext())
            {
                return context.Carts.AsNoTracking().Where(c => c.UserId == UserId).Select(c => c.Id).FirstOrDefault();
            }
        }

        public static bool CurrentUserHaveCart()
        {
            using (var context = new AppDbContext())
            {
                return context.Carts.Any(c => c.UserId == UsersData._currentUser.Id);
            }
        }
    }
}
