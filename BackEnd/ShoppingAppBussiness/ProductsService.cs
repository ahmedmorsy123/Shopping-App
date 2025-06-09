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
        public async Task<List<ProductDto>> GetOutOfStockProductsAsync()
        {
            _logger.LogInformation($"{_prefix}GetOutOfStockProductsAsync called");
            return await _productData.GetOutOfStockProductsAsync();
        }

        public async Task<List<ProductDto>> GetLowStockProductsAsync(int threshold)
        {
            _logger.LogInformation($"{_prefix}GetLowStockProductsAsync called with threshold: {threshold}");
            return await _productData.GetLowStockProductsAsync(threshold);
        }

        public async Task<bool> UpdateProductStockAsync(UpdateProductStockRequest stockRequest)
        {
            _logger.LogInformation($"{_prefix}UpdateProductStockAsync called for productId: {stockRequest.ProductId} , quantity:  {stockRequest.Quentity}");
            return await _productData.UpdateProductStockAsync(stockRequest);
        }
    }
}