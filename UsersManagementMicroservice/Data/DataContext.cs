using Microsoft.EntityFrameworkCore;
using UsersManagementMicroservice.Entities;

namespace UsersManagementMicroservice.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetUserProperties(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        private void SetUserProperties(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u => u.Id)
                .HasColumnName("id");

            modelBuilder.Entity<User>()
                .Property(u => u.Username)
                .IsRequired()
                .HasColumnName("username");

            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired()
                .HasColumnName("email");

            modelBuilder.Entity<User>()
                .Property(u => u.Password)
                .IsRequired()
                .HasColumnName("password");

            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .IsRequired()
                .HasDefaultValue("user")
                .HasColumnName("role");
        }
    }
}
