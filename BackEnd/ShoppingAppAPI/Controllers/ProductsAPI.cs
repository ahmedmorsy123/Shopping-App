using Microsoft.AspNetCore.Mvc;
using Serilog.Context;
using ShoppingAppBussiness;
using static ShoppingAppDB.ProductData;

namespace ShoppingAppAPI.Controllers
{
    [Route("api/Products")]
    [ApiController]
    public class ProductsAPI : ControllerBase
    {
        private readonly ILogger<ProductsAPI> _logger;
        private readonly Products _productsService;
        private const string _prefix = "ProductsAPI ";

        public ProductsAPI(ILogger<ProductsAPI> logger, Products products) 
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
