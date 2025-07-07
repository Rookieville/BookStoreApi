using bookapi_minimal.Models;
using Microsoft.EntityFrameworkCore;

namespace bookapi_minimal.AppContext
{
    public class ApplicationContext : DbContext
    {
        // Default schema for the database context
        private const string DefaultSchema = "public"; // PostgreSQL typically uses 'public' schema

        // DbSet to represent the collection of books in our database
        public DbSet<BookModel> Books { get; set; }
        
        // DbSet to represent the collection of users in our database
        public DbSet<UserModel> Users { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // For PostgreSQL, we might want to handle schema differently
            if (!string.IsNullOrEmpty(DefaultSchema))
            {
                modelBuilder.HasDefaultSchema(DefaultSchema);
            }

            // Remove duplicate call
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
        }
    }
}