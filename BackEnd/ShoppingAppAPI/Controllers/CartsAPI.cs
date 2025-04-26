using Microsoft.AspNetCore.Mvc;
using ShoppingAppBussiness;
using static ShoppingAppDB.CartData;

namespace ShoppingAppAPI.Controllers
{
    [Route("api/Carts")]
    [ApiController]
    public class CartsAPI : ControllerBase
    { 
        [HttpGet("GetUserCart")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<CartDto>> GetUserCart(int userId)
        {
            var cart = Carts.GetUserCart(userId);
            if (cart == null) return NotFound("Thre is no cart for this user or user is not logged in");
            return Ok(cart);
        }

        [HttpPut("UpdateUserCart")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<CartDto> UpdateUserCart(CartDto cart)
        {
            bool result = Carts.UpdateCart(cart);
            if (result == false)
            {
                if (Users.GetCurrentUser() == null) return BadRequest("Please Login First");

                Carts.AddCart(cart);
            }
            return Ok(cart);
        }

        [HttpPost("AddCart")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<CartDto> AddCart(CartDto cart)
        {
            if (Users.GetCurrentUser() == null) return BadRequest("Please Login First");

            int id = Carts.AddCart(cart);
            cart.CartId = id;
            return CreatedAtAction(nameof(GetUserCart), new { id = cart.CartId }, cart);
        }

        [HttpDelete("DeleteCart")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteCart(int cartId)
        {
            bool result = Carts.DeleteCart(cartId);
            if (result == false) return NotFound("Thre is no user with this id");
            return Ok();
        }
    }
}
