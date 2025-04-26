using Microsoft.EntityFrameworkCore;
using ShoppingAppDB.Data;
using ShoppingAppDB.Entities;
using static ShoppingAppDB.ProductData;
using static ShoppingAppDB.UsersData;

namespace ShoppingAppDB
{
    public class OrderData
    {
        public class OrderDto
        {
            public int Id { get; set; }
            public DateTime CreatedAt { get; set; }
            public decimal TotalPrice { get; set; }
            public string Status { get; set; }
            public string? ShippingAddress { get; set; }
            public string? PaymentMethod { get; set; }
            public List<ProductDto>? OrderItems { get; set; }

        }

        public static int AddOrder(OrderDto order)
        {
            using (var context = new AppDbContext())
            {
                var orderToAdd = new Order();
                orderToAdd.UserId = _currentUser.Id;
                orderToAdd.OrderDate = order.CreatedAt;
                orderToAdd.TotalPrice = order.TotalPrice;
                orderToAdd.Status = order.Status;
                orderToAdd.ShippingAddress = order.ShippingAddress;
                orderToAdd.PaymentMethod = order.PaymentMethod;
                context.Orders.Add(orderToAdd);
                context.SaveChanges();

                foreach(var item in order.OrderItems)
                {
                    var orderItemToAdd = new OrderItem();
                    orderItemToAdd.OrderId = orderToAdd.Id;
                    orderItemToAdd.ProductId = item.Id;
                    orderItemToAdd.Quantity = item.quantity;
                    orderItemToAdd.UnitPrice = item.price;
                    context.OrderItems.Add(orderItemToAdd);
                    context.SaveChanges();
                }
                
                return orderToAdd.Id;
            }
        }

        public static OrderDto? GetOrderById(int id)
        {
            using (var context = new AppDbContext())
            {
                return context.Orders.AsNoTracking()
                    .Where(o => o.Id == id)
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                    .Select(o => new OrderDto()
                    {
                        Id = o.Id,
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
                    }).FirstOrDefault();
            }
        }

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
                            Id = oi.Product.Id,
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
