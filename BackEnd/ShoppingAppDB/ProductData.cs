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