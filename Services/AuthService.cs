using System.Security.Cryptography;
using System.Text;
using bookapi_minimal.AppContext;
using bookapi_minimal.Contracts;
using bookapi_minimal.Extensions;
using bookapi_minimal.Interfaces;
using bookapi_minimal.Models;
using Microsoft.EntityFrameworkCore;

namespace bookapi_minimal.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationContext _context;
        private readonly IJwtService _jwtService;
        private readonly IConfiguration _configuration;

        public AuthService(ApplicationContext context, IJwtService jwtService, IConfiguration configuration)
        {
            _context = context;
            _jwtService = jwtService;
            _configuration = configuration;
        }

        public async Task<AuthResponse?> LoginAsync(LoginRequest loginRequest)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == loginRequest.Email.ToLower());

            if (user == null || !VerifyPassword(loginRequest.Password, user.PasswordHash))
            {
                return null;
            }

            var token = _jwtService.GenerateToken(
                user.Id.ToString(),
                user.Email,
                user.Role,
                new Dictionary<string, string>
                {
                    { "firstName", user.FirstName },
                    { "lastName", user.LastName }
                }
            );

            var expireMinutes = int.Parse(_configuration.GetSection("JwtSettings")["ExpireMinutes"] ?? "60");

            return new AuthResponse
            {
                Token = token,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role,
                ExpiresAt = DateTime.UtcNow.AddMinutes(expireMinutes)
            };
        }

        public async Task<AuthResponse?> RegisterAsync(RegisterRequest registerRequest)
        {
            if (await UserExistsAsync(registerRequest.Email))
            {
                return null; // User already exists
            }

            var user = new UserModel
            {
                Id = Guid.NewGuid(),
                Email = registerRequest.Email.ToLower(),
                PasswordHash = HashPassword(registerRequest.Password),
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                Role = registerRequest.Role,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var token = _jwtService.GenerateToken(
                user.Id.ToString(),
                user.Email,
                user.Role,
                new Dictionary<string, string>
                {
                    { "firstName", user.FirstName },
                    { "lastName", user.LastName }
                }
            );

            var expireMinutes = int.Parse(_configuration.GetSection("JwtSettings")["ExpireMinutes"] ?? "60");

            return new AuthResponse
            {
                Token = token,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role,
                ExpiresAt = DateTime.UtcNow.AddMinutes(expireMinutes)
            };
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            return await _context.Users
                .AnyAsync(u => u.Email.ToLower() == email.ToLower());
        }

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private static bool VerifyPassword(string password, string hashedPassword)
        {
            var hashedInput = HashPassword(password);
            return hashedInput == hashedPassword;
        }
    }
} 