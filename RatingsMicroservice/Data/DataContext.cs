using Microsoft.EntityFrameworkCore;
using RatingsMicroservice.Entities;

namespace RatingsMicroservice.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Rating> Ratings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetRatingProperties(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        private void SetRatingProperties(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rating>()
                .Property(r => r.Id)
                .HasColumnName("id");

            modelBuilder.Entity<Rating>()
                .Property(r => r.Value)
                .IsRequired()
                .HasColumnName("value");

            modelBuilder.Entity<Rating>()
                .Property(r => r.DateCreated)
                .IsRequired()
                .HasColumnName("date_created");

            modelBuilder.Entity<Rating>()
                .Property(r => r.DateUpdated)
                .IsRequired()
                .HasColumnName("date_updated");
        }
    }
}
