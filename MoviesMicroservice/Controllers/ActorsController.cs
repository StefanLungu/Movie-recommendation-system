using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesMicroservice.Data;
using MoviesMicroservice.Entities;
using MoviesMicroservice.Helpers;
using MoviesMicroservice.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesMicroservice.Controllers
{
    [Route("api/v1/actors")]
    [ApiController]
    public class ActorsController : ControllerBase
    {
        private readonly DataContext _context;
        public ActorsController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Actor>>> GetActors()
        {
            return await _context.Actors.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Actor>> GetActor(int id)
        {
            Actor actor = await _context.Actors.FindAsync(id);

            if (actor == null)
            {
                return NotFound(Messages.NotFoundMessage("Actor", id));
            }

            return actor;
        }

        [HttpGet]
        [Route("movies/{movieId}")]
        public async Task<ActionResult<IEnumerable<Actor>>> GetActorsByMovie(int movieId)
        {
            if (MovieExists(movieId))
            {
                Movie movie = await _context.Movies.FindAsync(movieId);
                _context.Entry(movie).Collection("Actors").Load();
                return movie.Actors.ToList();
            }
            return NotFound(Messages.NotFoundMessage("Movie", movieId));
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActor(int id, Actor actor)
        {
            if (id != actor.Id)
            {
                return BadRequest();
            }

            _context.Entry(actor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActorExists(id))
                {
                    return NotFound(Messages.NotFoundMessage("Actor", id));
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
        public async Task<ActionResult<Actor>> PostActor(Actor actor)
        {
            _context.Actors.Add(actor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetActor", new { id = actor.Id }, actor);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Actor>> DeleteActor(int id)
        {
            Actor actor = await _context.Actors.FindAsync(id);
            if (actor == null)
            {
                return NotFound(Messages.NotFoundMessage("Actor", id));
            }

            _context.Actors.Remove(actor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [Authorize]
        [HttpPost]
        [Route("{actorId}/movies/{movieId}")]
        public async Task<IActionResult> AddActorToMovie(int actorId, int movieId)
        {
            if (ActorExists(actorId))
            {
                if (MovieExists(movieId))
                {
                    Actor actor = await _context.Actors.FindAsync(actorId);
                    Movie movie = _context.Movies.Include(m => m.Actors).FirstOrDefault(m => m.Id == movieId);

                    if (!movie.Actors.Contains(actor))
                    {
                        movie.Actors.Add(actor);
                        await _context.SaveChangesAsync();
                        return NoContent();
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                return NotFound(Messages.NotFoundMessage("Movie", movieId));
            }
            return NotFound(Messages.NotFoundMessage("Actor", actorId));
        }

        [Authorize]
        [HttpDelete]
        [Route("{actorId}/movies/{movieId}")]
        public async Task<IActionResult> RemoveActorFromMovie(int actorId, int movieId)
        {
            if (ActorExists(actorId))
            {
                if (MovieExists(movieId))
                {
                    Actor actor = await _context.Actors.FindAsync(actorId);
                    Movie movie = _context.Movies.Include(m => m.Actors).FirstOrDefault(m => m.Id == movieId);

                    if (movie.Actors.Contains(actor))
                    {
                        movie.Actors.Remove(actor);
                        await _context.SaveChangesAsync();
                        return NoContent();
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                return NotFound(Messages.NotFoundMessage("Movie", movieId));
            }
            return NotFound(Messages.NotFoundMessage("Actor", actorId));
        }

        private bool ActorExists(int id)
        {
            return _context.Actors.Any(e => e.Id == id);
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}
