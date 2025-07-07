using bookapi_minimal.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bookapi_minimal.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UserModel>
    {
        public void Configure(EntityTypeBuilder<UserModel> builder)
        {
            // Primary key
            builder.HasKey(u => u.Id);

            // Email configuration
            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(255);

            // Create unique index on email
            builder.HasIndex(u => u.Email)
                .IsUnique()
                .HasDatabaseName("IX_Users_Email");

            // Password hash
            builder.Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(500);

            // First name
            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            // Last name
            builder.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(100);

            // Role
            builder.Property(u => u.Role)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("User");

            // Timestamps
            builder.Property(u => u.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(u => u.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            // Table name
            builder.ToTable("Users");
        }
    }
} 