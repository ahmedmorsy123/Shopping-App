using Bogus;
using Microsoft.EntityFrameworkCore;
using ShoppingAppDB.Data;
using ShoppingAppDB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingAppDB.Helpers
{
    public class DatabaseSeeder
    {
        private static readonly AppDbContext _context = new AppDbContext();
        private static readonly Random _random = new Random();



        public static async Task SeedAsync(int userCount = 100, int categoriesCount = 10, int productsCount = 200)
        {
            // Clear existing data (if needed)
            // await ClearDataAsync();

            // Seed data in order of dependencies
            await SeedCategoriesAsync(categoriesCount);
            await SeedProductsAsync(productsCount);
            await SeedUsersAsync(userCount);
            await SeedCartsAndItemsAsync();
            await SeedOrdersAsync();

            Console.WriteLine("Database seeding completed successfully!");
        }

        private static async Task ClearDataAsync()
        {
            // Be careful with this method as it will delete ALL data
            _context.OrderItems.RemoveRange(_context.OrderItems);
            _context.Orders.RemoveRange(_context.Orders);
            _context.CartItems.RemoveRange(_context.CartItems);
            _context.Carts.RemoveRange(_context.Carts);
            _context.Products.RemoveRange(_context.Products);
            _context.ProductCategories.RemoveRange(_context.ProductCategories);
            _context.Users.RemoveRange(_context.Users);

            await _context.SaveChangesAsync();
            Console.WriteLine("Database cleared successfully!");
        }

        private static async Task SeedCategoriesAsync(int count)
        {
            if (await _context.ProductCategories.AnyAsync())
                return;

            var categories = new Faker<ProductCategory>()
                .RuleFor(c => c.CategoryName, f => f.Commerce.Categories(1)[0] + " " + f.Commerce.ProductAdjective())
                .RuleFor(c => c.Description, f => f.Commerce.ProductDescription())
                .Generate(count);

            await _context.ProductCategories.AddRangeAsync(categories);
            await _context.SaveChangesAsync();
            Console.WriteLine($"{count} categories seeded successfully!");
        }

        private static async Task SeedProductsAsync(int count)
        {
            if (await _context.Products.AnyAsync())
                return;

            var categories = await _context.ProductCategories.ToListAsync();

            var products = new Faker<Product>()
                .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
                .RuleFor(p => p.Price, f => decimal.Parse(f.Commerce.Price(5, 1000)))
                .RuleFor(p => p.Weight, f => f.Random.Decimal(0.1m, 50m))
                .RuleFor(p => p.Quantity, f => f.Random.Int(10, 1000))
                .RuleFor(p => p.CreatedAt, f => f.Date.Past(2))
                .RuleFor(p => p.IsActive, f => f.Random.Bool(0.9f))
                .RuleFor(p => p.CategoryId, f => f.PickRandom(categories).Id)
                .Generate(count);

            await _context.Products.AddRangeAsync(products);
            await _context.SaveChangesAsync();
            Console.WriteLine($"{count} products seeded successfully!");
        }

        private static async Task SeedUsersAsync(int count)
        {
            if (await _context.Users.AnyAsync())
                return;

            var users = new Faker<User>()
                .RuleFor(u => u.Name, f => f.Name.FullName())
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.PasswordHash, f => BCrypt.Net.BCrypt.HashPassword("Password123!"))
                .RuleFor(u => u.CreatedAt, f => f.Date.Past(3))
                .RuleFor(u => u.LastLogin, f => f.Date.Recent(30))
                .Generate(count);

            await _context.Users.AddRangeAsync(users);
            await _context.SaveChangesAsync();
            Console.WriteLine($"{count} users seeded successfully!");
        }

        private static async Task SeedCartsAndItemsAsync()
        {
            if (await _context.Carts.AnyAsync())
                return;

            var users = await _context.Users.ToListAsync();
            var products = await _context.Products.ToListAsync();
            var faker = new Faker();

            // Create carts for about 30% of users
            var cartsToCreate = users.Take(users.Count / 3).ToList();

            foreach (var user in cartsToCreate)
            {
                var cart = new Cart
                {
                    UserId = user.Id,
                    CreatedAt = DateTime.Now.AddDays(-faker.Random.Int(1, 30)),
                    UpdatedAt = DateTime.Now,
                    CartItems = new List<CartItem>()
                };

                // Add 1-5 random products to each cart
                var itemCount = faker.Random.Int(1, 5);
                var selectedProducts = faker.Random.Shuffle(products).Take(itemCount).ToList();

                foreach (var product in selectedProducts)
                {
                    cart.CartItems.Add(new CartItem
                    {
                        ProductId = product.Id,
                        Quantity = faker.Random.Int(1, 10)
                    });
                }

                _context.Carts.Add(cart);
            }

            await _context.SaveChangesAsync();
            Console.WriteLine($"{cartsToCreate.Count} carts with items seeded successfully!");
        }

        private static async Task SeedOrdersAsync()
        {
            if (await _context.Orders.AnyAsync())
                return;

            var users = await _context.Users.ToListAsync();
            var products = await _context.Products.ToListAsync();
            var faker = new Faker();

            // Create orders for about 70% of users (some with multiple orders)
            var ordersToCreate = new List<Order>();

            foreach (var user in users.Take((int)(users.Count * 0.7)))
            {
                // Each user may have 1-3 orders
                var orderCount = faker.Random.Int(1, 3);

                for (int i = 0; i < orderCount; i++)
                {
                    var orderDate = faker.Date.Between(DateTime.Now.AddYears(-1), DateTime.Now);
                    var order = new Order
                    {
                        UserId = user.Id,
                        OrderDate = orderDate,
                        Status = GetRandomOrderStatus(orderDate),
                        ShippingAddress = faker.Address.FullAddress(),
                        PaymentMethod = faker.PickRandom("Credit Card", "PayPal", "Bank Transfer", "Cash on Delivery"),
                        OrderItems = new List<OrderItem>()
                    };

                    // Add 1-10 random products to each order
                    var itemCount = faker.Random.Int(1, 10);
                    var selectedProducts = faker.Random.Shuffle(products).Take(itemCount).ToList();
                    decimal totalPrice = 0;

                    foreach (var product in selectedProducts)
                    {
                        var quantity = faker.Random.Int(1, 5);
                        var unitPrice = product.Price * (1 - faker.Random.Decimal(0, 0.15m)); // Small discount

                        order.OrderItems.Add(new OrderItem
                        {
                            ProductId = product.Id,
                            Quantity = quantity,
                            UnitPrice = unitPrice
                        });

                        totalPrice += unitPrice * quantity;
                    }

                    order.TotalPrice = totalPrice;
                    ordersToCreate.Add(order);
                }
            }

            await _context.Orders.AddRangeAsync(ordersToCreate);
            await _context.SaveChangesAsync();
            Console.WriteLine($"{ordersToCreate.Count} orders with items seeded successfully!");
        }

        private static string GetRandomOrderStatus(DateTime orderDate)
        {
            // Logic to determine status based on date
            var daysAgo = (DateTime.Now - orderDate).TotalDays;

            if (daysAgo > 30)
                return "Delivered";
            if (daysAgo > 14)
                return _random.Next(100) < 90 ? "Delivered" : "Shipped";
            if (daysAgo > 7)
                return _random.Next(100) < 70 ? "Shipped" : "Processing";
            if (daysAgo > 3)
                return _random.Next(100) < 60 ? "Processing" : "Pending";

            return _random.Next(100) < 80 ? "Pending" : "Processing";
        }
    }
}
