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

        public static OrderDto AddOrder(string shippingAddress, string paymentMethod)
        {
            return OrderData.AddOrder(shippingAddress, paymentMethod);
        }

        public static OrderDto? GetOrderById(int id)
        {
            return OrderData.GetOrderById(id);
        }

        public static void CancelOrder(int id)
        {
            OrderData.CancelOrder(id);
        }

    }
}
