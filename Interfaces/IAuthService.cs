using bookapi_minimal.Contracts;

namespace bookapi_minimal.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse?> LoginAsync(LoginRequest loginRequest);
        Task<AuthResponse?> RegisterAsync(RegisterRequest registerRequest);
        Task<bool> UserExistsAsync(string email);
    }
} 