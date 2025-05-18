using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingAppBussiness;
using ShoppingAppDB.Models;
using ShoppingAppDB.Services;
using System.Security.Claims;


namespace ShoppingAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Auth : ControllerBase
    {
        private const string _prefix = "AuthAPI ";

        private readonly AuthService _authService;
        private readonly ILogger<Auth> _logger;

        public Auth(ILogger<Auth> logger, AuthService authService)
        {
            _logger = logger;
            _authService = authService;
        }

        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TokenResponseDto>> Login(LoginRequestDto loginnRequest)
        {
            _logger.LogInformation($"{_prefix}LoginAPI");
            TokenResponseDto? result = await _authService.LoginAsync(loginnRequest);
            if (result is null)
            {
                _logger.LogWarning($"{_prefix}Invalid username or password");
                return BadRequest("Invalid username or password");
            }
            _logger.LogInformation($"{_prefix}User logged in successfully");
            //return Ok(result);
            return CreatedAtAction(nameof(GetUser), new { id = }, result);
        }

        [HttpPost("refresh-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<TokenResponseDto> RefreshToken(RefreshTokenRequestDto request)
        {
            var result = _authService.RefreshToken(request);
            if (result is null || result.AccessToken is null || result.RefreshToken is null)
                return BadRequest("Invalid refresh token.");

            return Ok(result);
        }

        [Authorize]
        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Logout()
        {
            // Extract user ID from claims
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim is null || !int.TryParse(userIdClaim.Value, out var userId))
                return BadRequest("Invalid user identifier.");

            var result = await _authService.LogoutAsync(userId);
            if (!result)
                return NotFound("User not found.");

            return Ok("Logged out successfully.");
        }
    }
}