using Assignment_2_BE.DTOs;
using Assignment_2_BE.service;
using Microsoft.AspNetCore.Mvc;

namespace Assignment_2_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestDTO request)
        {
            var token = _authService.Authenticate(request);

            if (token == null)
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            return Ok(new { token });
        }
    }
}
