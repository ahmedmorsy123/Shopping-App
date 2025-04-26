﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingAppBussiness;
using static ShoppingAppDB.UsersData;

namespace ShoppingAppAPI.Controllers
{
    [Route("api/Users")]
    [ApiController]
    public class UsersAPI : ControllerBase
    {

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<UserDto>> GetUser(int id)
        {
            var user = Users.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(Users.GetUserById(id));
            
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<UserDto> UpdateUser(UserDto user, string oldPassword)
        {
            bool result = Users.UpdateUser(user, oldPassword);
            if (result == false) return NotFound("Thre is no user with this id");
            return Ok(user);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteUser(int id)
        {
            bool result = Users.DeleteUser(id);
            if (result == false) return NotFound("Thre is no user with this id");
            return Ok();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<UserDto> AddUser(UserDto user)
        {
            int id = Users.AddUser(user);
            user.Id = id;
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        [HttpGet("CurrentUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<UserDto> GetCurrentUser()
        {
            UserDto result = Users.GetCurrentUser();
            if (result == null) return NotFound("There is no Login user");
            return Ok(result);
        }


        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<UserDto> Login(string userName, string password)
        {
            bool result = Users.Login(userName, password);
            if (result == false) return Unauthorized("Invalid user name or password");
            return Ok("User Login Successfully");
        }

        [HttpPost("Logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<UserDto> Logout()
        {
            Users.Logout();
            return Ok("User Logout Successfully");
        }
    }
}
