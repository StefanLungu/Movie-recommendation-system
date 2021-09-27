using Bogus;
using MoviesMicroservice.Entities;
using MoviesMicroservice.Resources;
using MoviesMicroservice.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MoviesMicroservice.Data
{
    public class Seeder
    {
        private readonly DataContext _context;
        private readonly ICsvParser _parser;
        public Seeder(DataContext context, ICsvParser parser)
        {
            _context = context;
            _parser = parser;
        }

        public void SeedMovies()
        {
            List<Movie> movies = _parser.GetMovies();
            foreach (Movie movie in movies)
            {
                if (!_context.Movies.Any(m => m.Id == movie.Id))
                {
                    _context.Movies.Add(movie);
                }
            }
            _context.SaveChanges();
        }

        public void SeedActors()
        {
            List<Movie> movies = _context.Movies.ToList();
            for (int i = 0; i < Messages.NumberOfActors; ++i)
            {
                Actor actor = GenerateFakeActor();
                _context.Actors.Add(actor);
                movies = movies.OrderBy(m => Guid.NewGuid()).ToList();
                for (int j = 0; j < Messages.NumberOfMoviesPerActor; ++j)
                {
                    actor.Movies.Add(movies[j]);
                }
            }
            _context.SaveChanges();
        }

        private Actor GenerateFakeActor()
        {
            var faker = new Faker();
            return new Actor
            {
                Name = $"{faker.Name.FirstName()} {faker.Name.LastName()}",
                Age = faker.Random.Number(Messages.MinActorAge, Messages.MaxActorAge)
            };
        }
    }
}
