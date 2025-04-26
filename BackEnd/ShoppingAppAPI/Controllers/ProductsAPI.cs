using Microsoft.AspNetCore.Mvc;
using ShoppingAppBussiness;
using static ShoppingAppDB.ProductData;

namespace ShoppingAppAPI.Controllers
{
    [Route("api/Products")]
    [ApiController]
    public class ProductsAPI : ControllerBase
    {
        [HttpGet("GetAllProducts")]
        public ActionResult<IEnumerable<ProductDto>> GetAllProducts()
        {
            return Ok(Products.GetAllProducts());
        }

        [HttpGet("GetAllProductsPaginated")]
        public ActionResult<IEnumerable<ProductDto>> GetProductsPaginated(int page)
        {
            return Ok(Products.GetProductsPaginated(page));
        }
    }
}
