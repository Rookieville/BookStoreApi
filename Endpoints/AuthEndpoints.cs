using bookapi_minimal.Contracts;
using bookapi_minimal.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace bookapi_minimal.Endpoints
{
    public static class AuthEndpoints
    {
        public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
        {
            // Login endpoint
            app.MapPost("/auth/login", async ([FromBody] LoginRequest loginRequest, IAuthService authService) =>
            {
                var result = await authService.LoginAsync(loginRequest);
                
                if (result == null)
                {
                    return Results.BadRequest(new ErrorResponse 
                    { 
                        Message = "Invalid email or password" 
                    });
                }

                return Results.Ok(new ApiResponse<AuthResponse>(result, "Login successful"));
            })
            .WithName("Login")
            .WithSummary("User login")
            .WithDescription("Authenticate user and return JWT token")
            .Produces<ApiResponse<AuthResponse>>(200)
            .Produces<ErrorResponse>(400);

            // Register endpoint
            app.MapPost("/auth/register", async ([FromBody] RegisterRequest registerRequest, IAuthService authService) =>
            {
                var result = await authService.RegisterAsync(registerRequest);
                
                if (result == null)
                {
                    return Results.BadRequest(new ErrorResponse 
                    { 
                        Message = "User with this email already exists" 
                    });
                }

                return Results.Created("/auth/login", new ApiResponse<AuthResponse>(result, "Registration successful"));
            })
            .WithName("Register")
            .WithSummary("User registration")
            .WithDescription("Register a new user and return JWT token")
            .Produces<ApiResponse<AuthResponse>>(201)
            .Produces<ErrorResponse>(400);

            // Test protected endpoint
            app.MapGet("/auth/profile", (HttpContext context) =>
            {
                var user = context.User;
                var userId = user.FindFirst("userId")?.Value;
                var email = user.FindFirst("email")?.Value;
                var role = user.FindFirst("role")?.Value;
                var firstName = user.FindFirst("firstName")?.Value;
                var lastName = user.FindFirst("lastName")?.Value;

                return Results.Ok(new
                {
                    UserId = userId,
                    Email = email,
                    Role = role,
                    FirstName = firstName,
                    LastName = lastName,
                    Claims = user.Claims.Select(c => new { c.Type, c.Value }).ToList()
                });
            })
            .RequireAuthorization()
            .WithName("GetProfile")
            .WithSummary("Get user profile")
            .WithDescription("Get current user's profile information from JWT claims");

            return app;
        }
    }
} 