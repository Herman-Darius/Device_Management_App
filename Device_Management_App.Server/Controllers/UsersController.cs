using Microsoft.AspNetCore.Mvc;

namespace Device_Management_App.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpPatch("{id}/role")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] string newRole)
        {
            var success = await _userService.UpdateUserRoleAsync(id, newRole);
            if (!success) return NotFound();

            return NoContent();
        }
    }
}