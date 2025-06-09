using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingAppBussiness;
using ShoppingAppDB.Models;
using static ShoppingAppDB.Enums.Enums;

namespace ShoppingAppAPI.Controllers
{
    [Route("api/Orders")]
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

        [Authorize]
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

        [Authorize]
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

        [Authorize]
        [HttpPost("MakeOrders")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OrderDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<OrderDto?>> AddOrders(int userId, string shippingAddress, string paymentMethod)
        {
            _logger.LogInformation($"{_prefix}Add Order");

            if(string.IsNullOrEmpty(shippingAddress) || string.IsNullOrEmpty(paymentMethod)) 
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Order Creation Failed",
                    Detail = "Shipping Address and/or PaymentMethod Can't by empty.",
                    Instance = HttpContext.Request.Path
                }); 

            OrderDto? order = await _ordersService.AddOrderAsync(userId, shippingAddress, paymentMethod);

            if (order == null)
            {
                _logger.LogError($"{_prefix}Order was not created");
                return NotFound(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Order Creation Failed",
                    Detail = "The Cart is empty.",
                    Instance = HttpContext.Request.Path
                });
            }

            return CreatedAtAction(nameof(GetUserOrders), new { id = order.Id }, order);
        }

        [Authorize]
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

        //[Authorize(Roles = "Admin")]
        [HttpPut("ProcessOrder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> ProcessOrder(int orderId)
        {
            _logger.LogInformation($"{_prefix}ProcessOrder called for orderId: {orderId}");
            if (orderId <= 0)
            {
                _logger.LogWarning($"{_prefix}Invalid orderId: {orderId}");
                return BadRequest(new ProblemDetails()
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Invalid Order ID",
                    Detail = "Order ID must be greater than zero.",
                    Instance = HttpContext.Request.Path
                });
            }

            var status = await _ordersService.GetOrderStatusAsync(orderId);
            if (status != OrderStatus.Pending)
            {
                _logger.LogInformation($"Can't proccess order because it is not pending");
                return BadRequest(new ProblemDetails()
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Process order faild",
                    Detail = "Order is not in Pending state",
                    Instance = HttpContext.Request.Path
                });
            }

            var result = await _ordersService.ProcessOrderAsync(orderId);
            if (!result)
            {
                _logger.LogWarning($"{_prefix}Failed to process order with id: {orderId}");
                return NotFound(new ProblemDetails()
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Order Not Found",
                    Detail = "No order found with the provided ID.",
                    Instance = HttpContext.Request.Path
                });
            }

            return NoContent();
        }

        //[Authorize(Roles = "Admin")]
        [HttpPut("ShipOrder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> ShipOrder(int orderId)
        {
            _logger.LogInformation($"{_prefix}ShipOrder called for orderId: {orderId}");
            if (orderId <= 0)
            {
                _logger.LogWarning($"{_prefix}Invalid orderId: {orderId}");
                return BadRequest(new ProblemDetails()
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Invalid Order ID",
                    Detail = "Order ID must be greater than zero.",
                    Instance = HttpContext.Request.Path
                });
            }

            var status = await _ordersService.GetOrderStatusAsync(orderId);
            if (status != OrderStatus.Processing)
            {
                _logger.LogInformation($"Can't ship order because it is not processed");
                return BadRequest(new ProblemDetails()
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Ship order faild",
                    Detail = "Order is not in Processing state",
                    Instance = HttpContext.Request.Path
                });
            }

            var result = await _ordersService.ShipOrderAsync(orderId);
            if (!result)
            {
                _logger.LogWarning($"{_prefix}Failed to ship order with id: {orderId}");
                return NotFound(new ProblemDetails()
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Order Not Found",
                    Detail = "No order found with the provided ID.",
                    Instance = HttpContext.Request.Path
                });
            }


            return NoContent();
        }

        //[Authorize(Roles = "Admin")]
        [HttpPut("DeliverOrder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> DeliverOrder(int orderId)
        {
            _logger.LogInformation($"{_prefix}DeliverOrder called for orderId: {orderId}");
            if (orderId <= 0)
            {
                _logger.LogWarning($"{_prefix}Invalid orderId: {orderId}");
                return BadRequest(new ProblemDetails()
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Invalid Order ID",
                    Detail = "Order ID must be greater than zero.",
                    Instance = HttpContext.Request.Path
                });
            }

            var status = await _ordersService.GetOrderStatusAsync(orderId);
            if (status != OrderStatus.Shipped)
            {
                _logger.LogInformation($"Can't proccess order because it is not pending");
                return BadRequest(new ProblemDetails()
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Deliver Order Faild",
                    Detail = "Order is not in Shipping state",
                    Instance = HttpContext.Request.Path
                });

            }
            var result = await _ordersService.DeliverOrderAsync(orderId);
            if (!result)
            {
                _logger.LogWarning($"{_prefix}Failed to deliver order with id: {orderId}");
                return NotFound(new ProblemDetails()
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Order Not Found",
                    Detail = "No order found with the provided ID.",
                    Instance = HttpContext.Request.Path
                });
            }


            return NoContent();
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet("GetOrdersByDurationAndStatus")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<OrderDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersByDurationAndStatus(
                                [FromQuery] TimeDuration duration, [FromQuery] OrderStatus status)
        {
            _logger.LogInformation($"{_prefix}GetOrdersByDurationAndStatus called with duration: {duration}, status: {status}");
            var orders = await _ordersService.GetOrdersByDurationAndStatusAsync(duration, status);
            if (orders == null || !orders.Any())
            {
                _logger.LogWarning($"{_prefix}No orders found for the specified duration and status");
                return NotFound(new ProblemDetails()
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "No Orders Found",
                    Detail = "There are no orders matching the specified criteria.",
                    Instance = HttpContext.Request.Path
                });
            }
            return Ok(orders);
        }
    }
}