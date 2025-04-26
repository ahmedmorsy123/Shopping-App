using Microsoft.EntityFrameworkCore;
using ShoppingAppDB.Data;

namespace ShoppingAppDB
{
    public class ProductData
    {
        public class ProductDto
        {
            public int Id { get; set; }
            public string productName { get; set; }
            public string? productCategory { get; set; }
            public string? productDescription { get; set; }
            public int? quantity { get; set; }
            public decimal Weight { get; set; }
            public decimal price { get; set; }
        }


        public static List<ProductDto>? GetAllProducts()
        {
            var products = new List<ProductDto>();

            using (var context = new AppDbContext())
            {
                products = context.Products.AsNoTracking()
                    .Include(p => p.Category)
                    .Select(p => new ProductDto()
                {
                    Id = p.Id,
                    productCategory = p.Category.CategoryName,
                    productDescription = p.Description,
                    productName = p.Name,
                    Weight = p.Weight,
                    price = p.Price
                }).ToList();
            }

            return products;
        }
    }
}
