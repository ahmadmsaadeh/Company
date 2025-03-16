using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Company.Models;
using Company.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Company.Services
{
    public class AuthService : IAuthService
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IGenericRepository<User> userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<string> RegisterUserAsync(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Password) || user.Password.Length < 6)
                throw new ArgumentException("Password must be at least 6 characters long");

            user.Password = Convert.ToBase64String(Encoding.UTF8.GetBytes(user.Password));

            await _userRepository.AddAsync(user);
            return "User registered successfully!";
        }

        public async Task<string?> LoginUserAsync(string name, string password)
        {
            var user = (await _userRepository.GetAllAsync())
                        .FirstOrDefault(u => u.Name == name);

            if (user == null) return null;

            var decodedPassword = Encoding.UTF8.GetString(Convert.FromBase64String(user.Password));
            if (decodedPassword != password) return null;

            return GenerateJwtToken(user);
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!);
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim("UserID", user.Id.ToString()),
                    new Claim(ClaimTypes.Role, "User")
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}