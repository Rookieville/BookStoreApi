using System.Reflection;
using bookapi_minimal.AppContext;
using bookapi_minimal.Exceptions;
using bookapi_minimal.Interfaces;
using bookapi_minimal.Services;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace bookapi_minimal.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddApplicationServices(this IHostApplicationBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (builder.Configuration == null) throw new ArgumentNullException(nameof(builder.Configuration));

            // Adding the database context
            builder.Services.AddDbContext<ApplicationContext>(configure =>
            {
                configure.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();

            // Add JWT Authentication
            builder.Services.AddJwtAuthentication(builder.Configuration);

            builder.Services.AddScoped<IBookService, BookService>();
            builder.Services.AddScoped<IAuthService, AuthService>();

            // Adding validators from the current assembly
            builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}