using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
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
        public async Task<UserResponseDTO> Create([FromBody] UserDTO user)
        {
            return await _userService.AddNewUser(user);
        }

        [HttpPost("authenticate")]
        public async Task<UserAuthenticateResponseDTO> Authenticate([FromBody] UserAuthenticationDTO user)
        {
            return await _userService.Authenticate(user);
        }

        [HttpPut("changepassword")]
        public async Task<UserResponseDTO> ChangePassword([FromBody] ChangePasswordDTO user)
        {
            return await _userService.ChangePassword(user);
        }

        [Authorize]
        [HttpPut("update/{userId}")]
        public async Task<UserResponseDTO> Update([FromRoute] Guid userId, [FromBody] UserUpdateDTO user)
        {
            return await _userService.UpdateUserData(userId, user);
        }

        [Authorize]
        [HttpDelete("remove/{userId}")]
        public async Task<Guid> Remove([FromRoute] Guid userId)
        {
            return await _userService.DeleteUser(userId);
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
