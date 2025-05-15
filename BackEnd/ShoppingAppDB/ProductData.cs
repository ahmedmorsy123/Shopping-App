using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShoppingAppDB.Data;
using ShoppingAppDB.Models;

namespace ShoppingAppDB
{
    public class ProductData
    {
        private ILogger<ProductData> _logger;
        private const string _prefix = "ProductDA ";

        public ProductData(ILogger<ProductData> logger)
        {
            _logger = logger;
        }

        public async Task<List<ProductDto>> GetProductsPaginated(int pageNumber, int pageSize = 10)
        {
            _logger.LogInformation($"{_prefix}Get Products Paginated");
            var products = new List<ProductDto>();

            using (var context = new AppDbContext())
            {
                products = await context.Products.AsNoTracking()
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
                    .ToListAsync();
            }
            _logger.LogInformation($"{_prefix}Returned {products.Count} products, page {pageNumber}, pageSize {pageSize}");
            return products;
        }
    }
}