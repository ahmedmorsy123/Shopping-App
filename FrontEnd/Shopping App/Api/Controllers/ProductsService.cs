using System.Collections.Generic;
using System.Threading.Tasks;
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
        public async Task<List<ProductDto>> GetProductsPaginatedAsync(int pageNumber)
        {
            if (pageNumber < 1)
            {
                throw new ApiException(400, "Page number must be greater than or equal to 1");
            }

            string queryString = $"pageNumber={pageNumber}";

            try
            {
                return await GetAsync<List<ProductDto>>(GET_PRODUCTS_ENDPOINT, queryString);
            }
            catch (ApiException ex)
            {
                if (ex.StatusCode == 400)
                {
                    throw new ApiException(400, "Invalid page number");
                }
                throw;
            }
        }

        public async Task<int> GetPagesCount()
        {
            return await GetAsync<int>(GET_PAGES_COUNT);
        }
    }
}