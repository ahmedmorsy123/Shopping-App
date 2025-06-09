using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShoppingAppDB.Data;
using ShoppingAppDB.Entities;
using ShoppingAppDB.Models;
using System.Linq.Expressions;

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

        public async Task<PagedList<ProductDto>> GetProducts(string? SearchTerm, string? SortColumn, string? SortOrder, int Page, int PageSize)
        {
            _logger.LogInformation($"{_prefix}Get Products");

            using (var context = new AppDbContext())
            {
                IQueryable<Product> productsQuery = context.Products;

                if (!string.IsNullOrWhiteSpace(SearchTerm))
                {
                    productsQuery = productsQuery.Where(p => p.Name.Contains(SearchTerm) || (p.Description != null && p.Description.Contains(SearchTerm)));
                }

                if (SortOrder?.ToLower() == "desc")
                {
                    productsQuery = productsQuery.OrderByDescending(GetSortProperty(SortColumn));
                }
                else
                {
                    productsQuery = productsQuery.OrderBy(GetSortProperty(SortColumn));
                }

                var products = new List<ProductDto>();
                products = await context.Products.AsNoTracking()
                    .Include(p => p.Category)
                    .Skip((Page - 1) * PageSize)
                    .Take(PageSize)
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

                _logger.LogInformation($"{_prefix}Returned: page {Page}, pageSize {PageSize}");
                return new PagedList<ProductDto>(products, Page, PageSize, await context.Products.CountAsync());
            }
        }

        public async Task<List<ProductDto>> GetOutOfStockProductsAsync()
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

        public async Task<bool> UpdateProductStockAsync(UpdateProductStockRequest stockRequest)
        {
            _logger.LogInformation($"{_prefix}Stocking product with id {stockRequest.ProductId}  with quantity  {stockRequest.Quentity}");
            using (var context = new AppDbContext())
            {
                var product = await context.Products.FindAsync(stockRequest.ProductId);
                if (product != null)
                {
                    product.Quantity += stockRequest.Quentity;
                    if (product.Quantity > 0) product.IsActive = true; // Activate product if quantity is greater than 0
                    await context.SaveChangesAsync();
                    _logger.LogInformation($"{_prefix}Product {stockRequest.ProductId} stocked with quantity {stockRequest.Quentity}");
                    return true;
                }
                else
                {
                    _logger.LogWarning($"{_prefix}Product with id {stockRequest.ProductId} not found");
                    return false;
                }
            }
        }


        private static Expression<Func<Product, object>> GetSortProperty(string? SortColumn)
        {
             return SortColumn?.ToLower() switch
            {
                "name" => product => product.Name,
                "amount" => product => product.Quantity,
                "price" => product => product.Price,
                _ => product => product.Id
            };

        }
    }
}