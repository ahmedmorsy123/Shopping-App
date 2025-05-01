using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ShoppingAppDB.Data;
using ShoppingAppDB.Entities;
using static ShoppingAppDB.ProductData;
using static ShoppingAppDB.UserData;

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
            public List<ProductDto> OrderItems { get; set; }

        }

        private ILogger<OrderData> _logger;
        private CartData _cartData;
        private UserData _userData;
        private const string _prefix = "OrderDA ";

        public OrderData(ILogger<OrderData> logger, CartData cartData, UserData userData)
        {
            _logger = logger;
            _cartData = cartData;
            _userData = userData;
        }

        public OrderDto AddOrder(string shippingAddress, string paymentMethod)
        {
            _logger.LogInformation($"{_prefix}Add Order");
            using (var context = new AppDbContext())
            {
                var orderToAdd = new Order();
                orderToAdd.UserId = UserData._currentUser.Id;
                orderToAdd.OrderDate = DateTime.Now;
                orderToAdd.TotalPrice = 0;
                orderToAdd.Status = "Pending";
                orderToAdd.ShippingAddress = shippingAddress;
                orderToAdd.PaymentMethod = paymentMethod;
                context.Orders.Add(orderToAdd);
                context.SaveChanges();
                _logger.LogInformation($"{_prefix}Order Added with id {orderToAdd.Id}");

                var cartItems = context.CartItems
                    .Where(ci => ci.Cart.UserId == UserData._currentUser.Id)
                    .Include(ci => ci.Product)
                    .ToList();
                foreach (var item in cartItems)
                {
                    var orderItemToAdd = new OrderItem();
                    orderItemToAdd.OrderId = orderToAdd.Id;
                    orderItemToAdd.ProductId = item.ProductId;
                    orderItemToAdd.Quantity = item.Quantity;
                    orderItemToAdd.UnitPrice = item.Product.Price;
                    context.OrderItems.Add(orderItemToAdd);

                    context.Products.Where(p => p.Id == item.ProductId).FirstOrDefault().Quantity -= item.Quantity;

                    context.SaveChanges();
                    _logger.LogInformation($"{_prefix}OrderItem Added with id {orderItemToAdd.Id}");
                    orderToAdd.TotalPrice += item.Product.Price * item.Quantity;
                }

                _cartData.DeleteCart(_cartData.GetCartIdByUserId(UserData._currentUser.Id));

                return new OrderDto()
                {
                    Id = orderToAdd.Id,
                    CreatedAt = orderToAdd.OrderDate,
                    TotalPrice = orderToAdd.TotalPrice,
                    Status = orderToAdd.Status,
                    ShippingAddress = orderToAdd.ShippingAddress,
                    PaymentMethod = orderToAdd.PaymentMethod,
                    OrderItems = orderToAdd.OrderItems
                    .Select(oi => new ProductDto()
                    {
                        Id = oi.Product.Id,
                        productName = oi.Product.Name,
                        productCategory = context.ProductCategories.FirstOrDefault(pc => pc.Id == oi.Product.CategoryId).CategoryName,
                        productDescription = oi.Product.Description,
                        quantity = oi.Quantity,
                        price = oi.Product.Price
                    }).ToList()
                };
            }
        }

        public OrderDto? GetOrderById(int id)
        {
            _logger.LogInformation($"{_prefix}Get Order By Id");
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

        public List<OrderDto>? GetUserOrders(int UserId)
        {
            _logger.LogInformation($"{_prefix}Get User Orders");
            var user = _userData.GetUserById(UserId);
            if (user == null)
            {
                _logger.LogWarning($"{_prefix}User Not Found");
                return null;
            }
            List<OrderDto> OrderItems;

            using (var context = new AppDbContext())
            {
                OrderItems = context.Orders.AsNoTracking()
                    .Where(o => o.UserId == user.Id)
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
                            Id = oi.Product.Id,
                            productName = oi.Product.Name,
                            quantity = oi.Quantity,
                            price = oi.Product.Price
                        }).ToList()
                    }).ToList();
            }

            return OrderItems.Count == 0 ? null : OrderItems;
        }

        public void CancelOrder(int orderId)
        {
            _logger.LogInformation($"{_prefix}Cancel Order");
            using (var context = new AppDbContext())
            {
                var order = context.Orders.Find(orderId);
                if (order != null)
                {
                    order.Status = "Canceled";
                    context.SaveChanges();
                    _logger.LogInformation($"{_prefix}Order Canceled");
                }
                else
                {
                    _logger.LogWarning($"{_prefix}Order Not Found");
                }
            }
        }

    }
}
