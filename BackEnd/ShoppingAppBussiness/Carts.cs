using ShoppingAppDB;
using static ShoppingAppDB.CartData;

namespace ShoppingAppBussiness
{
    public class Carts
    {
        public static CartDto? GetCurrentUserCart()
        {
            return CartData.GetCurrentUserCart();
        }

        public static bool UpdateCart(CartDto cart)
        {
            return CartData.UpdateCart(cart);
        }

        public static int AddCart(CartDto cart)
        {
            return CartData.AddCart(cart);
        }

        public static bool DeleteCart(int cartId)
        {
            return CartData.DeleteCart(cartId);
        }

        public static int GetCartIdByUserId(int userId)
        {
            return CartData.GetCartIdByUserId(userId);
        }

        public static bool CurrentUserHaveCart()
        {
            return CartData.CurrentUserHaveCart();
        }
    }
}
