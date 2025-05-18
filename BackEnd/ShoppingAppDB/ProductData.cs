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
        private int _pageSize = 10;

        public ProductData(ILogger<ProductData> logger)
        {
            _logger = logger;
        }

        public async Task<int> GetPageCountAsync()
        {
            _logger.LogInformation($"{_prefix} page count");

            using(var context = new AppDbContext())
            {
                return (int)Math.Ceiling(await context.Products.CountAsync() / _pageSize * 1.0);
            }
        }

        public async Task<List<ProductDto>> GetProductsPaginatedAsync(int pageNumber)
        {
            _logger.LogInformation($"{_prefix}Get Products Paginated");
            var products = new List<ProductDto>();

            using (var context = new AppDbContext())
            {
                products = await context.Products.AsNoTracking()
                    .Include(p => p.Category)
                    .Skip((pageNumber - 1) * _pageSize)
                    .Take(_pageSize)
                    .Select(p => new ProductDto()
                    {
                        Id = p.Id,
                        productName = p.Name,
                        productCategory = p.Category.CategoryName,
                        productDescription = p.Description,
                        Weight = p.Weight,
                        price = p.Price
                    })
                    .ToListAsync();
            }
            _logger.LogInformation($"{_prefix}Returned {products.Count} products, page {pageNumber}, pageSize {_pageSize}");
            return products;
        }
    }
}