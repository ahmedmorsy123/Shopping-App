using Microsoft.AspNetCore.Mvc;
using Serilog.Context;
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
        private readonly ILogger<OrdersAPI> _logger;
        private readonly Users _usersService;
        private readonly Orders _ordersService; 
        private readonly Carts _cartsService;
        private const string _prefix = "OrdersAPI ";

        public OrdersAPI(ILogger<OrdersAPI> logger, Users users, Orders orders) 
        {
            _logger = logger;
            _usersService = users;
            _ordersService = orders; 
        }

        [HttpGet("GetCurrentUserOrders")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<IEnumerable<OrderDto>> GetUserOrders()
        {
            _logger.LogInformation($"{_prefix}Get User Orders");
            if (_usersService.GetCurrentUser() == null)
            {
                _logger.LogWarning($"{_prefix}User is not logged in");
                return BadRequest("Please Login First");
            }
            int userId = _usersService.GetCurrentUser().Id;
            var orders = _ordersService.GetUserOrders(userId); 
            if (orders == null)
            {
                _logger.LogWarning($"{_prefix}There is no orders for this user");
                return NotFound("There is no orders for this user");
            }
            return Ok(orders);
        }

        [HttpGet("GetOrderById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<OrderDto> GetOrderById(int orderId)
        {
            _logger.LogInformation($"{_prefix}Get Order By Id");
            var order = _ordersService.GetOrderById(orderId);
            if (order == null)
            {
                _logger.LogWarning($"{_prefix}There is no order with this id");
                return NotFound();
            }
            return Ok(order);
        }

        [HttpPost("MakeOrders")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<OrderDto> AddOrders(string shippingAddress, string paymentMethod)
        {
            _logger.LogInformation($"{_prefix}Add Order");

            if (_usersService.GetCurrentUser() == null)
            {
                _logger.LogWarning($"{_prefix}User is not logged in");
                return BadRequest("Please Login First");
            }

            if (!_cartsService.CurrentUserHaveCart())
            {
                _logger.LogWarning($"{_prefix}User has no cart");
                return BadRequest("User has no cart");
            }

            OrderDto order = _ordersService.AddOrder(shippingAddress, paymentMethod);

            if (order == null)
            {
                _logger.LogError($"{_prefix}Order was not created");
                return BadRequest("Order was not created");
            }

            return CreatedAtAction(nameof(GetUserOrders), new { id = order.Id }, order);
        }

        [HttpDelete("CancelOrder")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult CancelOrder(int orderId)
        {
            _logger.LogInformation($"{_prefix}Cancel Order");
            var result = _ordersService.GetOrderById(orderId); 
            if (result == null)
            {
                _logger.LogWarning($"{_prefix}Thre is no order with this id");
                return NotFound("Thre is no order with this id");
            }
            _ordersService.CancelOrder(orderId); 
            return Ok();
        }
    }
}
