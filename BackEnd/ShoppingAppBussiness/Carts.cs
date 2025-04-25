using ShoppingAppDB;
using static ShoppingAppDB.ProductData;

namespace ShoppingAppBussiness
{
    public class Carts
    {
        public static List<ProductDto>? GetUserCart(int UserId)
        {
            return CartData.GetUserCart(UserId);
        }
    }
}
