using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShoppingAppDB.Data;
using ShoppingAppDB.Entities;
using ShoppingAppDB.Models;
using ShoppingAppDB.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingAppDB
{
    public class AdminData
    {
        private ILogger<AdminData> _logger;
        private Password _passwordService;
        private const string _prefix = "AdminDA ";
        public enum TimeDeuration
        {
            Last24Hours,
            Last7Days,
            Last30Days,
            Last90Days,
            ThisMonth,
            ThisYear,
            ThisDay,
            AllTime
        }

        public enum OrderStatus
        {
            Pending,
            Processing,
            Shipped,
            Delivered,
            Cancelled,
            All
        }

        public AdminData(ILogger<AdminData> logger, Password passwordService)
        {
            _logger = logger;
            _passwordService = passwordService;
        }

        public async Task<UserDto?> AddAdminAsync(UserDto admin)
        {
            _logger.LogInformation($"{_prefix}Adding admin");
            if (admin == null)
            {
                _logger.LogWarning($"{_prefix}admin is null");
                return null;
            }

            using (var context = new AppDbContext())
            {
                if (context.Users.Any(u => u.Email == admin.Email))
                {
                    _logger.LogWarning($"{_prefix}admin with email {admin.Email} already exists");
                    return null;
                }
                if (context.Users.Any(u => u.Name == admin.Name))
                {
                    _logger.LogWarning($"{_prefix}admin with UserName {admin.Email} already exists");
                    return null;
                }

                User adminToAdd = new User();

                adminToAdd.Name = admin.Name;
                adminToAdd.Email = admin.Email;
                adminToAdd.PasswordHash = _passwordService.HashPassword(admin.Password!);
                adminToAdd.CreatedAt = DateTime.Now;
                adminToAdd.LastLogin = DateTime.Now;
                adminToAdd.Role = "Admin";
                await context.Users.AddAsync(adminToAdd);

                await context.SaveChangesAsync();
                _logger.LogInformation($"{_prefix}Admin added successfully with id {adminToAdd.Id}");

                return new UserDto(adminToAdd.Id, adminToAdd.Name, adminToAdd.Email, adminToAdd.PasswordHash);
            }
        }
        public async Task<bool> RemoveAdminAsync(int adminId)
        {
            _logger.LogInformation($"{_prefix}Deleted admin with id {adminId}");
            using (var context = new AppDbContext())
            {
                var userToDelete = await context.Users.FindAsync(adminId);
                if (userToDelete != null)
                {
                    context.Users.Remove(userToDelete);
                    await context.SaveChangesAsync();
                    _logger.LogInformation($"{_prefix}admim with id {adminId} deleted");
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> MakeAdminAsync(int userId)
        {
            _logger.LogInformation($"{_prefix}Making user with id {userId} an admin");
            using(var context = new AppDbContext())
            {
                var userToUpdate = await context.Users.FindAsync(userId);
                if (userToUpdate != null)
                {
                    userToUpdate.Role = "Admin";
                    await context.SaveChangesAsync();
                    _logger.LogInformation($"{_prefix}User with id {userId} is now an admin");
                    return true;
                }
                else
                {
                    _logger.LogWarning($"{_prefix}User with id {userId} not found");
                    return false;
                }
            }
        }
        public async Task<List<UserDto>> ListAdminsAsync()
        {
            _logger.LogInformation($"{_prefix}Listing all admins");
            using (var context = new AppDbContext())
            {
                var admins = await context.Users.AsNoTracking()
                    .Where(u => u.Role == "Admin")
                    .Select(u => new UserDto(u.Id, u.Name, u.Email, null))
                    .ToListAsync();
                
                if (admins.Count == 0)
                {
                    _logger.LogWarning($"{_prefix}No admins found");
                    return new List<UserDto>();
                }
                _logger.LogInformation($"{_prefix}Found {admins.Count} admins");
                return admins;
            }
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            _logger.LogInformation($"{_prefix}Getting all users");
            using (var context = new AppDbContext())
            {
                var users = await context.Users.AsNoTracking()
                    .Where(u => u.Role != "Admin") 
                    .Select(u => new UserDto(u.Id, u.Name, u.Email, null))
                    .ToListAsync();

                if (users.Count == 0)
                {
                    _logger.LogWarning($"{_prefix}No users found");
                    return new List<UserDto>();
                }
                _logger.LogInformation($"{_prefix}Found {users.Count} users");
                return users;
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
                    order.Status ="Processing";
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

        public async  Task<List<ProductDto>> GetOutOfStockProductsAsync()
        {
            _logger.LogInformation($"{_prefix}Fetching out of stock products");
            using (var context = new AppDbContext())
            {
                var outOfStockProducts = await context.Products.AsNoTracking()
                    .IgnoreQueryFilters() // Ignore any global filters
                    .Where(p => p.Quantity <= 0)
                    .Include(p => p.Category)
                    .Select(p => new ProductDto()
                    {
                        Id = p.Id,
                        productName = p.Name,
                        productCategory = p.Category.CategoryName,
                        productDescription = p.Description,
                        maxQuantity = p.Quantity,
                        Weight = p.Weight,
                        price = p.Price
                    })
                    .ToListAsync();
                if (outOfStockProducts.Count == 0)
                {
                    _logger.LogWarning($"{_prefix}No out of stock products found");
                    return new List<ProductDto>();
                }
                _logger.LogInformation($"{_prefix}Found {outOfStockProducts.Count} out of stock products");
                return outOfStockProducts;
            }
        }

        public async Task<List<ProductDto>> GetLowStockProductsAsync(int threshold)
        {
            _logger.LogInformation($"{_prefix}Fetching low stock products with threshold {threshold}");
            using (var context = new AppDbContext())
            {
                var lowStockProducts = await context.Products.AsNoTracking()
                    .IgnoreQueryFilters() // Ignore any global filters
                    .Where(p => p.Quantity > 0 && p.Quantity < threshold)
                    .Include(p => p.Category)
                    .Select(p => new ProductDto()
                    {
                        Id = p.Id,
                        productName = p.Name,
                        productCategory = p.Category.CategoryName,
                        productDescription = p.Description,
                        maxQuantity = p.Quantity,
                        Weight = p.Weight,
                        price = p.Price
                    })
                    .ToListAsync();
                if (lowStockProducts.Count == 0)
                {
                    _logger.LogWarning($"{_prefix}No low stock products found");
                    return new List<ProductDto>();
                }
                _logger.LogInformation($"{_prefix}Found {lowStockProducts.Count} low stock products");
                return lowStockProducts;
            }
        }

        public async Task<bool> StockProductAsync(int productId, int quantity)
        {
            _logger.LogInformation($"{_prefix}Stocking product with id {productId} with quantity {quantity}");
            using (var context = new AppDbContext())
            {
                var product = await context.Products.FindAsync(productId);
                if (product != null)
                {
                    product.Quantity += quantity;
                    if (product.Quantity > 0) product.IsActive = true; // Activate product if quantity is greater than 0
                    await context.SaveChangesAsync();
                    _logger.LogInformation($"{_prefix}Product {productId} stocked with quantity {quantity}");
                    return true;
                }
                else
                {
                    _logger.LogWarning($"{_prefix}Product with id {productId} not found");
                    return false;
                }
            }
        }

        public async Task<List<OrderDto>> GetOrdersByDurationAndStatusAsync(TimeDeuration duration, OrderStatus status)
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

        public async Task<int> GetLoginCountByDurationAsync(TimeDeuration duration)
        {
            _logger.LogInformation($"{_prefix}Fetching login count for duration: {duration}");
            using (var context = new AppDbContext())
            {
                DateTime startDate = CalculateStartDate(duration);
                int loginCount = await context.Users.AsNoTracking()
                    .Where(u => u.LastLogin >= startDate)
                    .CountAsync();
                _logger.LogInformation($"{_prefix}Login count for duration {duration}: {loginCount}");
                return loginCount;
            }
        }

        public async Task<int> GetRegisterationCountByDurationAsync(TimeDeuration duration)
        {
            _logger.LogInformation($"{_prefix}Fetching registration count for duration: {duration}");
            using (var context = new AppDbContext())
            {
                DateTime startDate = CalculateStartDate(duration);
                int registrationCount = await context.Users.AsNoTracking()
                    .Where(u => u.CreatedAt >= startDate)
                    .CountAsync();
                _logger.LogInformation($"{_prefix}Registration count for duration {duration}: {registrationCount}");
                return registrationCount;
            }
        }

        public async Task<int> GetCartsCountAsync()
        {
            _logger.LogInformation($"{_prefix}Fetching total carts count");
            using (var context = new AppDbContext())
            {
                int cartsCount = await context.Carts.AsNoTracking().CountAsync();
                _logger.LogInformation($"{_prefix}Total carts count: {cartsCount}");
                return cartsCount;
            }
        }


        private DateTime CalculateStartDate(TimeDeuration duration)
        {
            switch (duration)
            {
                case TimeDeuration.Last24Hours:
                    return DateTime.Now.AddHours(-24);
                case TimeDeuration.Last7Days:
                    return DateTime.Now.AddDays(-7);
                case TimeDeuration.Last30Days:
                    return DateTime.Now.AddDays(-30);
                case TimeDeuration.Last90Days:
                    return DateTime.Now.AddDays(-90);
                case TimeDeuration.ThisMonth:
                    return new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                case TimeDeuration.ThisYear:
                    return new DateTime(DateTime.Now.Year, 1, 1);
                case TimeDeuration.ThisDay:
                    return DateTime.Today;
                case TimeDeuration.AllTime:
                    return DateTime.MinValue; // No limit
                default:
                    return DateTime.MinValue;
            }
        }
    }
}
