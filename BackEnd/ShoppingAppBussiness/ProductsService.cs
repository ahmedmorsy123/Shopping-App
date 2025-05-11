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

        public List<ProductDto> GetProductsPaginated(int pageNumber)
        {
            _logger.LogInformation($"{_prefix}GetProductsPaginated");
            return _productData.GetProductsPaginated(pageNumber);
        }
    }
}