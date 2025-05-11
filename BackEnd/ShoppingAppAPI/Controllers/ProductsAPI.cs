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

        [HttpGet("GetAllProductsPaginated")]
        public ActionResult<IEnumerable<ProductDto>> GetProductsPaginated(int page)
        {
            _logger.LogInformation($"{_prefix}GetAllProductsPaginated");
            return Ok(_productsService.GetProductsPaginated(page));
        }
    }
}