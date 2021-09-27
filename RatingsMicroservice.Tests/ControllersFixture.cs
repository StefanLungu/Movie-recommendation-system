using Microsoft.EntityFrameworkCore;
using RatingsMicroservice.Data;
using RatingsMicroservice.Entities;
using System;

namespace RatingsMicroservice.Tests
{
    public class ControllersFixture
    {
        public DataContext DataContext { get; private set; }

        private void SeedRatings()
        {
            DataContext.Ratings.Add(new Rating { Id = 1, Value = 4, MovieId = 1, UserId = 1, DateCreated = DateTime.Now, DateUpdated = new DateTime(2020, 12, 31, 4, 15, 10) });
            DataContext.Ratings.Add(new Rating { Id = 2, Value = 4.5f, MovieId = 2, UserId = 1, DateCreated = new DateTime(2020, 12, 30, 4, 15, 10), DateUpdated = new DateTime(2020, 12, 31, 4, 15, 10) });
            DataContext.Ratings.Add(new Rating { Id = 3, Value = 3, MovieId = 1, UserId = 2, DateCreated = new DateTime(2020, 12, 30, 4, 20, 00), DateUpdated = new DateTime(2020, 12, 31, 4, 15, 10) });
            DataContext.Ratings.Add(new Rating { Id = 4, Value = 3.5f, MovieId = 2, UserId = 2, DateCreated = new DateTime(2020, 12, 30, 4, 25, 20), DateUpdated = new DateTime(2020, 12, 31, 4, 15, 10) });
            DataContext.Ratings.Add(new Rating { Id = 5, Value = 2, MovieId = 3, UserId = 1, DateCreated = new DateTime(2020, 12, 30, 4, 35, 10), DateUpdated = new DateTime(2020, 12, 31, 4, 15, 10) });
            DataContext.Ratings.Add(new Rating { Id = 6, Value = 4.5f, MovieId = 2, UserId = 3, DateCreated = new DateTime(2020, 12, 30, 4, 35, 10), DateUpdated = new DateTime(2020, 12, 31, 4, 15, 10) });
        }

        public ControllersFixture()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                    .UseInMemoryDatabase(databaseName: "MoviesDatabase")
                    .Options;

            DataContext = new DataContext(options);
            SeedRatings();
            DataContext.SaveChanges();
        }

        public void Dispose()
        {
            DataContext.Dispose();
        }
    }
}
