using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
            public int quantity { get; set; }
            public decimal Weight { get; set; }
            public decimal price { get; set; }
        }

        private ILogger<ProductData> _logger;
        private const string _prefix = "ProductDA ";

        public ProductData(ILogger<ProductData> logger)
        {
            _logger = logger;
        }

        public List<ProductDto> GetProductsPaginated(int pageNumber, int pageSize = 10)
        {
            _logger.LogInformation($"{_prefix}Get Products Paginated");
            var products = new List<ProductDto>();

            using (var context = new AppDbContext())
            {
                products = context.Products.AsNoTracking()
                    .Include(p => p.Category)
                    .Select(p => new ProductDto()
                    {
                        Id = p.Id,
                        productName = p.Name,
                        productCategory = p.Category.CategoryName,
                        productDescription = p.Description,
                        Weight = p.Weight,
                        price = p.Price
                    })
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            }
            _logger.LogInformation($"{_prefix}Returned {products.Count} products, page {pageNumber}, pageSize {pageSize}");
            return products;
        }

    }
}
