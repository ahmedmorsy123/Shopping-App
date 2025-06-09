using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingAppBussiness;
using ShoppingAppDB.Models;

namespace ShoppingAppAPI.Controllers
{
    [Route("api/Products")]
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

        [Authorize]
        [HttpGet("GetProducts")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedList<ProductDto>))]
        public async Task<ActionResult<PagedList<ProductDto>>> GetProducts(string? SearchTerm, string? SortColumn, string? SortOrder, int Page, int PageSize)
        {
            _logger.LogInformation($"{_prefix}GetProducts");

            return Ok(await _productsService.GetProducts(SearchTerm, SortColumn, SortOrder, Page, PageSize));
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet("LowStockProducts")]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ProductDto>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<List<ProductDto>>> GetLowStockProducts(int threshold = 5)
        {
            _logger.LogInformation($"{_prefix}Get Low Stock Products with threshold: {threshold}");
            var lowStockProducts = await _productsService.GetLowStockProductsAsync(threshold);
            if (lowStockProducts == null || !lowStockProducts.Any())
            {
                _logger.LogWarning($"{_prefix}No low stock products found with threshold: {threshold}");
                return NotFound(new ProblemDetails()
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "No Low Stock Products Found",
                    Detail = $"No products found with stock below {threshold}.",
                    Instance = HttpContext.Request.Path
                });
            }
            return Ok(lowStockProducts);
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet("OutOfStockProducts")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ProductDto>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<List<ProductDto>>> GetOutOfStockProducts()
        {
            _logger.LogInformation($"{_prefix}Get Out Of Stock Products");
            return Ok(await _productsService.GetOutOfStockProductsAsync());
        }

        //[Authorize(Roles = "Admin")]
        [HttpPut("UpdateProductStock")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> UpdateProductStock(UpdateProductStockRequest stockRequest)
        {
            _logger.LogInformation($"{_prefix}UpdateProductStock called for productId: {stockRequest.ProductId}");
            if (stockRequest.ProductId <= 0 || stockRequest.Quentity < 0)
            {
                _logger.LogWarning($"{_prefix}Invalid productId or newStock: {stockRequest.ProductId}, {stockRequest.Quentity}");
                return BadRequest(new ProblemDetails()
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Invalid Product ID or Stock",
                    Detail = "Product ID must be greater than zero and stock cannot be negative.",
                    Instance = HttpContext.Request.Path
                });
            }
            var result = await _productsService.UpdateProductStockAsync(stockRequest);
            if (!result)
            {
                _logger.LogWarning($"{_prefix}Failed to update stock for product with id: {stockRequest.ProductId}");
                return NotFound(new ProblemDetails()
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Product Not Found",
                    Detail = "No product found with the provided ID.",
                    Instance = HttpContext.Request.Path
                });
            }
            return NoContent();
        }
    }
}