using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesMicroservice.Data;
using MoviesMicroservice.Entities;
using MoviesMicroservice.Helpers;
using MoviesMicroservice.Models;
using MoviesMicroservice.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesMicroservice.Controllers
{
    [Route("api/v1/movies")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly DataContext _context;

        public MoviesController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies([FromQuery] PaginationDTO pagination)
        {
            var queryable = _context.Movies.Include(m => m.Genres).AsQueryable();
            if (!string.IsNullOrEmpty(pagination.Title))
            {
                queryable = queryable.Where(m => m.Title.ToLower().Contains(pagination.Title.ToLower()));
            }

            if (pagination.IdGenre != 0)
            {
                MovieGenre genre = await _context.MovieGenres.FindAsync(pagination.IdGenre);
                queryable = queryable.Where(m => m.Genres.Contains(genre));
            }

            if (!string.IsNullOrEmpty(pagination.SortByColumnName))
            {
                switch (pagination.SortByColumnName)
                {
                    case "Title":
                        if (pagination.SortDirection == "asc")
                        {
                            queryable = queryable.OrderBy(m => m.Title);
                        }
                        else
                        {
                            queryable = queryable.OrderByDescending(m => m.Title);
                        }
                        break;
                    case "ReleaseYear":
                        if (pagination.SortDirection == "asc")
                        {
                            queryable = queryable.OrderBy(m => m.ReleaseYear);
                        }
                        else
                        {
                            queryable = queryable.OrderByDescending(m => m.ReleaseYear);
                        }
                        break;
                    default:
                        break;
                }
            }

            await HttpContext.InsertPaginationParameterInResponse(queryable, pagination.EntitiesPerPage);
            return await queryable.Paginate(pagination).ToListAsync();
        }

        [HttpGet("{id}")]
        public ActionResult<Movie> GetMovie(int id)
        {
            Movie movie = _context.Movies.Include(m => m.Genres).FirstOrDefault(m => m.Id == id);

            if (movie == null)
            {
                return NotFound(Messages.NotFoundMessage("Movie", id));
            }

            return movie;
        }

        [HttpGet]
        [Route("actors/{id}")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetAllMoviesByActorId(int id)
        {
            if (!ActorExists(id))
            {
                return NotFound(Messages.NotFoundMessage("Actor", id));
            }

            Actor actor = await _context.Actors.FindAsync(id);

            return await _context.Movies.Where(m => m.Actors.Contains(actor)).Include(m => m.Genres).ToListAsync();
        }

        [HttpGet]
        [Route("genres/{id}")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetAllMoviesByMovieGenreId(int id)
        {
            if (!MovieGenreExists(id))
            {
                return NotFound(Messages.NotFoundMessage("MovieGenre", id));
            }

            MovieGenre genre = await _context.MovieGenres.FindAsync(id);

            return await _context.Movies.Where(m => m.Genres.Contains(genre)).Include(m => m.Genres).ToListAsync();
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovie(int id, Movie movie)
        {
            if (id != movie.Id)
            {
                return BadRequest();
            }

            _context.Entry(movie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
                {
                    return NotFound(Messages.NotFoundMessage("Movie", id));
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
        public async Task<ActionResult<Movie>> CreateMovie(Movie movie)
        {
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetById", new { id = movie.Id }, movie);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Movie>> DeleteMovie(int id)
        {
            Movie movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound(Messages.NotFoundMessage("Movie", id));
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(m => m.Id == id);
        }

        private bool ActorExists(int id)
        {
            return _context.Actors.Any(a => a.Id == id);
        }

        private bool MovieGenreExists(int id)
        {
            return _context.MovieGenres.Any(m => m.Id == id);
        }
    }
}
