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

        [HttpGet("GetProductsPaginated")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsPaginated(int pageNumber)
        {
            double pageCount = await _productsService.GetPageCountAsync();

            if (pageNumber > pageCount || pageNumber < 0)
                return BadRequest($"Invalid page number. Must be between 0 and {pageCount}.");

            _logger.LogInformation($"{_prefix}GetAllProductsPaginated");
            return Ok(await _productsService.GetProductsPaginatedAsync(pageNumber));
        }

        [HttpGet("GetPagesCount")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<int>> GetPageCountAsync()
        {
            return Ok(await _productsService.GetPageCountAsync());
        }
    }
}