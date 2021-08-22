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
            return Ok(_userService.AddNewUser(user));
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] UserDTO user)
        {
            return Ok(_userService.Authenticate(user));
        }

        [Authorize]
        [HttpPut("update/{userId}")]
        public IActionResult Update([FromRoute] Guid userId, [FromBody] UserDTO user)
        {
            return Ok(_userService.UpdateUser(userId, user));
        }

        [Authorize]
        [HttpDelete("remove/{userId}")]
        public IActionResult Remove([FromRoute] Guid userId)
        {
            return Ok(_userService.DeleteUser(userId));
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            return Ok(_userService.GetAllUsers());
        }
    }
}
