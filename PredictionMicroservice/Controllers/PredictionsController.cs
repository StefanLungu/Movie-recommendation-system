using Microsoft.AspNetCore.Mvc;
using PredictionMicroservice.DataModels;
using PredictionMicroservice.Helpers;
using PredictionMicroservice.Models;
using PredictionMicroservice.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace PredictionMicroservice.Controllers
{
    [Route("api/v1/recommendations")]
    [ApiController]
    public class PredictionsController : ControllerBase
    {
        private readonly HttpClient _client;
        public PredictionsController()
        {
            _client = new HttpClient();
        }

        [RecommendationAuthorize]
        [HttpGet("{userId}")]
        public async Task<ActionResult<List<GetMovie>>> GetRecommendedMoviesAsync(int userId)
        {
            if (!UserExists(userId).Result)
            {
                return NotFound(Messages.NotFoundMessage("User", userId));
            }

            IEnumerable<GetMovie> movies = await GetMoviesWithoutRatingFromUser(userId);

            IDictionary<GetMovie, float> predictedRatings = new Dictionary<GetMovie, float>();
            foreach (GetMovie movie in movies)
            {
                if (predictedRatings.ContainsKey(movie))
                {
                    continue;
                }

                MovieData movieData = new MovieData()
                {
                    UserId = userId,
                    MovieId = movie.Id
                };

                MoviePrediction prediction = ConsumeModel.Predict(movieData);
                predictedRatings.Add(movie, prediction.Score);
            }

            var sortedPredictedValues = predictedRatings.OrderByDescending(key => key.Value);

            List<GetMovie> recommendedMovies = new List<GetMovie>();
            for (int i = 0; i < 5 && i < sortedPredictedValues.Count(); ++i)
            {
                int movieId = sortedPredictedValues.ElementAt(i).Key.Id;
                GetMovie movie = movies.Where(m => m.Id == movieId).FirstOrDefault();
                recommendedMovies.Add(movie);
            }

            return recommendedMovies;
        }

        private async Task<bool> UserExists(int id)
        {
            HttpResponseMessage response = await _client.GetAsync($"{Messages.BaseUrl}/{Messages.GetUsersUrl}/{id}");
            return response.IsSuccessStatusCode;
        }

        private async Task<List<GetMovie>> GetMoviesRatedByUserAsync(int userId)
        {
            List<GetRating> ratings = await GetRatingsByUserId(userId);
            List<GetMovie> movies = new List<GetMovie>();
            foreach (GetRating rating in ratings)
            {
                movies.Add(await GetMovieById(rating.MovieId));
            }
            return movies;
        }

        private async Task<List<GetMovie>> GetMoviesWithoutRatingFromUser(int userId)
        {
            List<GetMovie> movies = await GetMovies();
            List<GetMovie> moviesToExcept = await GetMoviesRatedByUserAsync(userId);
            foreach (GetMovie movie in moviesToExcept)
            {
                movies.Remove(movie);
            }

            return movies;
        }

        private async Task<GetMovie> GetMovieById(int movieId)
        {
            HttpResponseMessage response = await _client.GetAsync($"{Messages.BaseUrl}/{Messages.GetMoviesUrl}/{movieId}");
            string responseString = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<GetMovie>(responseString,
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        private async Task<List<GetRating>> GetRatingsByUserId(int userId)
        {
            HttpResponseMessage response = await _client.GetAsync($"{Messages.BaseUrl}/{Messages.GetRatingsUrl}/{Messages.GetUsersUrl}/{userId}");
            string responseString = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<GetRating>>(responseString,
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        private async Task<List<GetMovie>> GetMovies()
        {
            HttpResponseMessage response = await _client.GetAsync($"{Messages.BaseUrl}/{Messages.GetMoviesUrl}?{Messages.EntitiesPerPage}");
            string responseString = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<GetMovie>>(responseString,
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }
    }
}