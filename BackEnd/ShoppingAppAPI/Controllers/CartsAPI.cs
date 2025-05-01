using Microsoft.AspNetCore.Mvc;
using Serilog.Context;
using ShoppingAppBussiness;
using static ShoppingAppDB.CartData;

namespace ShoppingAppAPI.Controllers
{
    [Route("api/Carts")]
    [ApiController]
    public class CartsAPI : ControllerBase
    {
        private readonly ILogger<CartsAPI> _logger;
        private readonly Users _usersService;
        private readonly Carts _cartsService;
        private const string _prefix = "CartsAPI ";

        public CartsAPI(ILogger<CartsAPI> logger, Users users, Carts cartsService) 
        {
            _logger = logger;
            _usersService = users;
            _cartsService = cartsService; 
        }

        [HttpGet("GetCurrentUserCart")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<IEnumerable<CartDto>> GetCurrentUserCart()
        {
            _logger.LogInformation($"{_prefix}Get Current User Cart");
            if (_usersService.GetCurrentUser() == null)
            {
                _logger.LogWarning($"{_prefix}Please Login First");
                return BadRequest("Please Login First");
            }
            var cart = _cartsService.GetCurrentUserCart();
            if (cart == null)
            {
                _logger.LogWarning($"{_prefix}There is no cart for this user");
                return NotFound("Thre is no cart for this user");
            }
            return Ok(cart);
        }

        [HttpPut("UpdateCart")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<CartDto> UpdateCart(CartDto cart)
        {
            _logger.LogInformation($"{_prefix}Update Cart");
            if (_usersService.GetCurrentUser() == null)
            {
                _logger.LogWarning($"{_prefix}User is not logged in");
                return BadRequest("Please Login First");
            }

            bool result = _cartsService.UpdateCart(cart);
            int id = cart.CartId;
            if (result == false)
            {
                _logger.LogInformation($"{_prefix}There is no cart with this id so add new one");
                id = _cartsService.AddCart(cart);
            }
            else
            {
                _logger.LogInformation($"{_prefix}Cart was updated");
            }
            cart.CartId = id;
            return Ok(cart);
        }

        [HttpPost("AddCart")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<CartDto> AddCart(CartDto cart)
        {
            _logger.LogInformation($"{_prefix}Add Cart");
            if (_usersService.GetCurrentUser() == null)
            {
                _logger.LogWarning($"{_prefix}Please Login First");
                return BadRequest("Please Login First");
            }
            if (_cartsService.CurrentUserHaveCart())
            {
                _logger.LogWarning($"{_prefix}User already have a cart, so no cart was added");
                return BadRequest("You already have a cart");
            }

            int id = _cartsService.AddCart(cart);
            cart.CartId = id;
            _logger.LogInformation($"{_prefix}Cart was added with id {id}");
            return CreatedAtAction(nameof(GetCurrentUserCart), cart);
        }

        [HttpDelete("DeleteCart")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteCart(int cartId)
        {
            _logger.LogInformation($"{_prefix}Delete Cart");
            bool result = _cartsService.DeleteCart(cartId);
            if (result == false)
            {
                _logger.LogWarning($"{_prefix}Cart was not found");
                return NotFound("Thre is no user with this id");
            }
            _logger.LogInformation($"{_prefix}Cart was deleted successfully");
            return Ok();
        }
    }
}
