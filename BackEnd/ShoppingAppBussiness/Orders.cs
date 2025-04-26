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

        public static int AddOrder(OrderDto order)
        {
            return OrderData.AddOrder(order);
        }

        public static OrderDto? GetOrderById(int id)
        {
            return OrderData.GetOrderById(id);
        }
    }
}
