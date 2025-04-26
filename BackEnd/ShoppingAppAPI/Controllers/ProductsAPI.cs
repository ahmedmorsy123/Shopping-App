using Microsoft.AspNetCore.Mvc;
using ShoppingAppBussiness;
using static ShoppingAppDB.ProductData;

namespace ShoppingAppAPI.Controllers
{
    [Route("api/Products")]
    [ApiController]
    public class ProductsAPI : ControllerBase
    {
        [HttpGet("GetAllProductsPaginated")]
        public ActionResult<IEnumerable<ProductDto>> GetProductsPaginated(int page)
        {
            return Ok(Products.GetProductsPaginated(page));
        }
    }
}
