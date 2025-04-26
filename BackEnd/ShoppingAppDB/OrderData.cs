using Microsoft.EntityFrameworkCore;
using ShoppingAppDB.Data;
using ShoppingAppDB.Entities;
using static ShoppingAppDB.ProductData;

namespace ShoppingAppDB
{
    public class OrderData
    {
        public class OrderDto
        {
            public DateTime CreatedAt { get; set; }
            public decimal TotalPrice { get; set; }
            public string Status { get; set; }
            public string? ShippingAddress { get; set; }
            public string? PaymentMethod { get; set; }
            public List<ProductDto>? OrderItems { get; set; }

        }

        //public static void AddOrder(OrderDto order)
        //{
        //    using (var context = new AppDbContext())
        //    {
        //        context.Orders.Add(new Order()
        //        {
        //            UserId = UsersData.GetCurrentUser().Id,
        //            OrderDate = order.CreatedAt,
        //            TotalPrice = order.TotalPrice,
        //            Status = order.Status,
        //            ShippingAddress = order.ShippingAddress,
        //            PaymentMethod = order.PaymentMethod,
        //            OrderItems = order.OrderItems.Select(oi => new OrderItem()
        //            {
        //                ProductId = oi.productName,
        //                Quantity = oi.quantity,
        //                UnitPrice = oi.price
        //            }).ToList()
        //        });
        //        context.SaveChanges();
        //    }
        //}
        
        public static List<OrderDto>? GetUserOrders(int UserId)
        {
            var user = UsersData.GetUserById(UserId);
            if (user == null) return null;
            List<OrderDto> OrderItems;

            using (var context = new AppDbContext())
            {
                OrderItems = context.Orders.AsNoTracking()
                    .Where(o => o.UserId == user.Id)
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                    .Select(o => new OrderDto()
                    {
                        CreatedAt = o.OrderDate,
                        TotalPrice = o.TotalPrice,
                        Status = o.Status,
                        ShippingAddress = o.ShippingAddress,
                        PaymentMethod = o.PaymentMethod,
                        OrderItems = o.OrderItems.Select(oi => new ProductDto()
                        {
                            productName = oi.Product.Name,
                            quantity = oi.Quantity,
                            price = oi.Product.Price
                        }).ToList()
                    }).ToList();
            }

            return OrderItems.Count == 0 ? null : OrderItems;
        }
    }
}
