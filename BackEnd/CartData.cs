using Microsoft.EntityFrameworkCore;
using ShoppingAppDB.Data;
using static ShoppingAppDB.ProductData;

namespace ShoppingAppDB
{
    public class CartData
    {

        public static List<ProductDto>? GetUserCart(int UserId)
        {
            var user = UsersData.GetUserById(UserId);
            if (user == null) return null;
            List<ProductDto> cartItems;

            using (var context = new AppDbContext())
            {
                var cart = context.Carts.AsNoTracking()
                    .Where(c => c.UserId == user.Id).FirstOrDefault();
                if (cart != null)
                {
                    cartItems = context.CartItems
                        .Where(ci => ci.CartId == cart.Id)
                        .Select(ci => new ProductDto()
                        {
                            productName = ci.Product.Name,
                            quantity = ci.Quantity,
                            price = ci.Product.Price
                        })
                        .ToList();
                }
                else return null;
            }

            return cartItems;
        }
    }
}
