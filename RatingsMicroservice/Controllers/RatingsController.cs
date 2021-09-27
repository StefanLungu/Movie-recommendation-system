using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RatingsMicroservice.Data;
using RatingsMicroservice.Entities;
using RatingsMicroservice.Helpers;
using RatingsMicroservice.Models;
using RatingsMicroservice.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace RatingsMicroservice.Controllers
{
    [Route("api/v1/ratings")]
    [ApiController]
    public class RatingsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly HttpClient _client;
        public RatingsController(DataContext context)
        {
            _context = context;
            _client = new HttpClient();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rating>>> GetRatings()
        {
            return await _context.Ratings.ToListAsync();
        }

        [HttpGet("{id}")]
        public ActionResult<Rating> GetRating(int id)
        {
            Rating rating = _context.Ratings.FirstOrDefault(r => r.Id == id);

            if (rating == null)
            {
                return NotFound(Messages.NotFoundMessage("Rating", id));
            }

            return rating;
        }

        [RatingAuthorize]
        [HttpPost]
        public async Task<ActionResult<Rating>> CreateRating([FromBody] CreateRatingRequest ratingDto)
        {
            if (!MovieExists(ratingDto.MovieId).Result)
            {
                return BadRequest(Messages.NotFoundMessage("Movie", ratingDto.MovieId));
            }

            if (!UserExists(ratingDto.UserId).Result)
            {
                return BadRequest(Messages.NotFoundMessage("User", ratingDto.UserId));
            }

            if (!IsRatingValueValid(ratingDto.Value))
            {
                return BadRequest(Messages.InvalidRatingMessage);
            }

            Rating rating = await _context.Ratings
                .FirstOrDefaultAsync(r => r.MovieId == ratingDto.MovieId && r.UserId == ratingDto.UserId);

            if (rating != null)
            {
                return BadRequest(Messages.DuplicateRatingForMovie(ratingDto.UserId, ratingDto.MovieId));
            }

            Rating newRating = new Rating
            {
                UserId = ratingDto.UserId,
                MovieId = ratingDto.MovieId,
                Value = ratingDto.Value,
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now
            };

            _context.Ratings.Add(newRating);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRating", new { id = newRating.Id }, newRating);
        }

        [RatingAuthorize]
        [HttpPut]
        public async Task<IActionResult> UpdateRating([FromBody] UpdateRatingRequest ratingDto)
        {
            if (!MovieExists(ratingDto.MovieId).Result)
            {
                return BadRequest(Messages.NotFoundMessage("Movie", ratingDto.MovieId));
            }

            if (!UserExists(ratingDto.UserId).Result)
            {
                return BadRequest(Messages.NotFoundMessage("User", ratingDto.UserId));
            }

            if (!IsRatingValueValid(ratingDto.Value))
            {
                return BadRequest(Messages.InvalidRatingMessage);
            }

            Rating rating = await _context.Ratings
                .FirstOrDefaultAsync(r => r.MovieId == ratingDto.MovieId && r.UserId == ratingDto.UserId);

            if (rating == null)
            {
                return BadRequest(Messages.UserHasNoRatingForMovie(ratingDto.UserId, ratingDto.MovieId));
            }

            rating.Value = ratingDto.Value;
            rating.DateUpdated = DateTime.Now;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [RatingAuthorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteRating([FromBody] DeleteRatingRequest ratingDto)
        {
            if (!MovieExists(ratingDto.MovieId).Result)
            {
                return BadRequest(Messages.NotFoundMessage("Movie", ratingDto.MovieId));
            }

            if (!UserExists(ratingDto.UserId).Result)
            {
                return BadRequest(Messages.NotFoundMessage("User", ratingDto.UserId));
            }

            Rating rating = await _context.Ratings
                .FirstOrDefaultAsync(r => r.MovieId == ratingDto.MovieId && r.UserId == ratingDto.UserId);

            if (rating == null)
            {
                return BadRequest(Messages.UserHasNoRatingForMovie(ratingDto.UserId, ratingDto.MovieId));
            }

            _context.Ratings.Remove(rating);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet]
        [Route("users/{userId}")]
        public async Task<ActionResult<IEnumerable<Rating>>> GetRatingsByUserId(int userId)
        {
            if (!UserExists(userId).Result)
            {
                return NotFound(Messages.NotFoundMessage("User", userId));
            }

            return await _context.Ratings.Where(r => r.UserId == userId).ToListAsync();
        }

        [HttpGet]
        [Route("movies/{movieId}")]
        public async Task<ActionResult<IEnumerable<Rating>>> GetRatingsByMovie(int movieId)
        {
            if (!MovieExists(movieId).Result)
            {
                return NotFound(Messages.NotFoundMessage("Movie", movieId));
            }

            return await _context.Ratings.Where(r => r.MovieId == movieId).ToListAsync();
        }

        [HttpGet]
        [Route("movies/{movieId}/average")]
        public async Task<ActionResult<float>> GetAverageRatingForMovie(int movieId)
        {
            if (!MovieExists(movieId).Result)
            {
                return NotFound(Messages.NotFoundMessage("Movie", movieId));
            }

            float sum = 0;
            int count = 0;

            List<Rating> ratings = await _context.Ratings.Where(r => r.MovieId == movieId).ToListAsync();

            foreach (Rating rating in ratings)
            {
                sum += rating.Value;
                count += 1;
            }

            if (count == 0)
            {
                return BadRequest(Messages.MovieHasNoRatings);
            }

            AverageRatingResult averageRatingResponse = new AverageRatingResult
            {
                Value = sum / count
            };

            return Ok(averageRatingResponse);
        }

        private bool IsRatingValueValid(float value)
        {
            return value >= 1 && value <= 5;
        }

        private async Task<bool> UserExists(int id)
        {
            HttpResponseMessage response = await _client.GetAsync($"{Messages.BaseUrl}/{Messages.GetUsersUrl}/{id}");
            return response.IsSuccessStatusCode;
        }

        private async Task<bool> MovieExists(int id)
        {
            HttpResponseMessage response = await _client.GetAsync($"{Messages.BaseUrl}/{Messages.GetMoviesUrl}/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
