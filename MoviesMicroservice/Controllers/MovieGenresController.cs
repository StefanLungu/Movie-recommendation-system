using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesMicroservice.Data;
using MoviesMicroservice.Entities;
using MoviesMicroservice.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesMicroservice.Controllers
{
    [Route("api/v1/genres")]
    [ApiController]
    public class MovieGenresController : ControllerBase
    {
        private readonly DataContext _context;

        public MovieGenresController(DataContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieGenre>>> GetAll()
        {
            return await _context.MovieGenres.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MovieGenre>> GetGenreById(int id)
        {
            MovieGenre movieGenre = await _context.MovieGenres.FindAsync(id);

            if (movieGenre == null)
            {
                return NotFound(Resources.Messages.NotFoundMessage("MovieGenre", id));
            }

            return movieGenre;
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, MovieGenre movieGenre)
        {
            if (id != movieGenre.Id)
            {
                return BadRequest();
            }

            _context.Entry(movieGenre).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieGenreExists(id))
                {
                    return NotFound(Resources.Messages.NotFoundMessage("MovieGenre", id));
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<MovieGenre>> Create(MovieGenre movieGenre)
        {
            _context.MovieGenres.Add(movieGenre);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGenreById", new { id = movieGenre.Id }, movieGenre);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<MovieGenre>> Delete(int id)
        {
            MovieGenre movieGenre = await _context.MovieGenres.FindAsync(id);

            if (movieGenre == null)
            {
                return NotFound(Resources.Messages.NotFoundMessage("MovieGenre", id));
            }

            _context.MovieGenres.Remove(movieGenre);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [Authorize]
        [HttpPost]
        [Route("{genreId}/movies/{movieId}")]
        public async Task<IActionResult> AddGenreToMovie(int genreId, int movieId)
        {
            if (MovieGenreExists(genreId) && MovieExists(movieId))
            {
                MovieGenre genre = await _context.MovieGenres.FindAsync(genreId);
                Movie movie = _context.Movies.Include(m => m.Genres).FirstOrDefault(m => m.Id == movieId);

                if (!movie.Genres.Contains(genre))
                {
                    movie.Genres.Add(genre);
                    await _context.SaveChangesAsync();
                    return NoContent();
                }
                else
                {
                    return BadRequest();
                }
            }
            return NotFound();
        }

        [Authorize]
        [HttpDelete]
        [Route("{genreId}/movies/{movieId}")]
        public async Task<IActionResult> RemoveGenreFromMovie(int genreId, int movieId)
        {
            if (MovieGenreExists(genreId))
            {
                if (MovieExists(movieId))
                {
                    MovieGenre genre = await _context.MovieGenres.FindAsync(genreId);
                    Movie movie = _context.Movies.Include(m => m.Genres).FirstOrDefault(m => m.Id == movieId);

                    if (movie.Genres.Contains(genre))
                    {
                        movie.Genres.Remove(genre);
                        await _context.SaveChangesAsync();
                        return NoContent();
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                return NotFound(Resources.Messages.NotFoundMessage("Movie", movieId));
            }
            return NotFound(Resources.Messages.NotFoundMessage("MovieGenre", genreId));
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(m => m.Id == id);
        }

        private bool MovieGenreExists(int id)
        {
            return _context.MovieGenres.Any(m => m.Id == id);
        }
    }
}
