using Microsoft.AspNetCore.Mvc;
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

        [HttpPost]
        public IActionResult Create([FromBody] UserDTO user)
        {
            return Ok(_userService.AddNewUser(user));
        }
    }
}
