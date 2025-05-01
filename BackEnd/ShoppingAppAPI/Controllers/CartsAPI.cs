using Microsoft.AspNetCore.Mvc;
using ShoppingAppBussiness;
using static ShoppingAppDB.CartData;

namespace ShoppingAppAPI.Controllers
{
    [Route("api/Carts")]
    [ApiController]
    public class CartsAPI : ControllerBase
    { 
        [HttpGet("GetCurrentUserCart")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<IEnumerable<CartDto>> GetCurrentUserCart()
        {
            if(Users.GetCurrentUser() == null) return BadRequest("Please Login First");
            var cart = Carts.GetCurrentUserCart();
            if (cart == null) return NotFound("Thre is no cart for this user");
            return Ok(cart);
        }

        [HttpPut("UpdateCart")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<CartDto> UpdateCart(CartDto cart)
        {
            if (Users.GetCurrentUser() == null) return BadRequest("Please Login First");

            bool result = Carts.UpdateCart(cart);
            int id = cart.CartId;
            if (result == false)
            {
                id = Carts.AddCart(cart);
            }
            cart.CartId = id;
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
            return CreatedAtAction(nameof(GetCurrentUserCart), cart);
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
