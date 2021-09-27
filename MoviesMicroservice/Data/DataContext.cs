using Microsoft.EntityFrameworkCore;
using MoviesMicroservice.Entities;

namespace MoviesMicroservice.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<MovieGenre> MovieGenres { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetMovieProperties(modelBuilder);
            SetActorProperties(modelBuilder);
            SetMovieGenreProperties(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void SetMovieProperties(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>()
                .Property(m => m.Id)
                .HasColumnName("id");

            modelBuilder.Entity<Movie>()
                .Property(m => m.Title)
                .HasMaxLength(100)
                .IsRequired()
                .HasColumnName("title");

            modelBuilder.Entity<Movie>()
                .Property(m => m.ReleaseYear)
                .IsRequired()
                .HasColumnName("release_year");
        }

        private void SetActorProperties(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Actor>()
                .Property(a => a.Id)
                .HasColumnName("id");

            modelBuilder.Entity<Actor>()
                .Property(a => a.Name)
                .HasMaxLength(100)
                .IsRequired()
                .HasColumnName("name");

            modelBuilder.Entity<Actor>()
                .Property(a => a.Age)
                .IsRequired()
                .HasColumnName("age");
        }

        private void SetMovieGenreProperties(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MovieGenre>()
                .Property(m => m.Id)
                .HasColumnName("id");

            modelBuilder.Entity<MovieGenre>()
                .Property(m => m.Name)
                .HasMaxLength(30)
                .IsRequired()
                .HasColumnName("name");
        }
    }
}
