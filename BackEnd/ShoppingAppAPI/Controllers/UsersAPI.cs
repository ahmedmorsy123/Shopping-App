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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<UserDto> GetUser(int id)
        {
            _logger.LogInformation($"{_prefix}GetUserAPI");
            var user = _usersService.GetUserById(id);
            if (user == null)
            {
                _logger.LogWarning($"{_prefix}There is no user with this id");
                return NotFound();
            }
            return Ok(_usersService.GetUserById(id));
        }

        [HttpPut("UpdateUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<UserDto> UpdateUser(UserDto user, string oldPassword)
        {
            _logger.LogInformation($"{_prefix}UpdateUserAPI");
            bool result = _usersService.UpdateUser(user, oldPassword);
            if (result == false)
            {
                _logger.LogWarning($"{_prefix}There is no user with this id or wrong password or you are not logged in");
                return NotFound("There is no user with this id or wrong password or you are not logged in");
            }

            UserDto UpdatedUser = _usersService.GetUserById(user.Id);
            return Ok(UpdatedUser);
        }

        [HttpDelete("DeleteUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteUser(int id)
        {
            _logger.LogInformation($"{_prefix}DeleteUserAPI");
            bool result = _usersService.DeleteUser(id);
            if (result == false)
            {
                _logger.LogWarning($"{_prefix}There is no user with this id");
                return NotFound("There is no user with this id");
            }
            _logger.LogInformation($"{_prefix}User deleted successfully");
            return Ok();
        }

        [HttpPost("AddUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<UserDto> AddUser(UserDto user)
        {
            _logger.LogInformation($"{_prefix}AddUserAPI");
            UserDto? newUser = _usersService.AddUser(user);

            if (newUser == null)
            {
                _logger.LogWarning($"{_prefix}User was not added");
                return BadRequest("UserName or Email already exists");
            }
            _logger.LogInformation($"{_prefix}Added user with id: {user.Id}");
            return CreatedAtAction(nameof(GetUser), new { id = newUser.Id }, newUser);
        }
    }
}