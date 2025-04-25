using ShoppingAppDB;
using static ShoppingAppDB.OrderData;

namespace ShoppingAppBussiness
{
    public class Orders
    {
        public static List<OrderDto>? GetUserOrders(int UserId)
        {
            return OrderData.GetUserOrders(UserId);
        }
    }
}
