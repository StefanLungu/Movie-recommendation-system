using Microsoft.EntityFrameworkCore;
using MoviesMicroservice.Data;
using MoviesMicroservice.Entities;
using System;

namespace MoviesMicroservice.Tests
{
    public class ControllersFixture : IDisposable
    {
        public DataContext DataContext { get; private set; }

        private void SeedMovies()
        {
            DataContext.Movies.Add(new Movie { Id = 1, Title = "Tenet", ReleaseYear = 2020 });
            DataContext.Movies.Add(new Movie { Id = 2, Title = "Schindler's List", ReleaseYear = 1994 });
            DataContext.Movies.Add(new Movie { Id = 3, Title = "Titanic", ReleaseYear = 1997 });

        }

        private void SeedActors()
        {
            DataContext.Actors.Add(new Actor { Id = 1, Name = "Morgan Freeman", Age = 83 });
            DataContext.Actors.Add(new Actor { Id = 2, Name = "Leonardo DiCaprio", Age = 46 });
            DataContext.Actors.Add(new Actor { Id = 3, Name = "Robert De Niro", Age = 77 });
            DataContext.Actors.Add(new Actor { Id = 4, Name = "Robert Pattinson", Age = 34 });
        }

        private void SeedMovieGenres()
        {
            DataContext.MovieGenres.Add(new MovieGenre { Id = 1, Name = "comedy" });
            DataContext.MovieGenres.Add(new MovieGenre { Id = 2, Name = "romance" });
            DataContext.MovieGenres.Add(new MovieGenre { Id = 3, Name = "drama" });
            DataContext.MovieGenres.Add(new MovieGenre { Id = 4, Name = "horror" });
            DataContext.MovieGenres.Add(new MovieGenre { Id = 5, Name = "action" });
        }

        public ControllersFixture()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                    .UseInMemoryDatabase(databaseName: "MoviesDatabase")
                    .Options;

            DataContext = new DataContext(options);
            SeedMovies();
            SeedActors();
            SeedMovieGenres();
            DataContext.SaveChanges();
        }

        public void Dispose()
        {
            DataContext.Dispose();
        }
    }
}
