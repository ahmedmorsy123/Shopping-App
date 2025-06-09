using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingAppBussiness;
using ShoppingAppDB.Models;
using ShoppingAppDB.Services;
using System.Security.Claims;
using static ShoppingAppDB.Enums.Enums;


namespace ShoppingAppAPI.Controllers
{
    [Route("api/Auth")]
    [ApiController]
    public class AuthAPI : ControllerBase
    {
        private const string _prefix = "AuthAPI ";

        private readonly AuthService _authService;
        private readonly ILogger<AuthAPI> _logger;

        public AuthAPI(ILogger<AuthAPI> logger, AuthService authService)
        {
            _logger = logger;
            _authService = authService;
        }

        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<TokenResponseDto>> Login(LoginRequestDto loginnRequest)
        {
            _logger.LogInformation($"{_prefix}LoginAPI");
            TokenResponseDto? result = await _authService.LoginAsync(loginnRequest);
            if (result is null)
            {
                _logger.LogWarning($"{_prefix}Invalid username or password");
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Invalid username or password",
                    Detail = "The username or password provided is incorrect.",
                    Instance = HttpContext.Request.Path
                });
            }
            _logger.LogInformation($"{_prefix}User logged in successfully");
            return Ok(result);
        }

        [HttpPost("refresh-token")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        public ActionResult<TokenResponseDto?> RefreshToken(RefreshTokenRequestDto request)
        {
            var result = _authService.RefreshToken(request);
            if (result is null || result.AccessToken is null || result.RefreshToken is null)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Invalid refresh token",
                    Detail = "The refresh token is invalid or expired.",
                    Instance = HttpContext.Request.Path
                });
            }

            return Ok(result);
        }

        [Authorize]
        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        public async Task<ActionResult> Logout()
        {
            // Extract user ID from claims
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim is null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Invalid user identifier",
                    Detail = "Could not extract a valid user identifier from the claims.",
                    Instance = HttpContext.Request.Path
                });
            }

            var result = await _authService.LogoutAsync(userId);
            if (!result)
            {
                return NotFound(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "User not found or Refresh token expired",
                    Detail = "The user was not found or the refresh token has expired.",
                    Instance = HttpContext.Request.Path
                });
            }

            return Ok("Logged out successfully.");
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet("GetLoginCountByDuration")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<int>> GetLoginCountByDuration([FromQuery] TimeDuration duration)
        {
            _logger.LogInformation($"{_prefix}GetLoginCountByDuration called with duration: {duration}");
            return Ok(await _authService.GetLoginCountByDurationAsync(duration));
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet("GetRegisterationCountByDuration")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<int>> GetRegisterationCountByDuration([FromQuery] TimeDuration duration)
        {
            _logger.LogInformation($"{_prefix}GetRegisterationCountByDuration called with duration: {duration}");
            return Ok(await _authService.GetRegisterationCountByDurationAsync(duration));
        }

    }
}