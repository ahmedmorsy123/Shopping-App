using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingAppBussiness;
using ShoppingAppDB.Models;

namespace ShoppingAppAPI.Controllers
{
    [Route("api/Products")]
    [Authorize]
    [ApiController]
    public class ProductsAPI : ControllerBase
    {
        private readonly ILogger<ProductsAPI> _logger;
        private readonly ProductsService _productsService;
        private const string _prefix = "ProductsAPI ";

        public ProductsAPI(ILogger<ProductsAPI> logger, ProductsService products)
        {
            _logger = logger;
            _productsService = products;
        }

        [HttpGet("GetProducts")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedList<ProductDto>))]
        public async Task<ActionResult<PagedList<ProductDto>>> GetProducts(string? SearchTerm, string? SortColumn, string? SortOrder, int Page, int PageSize)
        {
            _logger.LogInformation($"{_prefix}GetProducts");

            return Ok(await _productsService.GetProducts(SearchTerm, SortColumn, SortOrder, Page, PageSize));
        }
    }
}