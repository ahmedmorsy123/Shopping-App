using Microsoft.Extensions.Logging;
using ShoppingAppDB;
using ShoppingAppDB.Models;

namespace ShoppingAppBussiness
{
    public class ProductsService
    {
        private ILogger<ProductsService> _logger;
        private readonly ProductData _productData;
        private const string _prefix = "ProductsBL ";

        public ProductsService(ILogger<ProductsService> logger, ProductData productData)
        {
            _logger = logger;
            _productData = productData;
        }
        public async Task<PagedList<ProductDto>> GetProducts(string? SearchTerm, string? SortColumn, string? SortOrder, int Page, int PageSize)
        {
            _logger.LogInformation($"{_prefix}GetProductsPaginated");
            return await _productData.GetProducts(SearchTerm, SortColumn, SortOrder, Page, PageSize);
        }
    }
}