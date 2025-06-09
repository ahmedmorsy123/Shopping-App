using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Serilog;
using Shopping_App;
using Shopping_App.Hellpers;
using ShoppingApp.Api.Models;
using ShoppingAppDB.Models;

namespace ShoppingApp.Api.Controllers
{
    public class ProductsService : ApiClient
    {
        public ProductsService(HttpClient httpClient) : base(httpClient)
        {
        }
        public async Task<PagedList<ProductDto>> GetProductsAsync(string SearchTerm, string SortColumn, string SortOrder, int Page, int PageSize)
        {
            Log.Information("Getting products for page number: {PageNumber}", Page);
            if (Page < 1)
            {
                throw new ApiException(400, "Page number must be greater than or equal to 1");
            }

            string queryString = $"Page={Page}&PageSize={PageSize}";

            if (!string.IsNullOrEmpty(SearchTerm))
            {
                queryString += $"&SearchTerm={SearchTerm}";
            }

            if (!string.IsNullOrEmpty(SortColumn))
            {
                queryString += $"&SortColumn={SortColumn}";
            }

            try
            {
                return await GetAsync<PagedList<ProductDto>>(Config.GetApiEndpoint("Products", "GetProducts"), queryString);
            }
            catch (ApiException ex)
            {
                if(ex.StatusCode == 401)
                {
                    Log.Error("Unauthorized access to products for page number: {PageNumber}", Page);
                    throw new ApiException(401, "Unauthorized access to products");
                }
                else
                {
                    throw;
                }

            }
        }

        public async Task<List<ProductDto>> GetLowStockProducts(int threshold = 5)
        {
            Log.Information("Getting low stock products with threshold: {Threshold}", threshold);
            string queryString = $"threshold={threshold}";

            try
            {
                var products = await GetAsync<List<ProductDto>>(Config.GetApiEndpoint("Products", "LowStockProducts"), queryString);
                return products;
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == 401)
                {
                    Log.Error("Unauthorized access to low stock products");
                    throw new ApiException(401, "Unauthorized access to low stock products");
                }
                else if (ex.StatusCode == 403)
                {
                    Log.Error("Forbidden access to low stock products");
                    throw new ApiException(403, "Forbidden access to low stock products");
                }
                else if (ex.StatusCode == 404)
                {
                    Log.Error("Low stock products not found");
                    throw new ApiException(404, "Low stock products not found");
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<List<ProductDto>> GetOutOfStockProducts()
        {
            Log.Information("Getting out of stock products");
            try
            {
                var products = await GetAsync<List<ProductDto>>(Config.GetApiEndpoint("Products", "OutOfStockProducts"));
                return products;
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == 401)
                {
                    Log.Error("Unauthorized access to out of stock products");
                    throw new ApiException(401, "Unauthorized access to out of stock products");
                }
                else if (ex.StatusCode == 403)
                {
                    Log.Error("Forbidden access to out of stock products");
                    throw new ApiException(403, "Forbidden access to out of stock products");
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task UpdateProductStock(UpdateProductStockRequest stockRequest)
        {
            Log.Information("Updating stock for product ID: {ProductId}", stockRequest.ProductId);
            try
            {
                await PutAsync<string>(Config.GetApiEndpoint("Products", "UpdateProductStock"), stockRequest);
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == 400)
                {
                    Log.Error("Invalid stock update request for product ID: {ProductId}", stockRequest.ProductId);
                    throw new ApiException(400, "Invalid stock update request");
                }
                else if (ex.StatusCode == 401)
                {
                    Log.Error("Unauthorized to update stock for product ID: {ProductId}", stockRequest.ProductId);
                    throw new ApiException(401, "Unauthorized to update product stock");
                } else if(ex.StatusCode == 404)
                {
                    Log.Error("Product not found with ID: {ProductId}", stockRequest.ProductId);
                    throw new ApiException(404, $"Product with ID {stockRequest.ProductId} not found");
                } else if (ex.StatusCode == 403)
                {
                    Log.Error("Forbidden access to update stock for product ID: {ProductId}", stockRequest.ProductId);
                    throw new ApiException(403, "Forbidden access to update product stock");
                }
                else
                {
                    throw;
                }
            }
        }
    }
}