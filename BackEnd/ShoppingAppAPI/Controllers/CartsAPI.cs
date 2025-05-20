using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingAppBussiness;
using ShoppingAppDB.Models;

namespace ShoppingAppAPI.Controllers
{
    [Route("api/Carts")]
    [Authorize]
    [ApiController]
    public class CartsAPI : ControllerBase
    {
        private readonly ILogger<CartsAPI> _logger;
        private readonly CartsService _cartsService;
        private const string _prefix = "CartsAPI ";

        public CartsAPI(ILogger<CartsAPI> logger, CartsService cartsService)
        {
            _logger = logger;
            _cartsService = cartsService;
        }

        [HttpGet("GetUserCart")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CartDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<CartDto>> GetUserCart(int UserId)
        {
            _logger.LogInformation($"{_prefix}Get Current User Cart");

            var cart = await _cartsService.GetUserCartAsync(UserId);
            if (cart == null)
            {
                _logger.LogWarning($"{_prefix}There is no cart for this user");
                return NotFound(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Cart Not Found",
                    Detail = "There is no cart for this user.",
                    Instance = HttpContext.Request.Path
                });
            }
            return Ok(cart);
        }

        [HttpPut("UpdateCart")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CartDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<CartDto>> UpdateCart(CartDto cart)
        {
            _logger.LogInformation($"{_prefix}Update Cart");

            bool result = await _cartsService.UpdateCartAsync(cart);
            int id = cart.CartId;
            if (result == false)
            {
                _logger.LogInformation($"{_prefix}There is no cart with this id so add new one");
                id = await _cartsService.AddCartAsync(cart);
            }
            else
            {
                _logger.LogInformation($"{_prefix}Cart was updated");
            }
            cart.CartId = id;
            return Ok(cart);
        }

        [HttpPost("AddCart")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CartDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<CartDto>> AddCart(CartDto cart)
        {
            _logger.LogInformation($"{_prefix}Add Cart");

            int id = await _cartsService.AddCartAsync(cart);
            cart.CartId = id;
            _logger.LogInformation($"{_prefix}Cart was added with id {id}");
            return CreatedAtAction(nameof(GetUserCart), new { UserId = cart.UserId }, cart);
        }

        [HttpDelete("DeleteCart")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> DeleteCart(int cartId)
        {
            _logger.LogInformation($"{_prefix}Delete Cart");
            bool result = await _cartsService.DeleteCartAsync(cartId);
            if (result == false)
            {
                _logger.LogWarning($"{_prefix}Cart was not found");
                return NotFound(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Cart Not Found",
                    Detail = "There is no cart with this id.",
                    Instance = HttpContext.Request.Path
                });
            }
            _logger.LogInformation($"{_prefix}Cart was deleted successfully");
            return Ok();
        }
    }
}