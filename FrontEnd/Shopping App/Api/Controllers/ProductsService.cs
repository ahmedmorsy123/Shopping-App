using System.Collections.Generic;
using System.Threading.Tasks;
using Serilog;
using ShoppingApp.Api.Models;

namespace ShoppingApp.Api.Controllers
{
    public class ProductsService : ApiClient
    {
        private const string GET_PRODUCTS_ENDPOINT = "/api/Products/GetProductsPaginated";
        private const string GET_PAGES_COUNT = "/api/Products/GetPagesCount";

        /// <summary>
        /// Gets a paginated list of products
        /// </summary>
        /// <param name="pageNumber">Page number to retrieve</param>
        /// <returns>List of products for the specified page</returns>
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
                return await GetAsync<PagedList<ProductDto>>(GET_PRODUCTS_ENDPOINT, queryString);
            }
            catch (ApiException ex)
            {
                throw;
            }
        }

         
    }
}