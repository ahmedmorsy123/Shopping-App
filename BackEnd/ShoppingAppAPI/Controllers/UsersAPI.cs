using Microsoft.AspNetCore.Http;
using Serilog.Context;
using Microsoft.AspNetCore.Mvc;
using ShoppingAppBussiness;
using static ShoppingAppDB.UserData;

namespace ShoppingAppAPI.Controllers
{
    [Route("api/Users")]
    [ApiController]
    public class UsersAPI : ControllerBase
    {
        private readonly ILogger<UsersAPI> _logger;
        private readonly Users _usersService;
        private const string _prefix = "UsersAPI ";

        public UsersAPI(Users usersService, ILogger<UsersAPI> logger)
        {
            _usersService = usersService;
            _logger = logger;
        }

        [HttpGet("getUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<UserDto>> GetUser(int id)
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
        public ActionResult<UserDto> AddUser(UserDto user)
        {
            _logger.LogInformation($"{_prefix}AddUserAPI");
            int id = _usersService.AddUser(user);
            user.Id = id;
            _logger.LogInformation($"{_prefix}Added user with id: {user.Id}");
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        [HttpGet("CurrentUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<UserDto> GetCurrentUser()
        {
            _logger.LogInformation($"{_prefix}GetCurrentUserAPI");
            UserDto result = _usersService.GetCurrentUser();
            if (result == null)
            {
                _logger.LogWarning($"{_prefix}There is no logged-in user");
                return NotFound("There is no logged-in user");
            }
            return Ok(result);
        }

        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<UserDto> Login(string userName, string password)
        {
            _logger.LogInformation($"{_prefix}LoginAPI");
            bool result = _usersService.Login(userName, password);
            if (result == false)
            {
                _logger.LogWarning($"{_prefix}Invalid username or password");
                return Unauthorized("Invalid username or password");
            }
            _logger.LogInformation($"{_prefix}User logged in successfully");
            return Ok(_usersService.GetCurrentUser());
        }

        [HttpPost("Logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult Logout()
        {
            _logger.LogInformation($"{_prefix}LogoutAPI");
            _usersService.Logout();
            return Ok("User logged out successfully");
        }
    }
}
