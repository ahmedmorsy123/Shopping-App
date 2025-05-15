using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingAppBussiness;
using ShoppingAppDB.Models;

namespace ShoppingAppAPI.Controllers
{
    [Route("api/Orders")]
    [Authorize]
    [ApiController]
    public class OrdersAPI : ControllerBase
    {
        private readonly ILogger<OrdersAPI> _logger;
        private readonly OrdersService _ordersService;
        private const string _prefix = "OrdersAPI ";

        public OrdersAPI(ILogger<OrdersAPI> logger, OrdersService orders)
        {
            _logger = logger;
            _ordersService = orders;
        }

        [HttpGet("GetCurrentUserOrders")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetUserOrders(int userId)
        {
            _logger.LogInformation($"{_prefix}Get User Orders");

            var orders = await _ordersService.GetUserOrders(userId);
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
        public async Task<ActionResult<OrderDto>> GetOrderById(int orderId)
        {
            _logger.LogInformation($"{_prefix}Get Order By Id");
            var order = await _ordersService.GetOrderById(orderId);
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
        public async Task<ActionResult<OrderDto>> AddOrders(int userId, string shippingAddress, string paymentMethod)
        {
            _logger.LogInformation($"{_prefix}Add Order");

            OrderDto order = await _ordersService.AddOrder(userId, shippingAddress, paymentMethod);

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
        public async Task<ActionResult> CancelOrder(int orderId)
        {
            _logger.LogInformation($"{_prefix}Cancel Order");
            var result = await _ordersService.GetOrderById(orderId);
            if (result == null)
            {
                _logger.LogWarning($"{_prefix}Thre is no order with this id");
                return NotFound("Thre is no order with this id");
            }
            await _ordersService.CancelOrder(orderId);
            return Ok();
        }
    }
}