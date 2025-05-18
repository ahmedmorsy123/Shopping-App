﻿using Microsoft.Extensions.Logging;
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
        public async Task<int> GetPageCountAsync()
        {
            return await _productData.GetPageCountAsync();
        }
        public async Task<List<ProductDto>> GetProductsPaginatedAsync(int pageNumber)
        {
            _logger.LogInformation($"{_prefix}GetProductsPaginated");
            return await _productData.GetProductsPaginatedAsync(pageNumber);
        }
    }
}