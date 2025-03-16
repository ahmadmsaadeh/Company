using Company.Models;

namespace Company.Services
{
    public interface IAuthService
    {
        Task<string> RegisterUserAsync(User user);
        Task<string?> LoginUserAsync(string name, string password);
    }
}
