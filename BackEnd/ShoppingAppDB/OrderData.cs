using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShoppingAppDB.Data;
using ShoppingAppDB.Entities;
using ShoppingAppDB.Models;
using static ShoppingAppDB.Enums.Enums;

namespace ShoppingAppDB
{
    public class OrderData
    {
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

        public async Task<OrderDto?> AddOrderAsync(int userId, string shippingAddress, string paymentMethod)
        {
            _logger.LogInformation($"{_prefix}Add Order");
            using (var context = new AppDbContext())
            {
                var orderToAdd = new Order();
                orderToAdd.UserId = userId;
                orderToAdd.OrderDate = DateTime.Now;
                orderToAdd.TotalPrice = 0;
                orderToAdd.Status = "Pending";
                orderToAdd.ShippingAddress = shippingAddress;
                orderToAdd.PaymentMethod = paymentMethod;
                await context.Orders.AddAsync(orderToAdd);

                _logger.LogInformation($"{_prefix}Order Added with id {orderToAdd.Id}");

                var cartItems = await context.CartItems.AsNoTracking()
                    .Where(ci => ci.Cart.UserId == userId)
                    .Include(ci => ci.Product)
                    .ToListAsync();

                if (cartItems.Count == 0) return null;
                await context.SaveChangesAsync();

                foreach (var item in cartItems)
                {
                    var orderItemToAdd = new OrderItem();
                    orderItemToAdd.OrderId = orderToAdd.Id;
                    orderItemToAdd.ProductId = item.ProductId;
                    orderItemToAdd.Quantity = item.Quantity;
                    orderItemToAdd.UnitPrice = item.Product.Price;
                    await context.OrderItems.AddAsync(orderItemToAdd);

                    var product = await context.Products.Where(p => p.Id == item.ProductId).FirstAsync();
                    product.Quantity -= item.Quantity;
                    if (product.Quantity == 0) product.IsActive = false;

                    _logger.LogInformation($"{_prefix}OrderItem Added with id {orderItemToAdd.Id}");
                    orderToAdd.TotalPrice += item.Product.Price * item.Quantity;
                    await context.SaveChangesAsync();
                }

                await _cartData.DeleteCartAsync(_cartData.GetCartIdByUserId(userId));

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
                        productCategory = context.ProductCategories.FirstOrDefault(pc => pc.Id == oi.Product.CategoryId)!.CategoryName,
                        productDescription = oi.Product.Description,
                        quantity = oi.Quantity,
                        price = oi.Product.Price
                    }).ToList()
                };
            }
        }

        public async Task<OrderDto?> GetOrderByIdAsync(int OrderId)
        {
            _logger.LogInformation($"{_prefix}Get Order By Id");
            using (var context = new AppDbContext())
            {
                return await context.Orders.AsNoTracking()
                    .Where(o => o.Id == OrderId)
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
                            maxQuantity = oi.Product.Quantity,
                            price = oi.Product.Price
                        }).ToList()
                    }).FirstOrDefaultAsync();
            }
        }

        public async Task<List<OrderDto>?> GetUserOrdersAsync(int UserId)
        {
            _logger.LogInformation($"{_prefix}Get User Orders");
            var user = await _userData.GetUserByIdAsync(UserId);
            if (user == null)
            {
                _logger.LogWarning($"{_prefix}User Not Found");
                return null;
            }
            List<OrderDto> OrderItems;

            using (var context = new AppDbContext())
            {
                OrderItems = await context.Orders.AsNoTracking()
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
                            maxQuantity = oi.Product.Quantity,
                            price = oi.Product.Price
                        }).ToList()
                    }).ToListAsync();
            }
            return OrderItems.Count == 0 ? null : OrderItems;
        }

        public async Task CancelOrderAsync(int orderId)
        {
            _logger.LogInformation($"{_prefix}Cancel Order");
            using (var context = new AppDbContext())
            {
                var order = await context.Orders.FindAsync(orderId);
                if (order != null)
                {
                    order.Status = "Cancelled";
                    await context.SaveChangesAsync();
                    _logger.LogInformation($"{_prefix}Order Canceled");
                }
                else
                {
                    _logger.LogWarning($"{_prefix}Order Not Found");
                }
            }
        }

        public async Task<bool> ProcessOrderAsync(int orderId)
        {
            _logger.LogInformation($"{_prefix}Processing order with id {orderId}");
            using (var context = new AppDbContext())
            {
                var order = await context.Orders.FindAsync(orderId);
                if (order != null)
                {
                    order.Status = "Processing";
                    await context.SaveChangesAsync();
                    _logger.LogInformation($"{_prefix}Order {orderId} has been processed");
                    return true;
                }
                else
                {
                    _logger.LogWarning($"{_prefix}Order with id {orderId} not found");
                    return false;
                }
            }
        }
        public async Task<bool> ShipOrderAsync(int orderId)
        {
            _logger.LogInformation($"{_prefix}Shipping order with id {orderId}");
            using (var context = new AppDbContext())
            {
                var order = await context.Orders.FindAsync(orderId);
                if (order != null)
                {
                    order.Status = "Shipped";
                    await context.SaveChangesAsync();
                    _logger.LogInformation($"{_prefix}Order {orderId} has been shipped");
                    return true;
                }
                else
                {
                    _logger.LogWarning($"{_prefix}Order with id {orderId} not found");
                    return false;
                }
            }
        }

        public async Task<bool> DeliverOrderAsync(int orderId)
        {
            _logger.LogInformation($"{_prefix}Delivering order with id {orderId}");
            using (var context = new AppDbContext())
            {
                var order = await context.Orders.FindAsync(orderId);
                if (order != null)
                {
                    order.Status = "Delivered";
                    await context.SaveChangesAsync();
                    _logger.LogInformation($"{_prefix}Order {orderId} has been delivered");
                    return true;
                }
                else
                {
                    _logger.LogWarning($"{_prefix}Order with id {orderId} not found");
                    return false;
                }
            }
        }

        public async Task<List<OrderDto>> GetOrdersByDurationAndStatusAsync(TimeDuration duration, OrderStatus status)
        {
            _logger.LogInformation($"{_prefix}Fetching orders for duration: {duration} and status: {status}");
            using (var context = new AppDbContext())
            {
                DateTime startDate = CalculateStartDate(duration);
                var ordersQuery = context.Orders.AsNoTracking()
                    .Where(o => o.OrderDate >= startDate);
                if (status != OrderStatus.All)
                {
                    ordersQuery = ordersQuery.Where(o => o.Status == status.ToString());
                }
                var orders = await ordersQuery
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
                            maxQuantity = oi.Product.Quantity,
                            price = oi.Product.Price
                        }).ToList()
                    })
                    .ToListAsync();
                if (orders.Count == 0)
                {
                    _logger.LogWarning($"{_prefix}No orders found for the specified duration and status");
                    return new List<OrderDto>();
                }

                _logger.LogInformation($"{_prefix}Found {orders.Count} orders for the specified duration and status");
                return orders;
            }
        }

        public async Task<OrderStatus> GetOrderStatusAsync(int orderId)
        {
            _logger.LogInformation($"GetOrderStatus: {orderId}");
            using (var context = new AppDbContext())
            {
                var order = await context.Orders.FindAsync(orderId);

                Enum.TryParse(order!.Status, false, out OrderStatus orderStatusEnum);
                return orderStatusEnum;
            }
        }

    }
}