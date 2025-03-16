using Company.DTOs;
using Company.Models;
using Company.Services;
using Microsoft.AspNetCore.Mvc;

namespace Company.Controllers
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

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO userDto)
        {
            if (userDto == null || string.IsNullOrEmpty(userDto.Name) || string.IsNullOrEmpty(userDto.Password))
                return BadRequest("Invalid user data");

            var newUser = new User
            {
                Name = userDto.Name,
                Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
                PermissionID = userDto.PermissionID
            };

            var result = await _authService.RegisterUserAsync(newUser);
            return Ok(new { message = result });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO loginDto)
        {
            var token = await _authService.LoginUserAsync(loginDto.Name, loginDto.Password);

            if (token == null) return Unauthorized(new { message = "Invalid credentials" });

            return Ok(new { token });
        }
    }
}
