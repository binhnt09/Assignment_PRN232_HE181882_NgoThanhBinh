using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace RV_Assignment_1.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        public AuthController(IConfiguration config) => _config = config;

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var adminEmail = _config["AdminAccount:Email"];
            var adminPassword = _config["AdminAccount:Password"];

            if (request.Email == adminEmail && request.Password == adminPassword)
            {
                return Ok(new { Message = "Đăng nhập thành công Admin!", Role = "Admin" });
            }
            return Unauthorized(new { Message = "Sai tài khoản hoặc mật khẩu!" });
        }
    }
}
