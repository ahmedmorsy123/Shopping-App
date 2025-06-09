using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingAppBussiness;
using ShoppingAppDB.Models;
using static ShoppingAppDB.AdminData;
using static ShoppingAppDB.Enums.Enums;

namespace ShoppingAppAPI.Controllers
{
    [Route("api/Admin")]
    [ApiController]
    //[Authorize(Roles = "Admin")]
    public class AdminApi : ControllerBase
    {
        private const string _prefix = "AuthAPI ";

        private readonly AdminService _adminService;
        private readonly ILogger<AdminApi> _logger;

        public AdminApi(ILogger<AdminApi> logger, AdminService adminService)
        {
            _logger = logger;
            _adminService = adminService;
        }

        [HttpPost("AddAdmin")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
        public async Task<ActionResult<UserDto>> AddAdmin([FromBody] UserDto admin)
        {
            _logger.LogInformation($"{_prefix}AddAdmin called for admin: {admin.Name}");
            if (admin == null)
            {
                _logger.LogWarning($"{_prefix}Admin data is null");
                return BadRequest(new ProblemDetails()
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Invalid Admin Data",
                    Detail = "Admin data cannot be null.",
                    Instance = HttpContext.Request.Path
                });
            }
            var result = await _adminService.AddAdminAsync(admin);
            if (result == null)
            {
                _logger.LogWarning($"{_prefix}Failed to add admin: {admin.Name}");
                return BadRequest(new ProblemDetails()
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Admin Creation Failed",
                    Detail = "An admin with this email or username already exists.",
                    Instance = HttpContext.Request.Path
                });
            }
            return Ok(result);
        }

        [HttpDelete("RemoveAdmin")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> RemoveAdmin([FromQuery] int adminId)
        {
            _logger.LogInformation($"{_prefix}RemoveAdmin called for adminId: {adminId}");
            if (adminId <= 0)
            {
                _logger.LogWarning($"{_prefix}Invalid adminId: {adminId}");
                return BadRequest(new ProblemDetails()
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Invalid Admin ID",
                    Detail = "Admin ID must be greater than zero.",
                    Instance = HttpContext.Request.Path
                });
            }
            var result = await _adminService.RemoveAdminAsync(adminId);
            if (!result)
            {
                _logger.LogWarning($"{_prefix}Failed to remove admin with id: {adminId}");
                return NotFound(new ProblemDetails()
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Admin Not Found",
                    Detail = "No admin found with the provided ID.",
                    Instance = HttpContext.Request.Path
                });
            }
            return NoContent();
        }

        [HttpPut("MakeAdmin")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> MakeAdmin([FromBody] int userId)
        {
            _logger.LogInformation($"{_prefix}MakeAdmin called for userId: {userId}");
            if (userId <= 0)
            {
                _logger.LogWarning($"{_prefix}Invalid userId: {userId}");
                return BadRequest(new ProblemDetails()
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Invalid User ID",
                    Detail = "User ID must be greater than zero.",
                    Instance = HttpContext.Request.Path
                });
            }
            var result = await _adminService.MakeAdminAsync(userId);
            if (!result)
            {
                _logger.LogWarning($"{_prefix}Failed to make user with id {userId} an admin");
                return NotFound(new ProblemDetails()
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "User Not Found",
                    Detail = "No user found with the provided ID.",
                    Instance = HttpContext.Request.Path
                });
            }
            return NoContent();
        }

        [HttpGet("ListAdmins")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserDto>))]
        public async Task<ActionResult<IEnumerable<UserDto>>> ListAdmins()
        {
            _logger.LogInformation($"{_prefix}ListAdmins called");
            var admins = await _adminService.ListAdminsAsync();
            if (admins == null || !admins.Any())
            {
                _logger.LogWarning($"{_prefix}No admins found");
                return NotFound(new ProblemDetails()
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "No Admins Found",
                    Detail = "There are no admins in the system.",
                    Instance = HttpContext.Request.Path
                });
            }
            return Ok(admins);
        }
    }
}