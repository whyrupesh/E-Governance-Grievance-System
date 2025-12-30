using Backend.DTOs.Auth;
using Backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok("Auth api is alive");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register( RegisterDto dto)
        {
            var result = await _authService.RegisterAsync(dto);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login( LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);
            return Ok(result);
        }



    }
}
