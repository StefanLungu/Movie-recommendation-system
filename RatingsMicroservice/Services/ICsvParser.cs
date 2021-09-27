using RatingsMicroservice.Entities;
using System.Collections.Generic;

namespace RatingsMicroservice.Services
{
    public interface ICsvParser
    {
        List<Rating> GetRatings();
    }
}
