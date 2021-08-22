using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using UsersCrud.Domain.DTO;
using UsersCrud.Domain.ServicesInterfaces;

namespace UsersCRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize]
        [HttpPost("create")]
        public IActionResult Create([FromBody] UserDTO user)
        {
            return Execute(() => _userService.AddNewUser(user));
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] UserAuthenticationDTO user)
        {
            return Execute(() =>_userService.Authenticate(user));
        }

        [HttpPut("changepassword")]
        public IActionResult ChangePassword([FromBody] ChangePasswordDTO user)
        {
            return Execute(() => _userService.ChangePassword(user));
        }

        [Authorize]
        [HttpPut("update/{userId}")]
        public IActionResult Update([FromRoute] Guid userId, [FromBody] UserUpdateDTO user)
        {
            return Execute(() => _userService.UpdateUserData(userId, user));
        }

        [Authorize]
        [HttpDelete("remove/{userId}")]
        public IActionResult Remove([FromRoute] Guid userId)
        {
            return Execute(() => _userService.DeleteUser(userId));
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            return Execute(() => _userService.GetAllUsers());
        }

        private IActionResult Execute(Func<object> func)
        {
            try
            {
                var result = func();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
