﻿namespace ShoppingAppDB.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public ProductCategory Category { get; set; } = new ProductCategory();
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Weight { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }

        public List<CartItem>? CartItems { get; set; }
        public List<OrderItem>? OrderItems { get; set; }
    }


}
