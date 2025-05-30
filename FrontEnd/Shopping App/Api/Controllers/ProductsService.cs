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
                throw;
            }
        }
    }
}