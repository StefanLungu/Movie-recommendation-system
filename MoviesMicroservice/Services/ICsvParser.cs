using MoviesMicroservice.Entities;
using System.Collections.Generic;

namespace MoviesMicroservice.Services
{
    public interface ICsvParser
    {
        List<Movie> GetMovies();
    }
}
