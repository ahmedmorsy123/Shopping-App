using Microsoft.AspNetCore.Mvc;
using ShoppingAppBussiness;
using ShoppingAppDB.Data;
using ShoppingAppDB.Data.Seeder;
using static ShoppingAppDB.OrderData;

namespace ShoppingAppAPI.Controllers
{
    [Route("api/Orders")]
    [ApiController]
    public class OrdersAPI : ControllerBase
    {
        [HttpGet("GetUserOrders")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<OrderDto>> GetUserOrders(int userId) 
        {
            var orders = Orders.GetUserOrders(userId);
            if (orders == null) return NotFound();
            return Ok(orders);
        }

        [HttpGet("GetOrderById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<OrderDto> GetOrderById(int orderId)
        {
            var order = Orders.GetOrderById(orderId);
            if (order == null) return NotFound();
            return Ok(order);
        }

        [HttpPost("AddOrders")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<OrderDto> AddOrders(OrderDto order)
        {
            if(Users.GetCurrentUser() == null) return BadRequest("Please Login First");
            int id = Orders.AddOrder(order);
            order.Id = id;
            return CreatedAtAction(nameof(GetUserOrders), new { id = order.Id }, order);
        }
    }
}
