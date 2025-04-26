using Bogus;
using Microsoft.EntityFrameworkCore;
using ShoppingAppDB.Data;
using ShoppingAppDB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingAppDB.Data.Seeder
{
    public class DatabaseSeeder
    {
        private readonly AppDbContext _context;
        private readonly Random _random = new Random();

        public DatabaseSeeder(AppDbContext context)
        {
            _context = context;
        }


        public void Seed(int userCount = 10, int categoryCount = 5, int productCount = 50)
        {

            // Seed in proper order to maintain relationships
            SeedUsers(userCount);
            SeedCarts();
            SeedProductCategories(categoryCount);
            SeedProducts(productCount, categoryCount);
            SeedCartItemsRandom();
            SeedOrdersRandom();
            SeedOrderItemsRandom();

            // Save all changes
            _context.SaveChanges();
        }

        private void SeedUsers(int count)
        {
            if (!_context.Users.Any())
            {
                var userFaker = new Faker<User>()
                    .RuleFor(u => u.Name, f => f.Name.FullName())
                    .RuleFor(u => u.Email, (f, u) => f.Random.Bool(0.9f) ? f.Internet.Email(u.Name) : "default@example.com") // Default email for null cases
                    .RuleFor(u => u.PasswordHash, f => f.Internet.Password(12, false, "", "!@#$%^&*"))
                    .RuleFor(u => u.CreatedAt, f => f.Date.Between(DateTime.Now.AddYears(-2), DateTime.Now.AddMonths(-1)))
                    .RuleFor(u => u.LastLogin, (f, u) => f.Random.Bool(0.8f) ? f.Date.Between(u.CreatedAt, DateTime.Now) : null); // 20% null LastLogin

                var users = userFaker.Generate(count);
                _context.Users.AddRange(users);
                _context.SaveChanges();
            }
        }

        private void SeedCarts()
        {
            if (!_context.Carts.Any())
            {
                var users = _context.Users.ToList();
                var cartFaker = new Faker<Cart>()
                    .RuleFor(c => c.CreatedAt, f => f.Date.Between(DateTime.Now.AddMonths(-6), DateTime.Now.AddDays(-10)))
                    .RuleFor(c => c.UpdatedAt, (f, c) => f.Date.Between(c.CreatedAt, DateTime.Now));

                foreach (var user in users)
                {
                    var faker = new Faker();

                    if (faker.Random.Bool(0.9f))
                    {
                        var cart = cartFaker.Generate();
                        cart.UserId = user.Id;
                        _context.Carts.Add(cart);
                    }
                }

                _context.SaveChanges();
            }
        }

        private void SeedProductCategories(int count)
        {
            if (!_context.ProductCategories.Any())
            {
                var commonCategories = new List<string>
                    {
                        "Electronics", "Clothing", "Books", "Home & Kitchen",
                        "Sports", "Beauty", "Toys", "Jewelry", "Automotive", "Garden",
                        "Office Supplies", "Pets", "Food & Groceries", "Health", "Music"
                    };

                var categoryFaker = new Faker<ProductCategory>()
                    .RuleFor(c => c.CategoryName, f =>
                    {
                        if (commonCategories.Count > 0)
                        {
                            var name = commonCategories[0];
                            commonCategories.RemoveAt(0);
                            return name;
                        }

                        // Generate up to 10 categories and pick the first valid one
                        var generatedCategories = f.Commerce.Categories(10).Where(x => !string.IsNullOrEmpty(x)).ToList();
                        return generatedCategories.FirstOrDefault() ?? f.Commerce.ProductAdjective(); // Fallback if all are invalid
                    })
                    .RuleFor(c => c.Description, f => f.Random.Bool(0.9f) ? f.Commerce.ProductDescription() : null);

                var categories = categoryFaker.Generate(count).Where(x => !string.IsNullOrEmpty(x.CategoryName)).ToList();
                _context.ProductCategories.AddRange(categories);
                _context.SaveChanges();
            }
        }

        private void SeedProducts(int totalProductCount, int categoryCount)
        {
            if (!_context.Products.Any())
            {
                var categories = _context.ProductCategories.ToList();
                var allProducts = new List<Product>();

                int baseProductsPerCategory = totalProductCount / categoryCount;
                int remainingProducts = totalProductCount % categoryCount;

                foreach (var category in categories)
                {
                    int productsForThisCategory = baseProductsPerCategory;
                    if (remainingProducts > 0)
                    {
                        productsForThisCategory++;
                        remainingProducts--;
                    }

                    var productFaker = new Faker<Product>()
                        .RuleFor(p => p.CategoryId, f => category.Id)
                        .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                        .RuleFor(p => p.Description, f => f.Random.Bool(0.9f) ? f.Commerce.ProductDescription() : null)
                        .RuleFor(p => p.Weight, f => Math.Round(f.Random.Decimal(0.1M, 10M), 2))
                        .RuleFor(p => p.Price, f => Math.Round(f.Random.Decimal(5M, 1000M), 2))
                        .RuleFor(p => p.Quantity, f => f.Random.Int(0, 500))
                        .RuleFor(p => p.CreatedAt, f => f.Date.Between(DateTime.Now.AddYears(-1), DateTime.Now))
                        .RuleFor(p => p.IsActive, f => f.Random.Bool(0.9f));

                    var products = productFaker.Generate(productsForThisCategory);
                    allProducts.AddRange(products);
                }

                _context.Products.AddRange(allProducts);
                _context.ChangeTracker.Entries();
                _context.SaveChanges();
            }
        }

        private void SeedCartItemsRandom()
        {
            if (!_context.CartItems.Any())
            {
                var carts = _context.Carts.ToList();
                var activeProducts = _context.Products.Where(p => p.IsActive).ToList();
                var allCartItems = new List<CartItem>();

                var faker = new Faker();

                foreach (var cart in carts)
                {
                    int itemCount = faker.Random.Int(0, 5);

                    if (itemCount > 0)
                    {
                        var cartProducts = activeProducts
                            .OrderBy(x => Guid.NewGuid())
                            .Take(Math.Min(itemCount, activeProducts.Count))
                            .ToList();

                        foreach (var product in cartProducts)
                        {
                            var cartItem = new CartItem
                            {
                                CartId = cart.Id,
                                ProductId = product.Id,
                                Quantity = faker.Random.Int(1, 5)
                            };
                            allCartItems.Add(cartItem);
                        }
                    }
                }

                _context.CartItems.AddRange(allCartItems);
            }
            _context.SaveChanges();
        }

        private void SeedOrdersRandom()
        {
            if (!_context.Orders.Any())
            {
                var users = _context.Users.ToList();
                var allOrders = new List<Order>();

                var orderStatuses = new[] { "Pending", "Processing", "Shipped", "Delivered", "Cancelled" };
                var paymentMethods = new[] { "Credit Card", "PayPal", "Bank Transfer", null };

                var faker = new Faker();

                foreach (var user in users)
                {
                    if (faker.Random.Bool(0.8f))
                    {
                        int orderCount = faker.Random.Int(1, 5);

                        for (int i = 0; i < orderCount; i++)
                        {
                            var orderDate = faker.Date.Between(DateTime.Now.AddYears(-1), DateTime.Now);

                            var order = new Order
                            {
                                UserId = user.Id,
                                OrderDate = orderDate,
                                TotalPrice = Math.Round(faker.Random.Decimal(10M, 2000M), 2),
                                Status = faker.PickRandom(orderStatuses),
                                ShippingAddress = faker.Random.Bool(0.9f) ? faker.Address.FullAddress() : null,
                                PaymentMethod = faker.PickRandom(paymentMethods)
                            };

                            allOrders.Add(order);
                        }
                    }
                }

                _context.Orders.AddRange(allOrders);
                _context.SaveChanges();
            }
        }

        private void SeedOrderItemsRandom()
        {
            if (!_context.OrderItems.Any())
            {
                var orders = _context.Orders.ToList();
                var activeProducts = _context.Products.Where(p => p.IsActive).ToList();
                var allOrderItems = new List<OrderItem>();
                var faker = new Faker();

                foreach (var order in orders)
                {
                    int itemCount = faker.Random.Int(1, 8);

                    var orderProducts = activeProducts
                        .OrderBy(x => Guid.NewGuid())
                        .Take(Math.Min(itemCount, activeProducts.Count))
                        .ToList();

                    decimal orderTotal = 0;

                    foreach (var product in orderProducts)
                    {
                        var quantity = faker.Random.Int(1, 5);
                        var unitPrice = product.Price;

                        var orderItem = new OrderItem
                        {
                            OrderId = order.Id,
                            ProductId = product.Id,
                            Quantity = quantity,
                            UnitPrice = unitPrice
                        };

                        orderTotal += unitPrice * quantity;
                        allOrderItems.Add(orderItem);
                    }

                    order.TotalPrice = Math.Round(orderTotal, 2);
                }

                _context.OrderItems.AddRange(allOrderItems);
                _context.SaveChanges();
            }
        }
   
    }
}