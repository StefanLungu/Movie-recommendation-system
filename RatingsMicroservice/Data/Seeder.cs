using RatingsMicroservice.Entities;
using RatingsMicroservice.Services;
using System.Collections.Generic;

namespace RatingsMicroservice.Data
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

        public void SeedRatings()
        {
            List<Rating> ratings = _parser.GetRatings();
            foreach (Rating rating in ratings)
            {
                _context.Ratings.Add(rating);
            }
            _context.SaveChanges();
        }
    }
}
