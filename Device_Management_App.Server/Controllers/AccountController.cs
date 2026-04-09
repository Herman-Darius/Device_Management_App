using Device_Management_App.Server.Data;
using Device_Management_App.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace Device_Management_App.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(User user)
        {
            var (success, message) = await _accountService.RegisterAsync(user);
            if (!success) return BadRequest(message);

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            var (success, message, data) = await _accountService.LoginAsync(login);
            if (!success) return Unauthorized(message);

            return Ok(data);
        }
    }
}