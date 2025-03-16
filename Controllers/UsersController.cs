using AutoMapper;
using Company.DTOs;
using Company.Models;
using Company.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Company.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UsersController(IUserService userService, IMapper mappere)
        {
            _userService = userService;
            _mapper = mappere;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            var userDTOs = _mapper.Map<UserDTO>(users);
            return users.Any() ? Ok(userDTOs) : NotFound();
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();

            var userDto = _mapper.Map<UserDTO>(user);

            return Ok(userDto);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<UserDTO>> CreateUser([FromBody] UserRegisterDTO userDto)
        {
            if (userDto == null) return BadRequest("User data is required.");

            var newUser = new User
            {
                Name = userDto.Name,
                Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password), // Hash Password
                PermissionID = userDto.PermissionID
            };

            await _userService.CreateUserAsync(newUser);

            var userResponse = _mapper.Map<UserDTO>(userDto);

            return CreatedAtAction(nameof(GetUser), new { id = newUser.Id }, userResponse);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserRegisterDTO updatedUser)
        {
            var existingUser = await _userService.GetUserByIdAsync(id);
            if (existingUser == null) return NotFound();

            existingUser.Name = updatedUser.Name;
            existingUser.Password = BCrypt.Net.BCrypt.HashPassword(updatedUser.Password); // Hash Password
            existingUser.PermissionID = updatedUser.PermissionID;

            await _userService.UpdateUserAsync(existingUser);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            return await _userService.DeleteUserAsync(id) ? NoContent() : NotFound();
        }
    }
}
