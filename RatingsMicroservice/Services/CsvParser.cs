using Microsoft.Extensions.Options;
using Microsoft.VisualBasic.FileIO;
using RatingsMicroservice.Entities;
using RatingsMicroservice.Helpers;
using System;
using System.Collections.Generic;

namespace RatingsMicroservice.Services
{
    public class CsvParser : ICsvParser
    {
        private List<Rating> Ratings { set; get; }

        private readonly AppSettings _appSettings;

        public CsvParser(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            Ratings = new List<Rating>();
            GenerateRatings();
        }

        private void GenerateRatings()
        {
            var parser = new TextFieldParser(_appSettings.RatingsCsvPath)
            {
                TextFieldType = FieldType.Delimited
            };
            parser.SetDelimiters(new string[] { "," });
            parser.ReadFields();
            int ratingId = 0;
            while (!parser.EndOfData)
            {
                string[] row = parser.ReadFields();
                int userId = int.Parse(row[0]);
                int movieId = int.Parse(row[1]);
                float ratingValue = float.Parse(row[2]);
                ratingId++;
                Rating rating = new Rating
                {
                    Id = ratingId,
                    MovieId = movieId,
                    UserId = userId,
                    Value = ratingValue,
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now
                };
                Ratings.Add(rating);
            }
        }

        public List<Rating> GetRatings()
        {
            return Ratings;
        }
    }
}
