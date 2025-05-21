using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingAppBussiness;
using ShoppingAppDB.Models;
using ShoppingAppDB.Services;
using System.Security.Claims;

namespace ShoppingAppAPI.Controllers
{
    [Route("api/Users")]
    [Authorize]
    [ApiController]
    public class UsersAPI : ControllerBase
    {
        private readonly ILogger<UsersAPI> _logger;
        private readonly UsersService _usersService;
        private const string _prefix = "UsersAPI ";

        public UsersAPI(UsersService usersService, ILogger<UsersAPI> logger)
        {
            _usersService = usersService;
            _logger = logger;
        }

        [HttpGet("getUser")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            _logger.LogInformation($"{_prefix}GetUserAPI");
            var user = await _usersService.GetUserByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning($"{_prefix}There is no user with this id");
                return NotFound(new ProblemDetails
                {
                    Title = "User not found",
                    Detail = "There is no user with this id",
                    Status = StatusCodes.Status404NotFound,
                    Instance = HttpContext.Request.Path
                });
            }
            return Ok(user);
        }

        [HttpPut("UpdateUser")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UserDto>> UpdateUser(UpdateUserDto user)
        {
            _logger.LogInformation($"{_prefix}UpdateUserAPI");
            var UpdatedUser = await _usersService.UpdateUserAsync(user);
            if (UpdatedUser == null)
            {
                _logger.LogWarning($"{_prefix}There is no user with this id or wrong password or you are not logged in");
                return NotFound(new ProblemDetails
                {
                    Title = "User update failed",
                    Detail = "wrong password",
                    Status = StatusCodes.Status400BadRequest,
                    Instance = HttpContext.Request.Path
                });
            }

            return Ok(UpdatedUser);
        }

        [HttpDelete("DeleteUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> DeleteUser(int id)
        {
            _logger.LogInformation($"{_prefix}DeleteUserAPI");
            bool result = await _usersService.DeleteUserAsync(id);
            if (result == false)
            {
                _logger.LogWarning($"{_prefix}There is no user with this id");
                return NotFound(new ProblemDetails
                {
                    Title = "User not found",
                    Detail = "There is no user with this id",
                    Status = StatusCodes.Status404NotFound,
                    Instance = HttpContext.Request.Path
                });
            }
            _logger.LogInformation($"{_prefix}User deleted successfully");
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("AddUser")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<UserDto>> AddUser(UserDto user)
        {

            _logger.LogInformation($"{_prefix}AddUserAPI");
            UserDto? newUser = await _usersService.AddUserAsync(user);

            if (newUser == null)
            {
                _logger.LogWarning($"{_prefix}User was not added");
                return BadRequest(new ProblemDetails
                {
                    Title = "User creation failed",
                    Detail = "UserName or Email already exists",
                    Status = StatusCodes.Status400BadRequest,
                    Instance = HttpContext.Request.Path
                });
            }
            _logger.LogInformation($"{_prefix}Added user with id: {user.Id}");
            return CreatedAtAction(nameof(GetUser), new { id = newUser.Id }, newUser);
        }
    }
}