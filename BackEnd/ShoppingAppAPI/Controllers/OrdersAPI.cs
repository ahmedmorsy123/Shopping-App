﻿using Microsoft.AspNetCore.Authorization;
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

        [HttpGet("GetUserOrders")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<OrderDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetUserOrders(int userId)
        {
            _logger.LogInformation($"{_prefix}Get User Orders");

            var orders = await _ordersService.GetUserOrdersAsync(userId);
            if (orders == null)
            {
                _logger.LogWarning($"{_prefix}There is no orders for this user");
                return NotFound(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Orders Not Found",
                    Detail = "There are no orders for this user.",
                    Instance = HttpContext.Request.Path
                });
            }
            return Ok(orders);
        }

        [HttpGet("GetOrderById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<OrderDto>> GetOrderById(int orderId)
        {
            _logger.LogInformation($"{_prefix}Get Order By Id");
            var order = await _ordersService.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                _logger.LogWarning($"{_prefix}There is no order with this id");
                return NotFound(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Order Not Found",
                    Detail = "There is no order with this id.",
                    Instance = HttpContext.Request.Path
                });
            }
            return Ok(order);
        }

        [HttpPost("MakeOrders")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OrderDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<OrderDto>> AddOrders(int userId, string shippingAddress, string paymentMethod)
        {
            _logger.LogInformation($"{_prefix}Add Order");

            OrderDto order = await _ordersService.AddOrderAsync(userId, shippingAddress, paymentMethod);

            if (order == null)
            {
                _logger.LogError($"{_prefix}Order was not created");
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Order Creation Failed",
                    Detail = "Order was not created.",
                    Instance = HttpContext.Request.Path
                });
            }

            return CreatedAtAction(nameof(GetUserOrders), new { id = order.Id }, order);
        }

        [HttpDelete("CancelOrder")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> CancelOrder(int orderId)
        {
            _logger.LogInformation($"{_prefix}Cancel Order");
            var result = await _ordersService.GetOrderByIdAsync(orderId);
            if (result == null)
            {
                _logger.LogWarning($"{_prefix}There is no order with this id");
                return NotFound(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Order Not Found",
                    Detail = "There is no order with this id.",
                    Instance = HttpContext.Request.Path
                });
            }
            await _ordersService.CancelOrderAsync(orderId);
            return Ok();
        }
    }
}