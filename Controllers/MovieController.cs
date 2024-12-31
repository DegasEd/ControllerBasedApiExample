using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory.Storage.Internal;

namespace ControllerBasedApiSwagger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly MovieDb _db;

        public MovieController(MovieDb db)
        {
            _db = db;
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Movie>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Get()
        {
            var movies = await _db.Movies.ToListAsync();

            return Ok (movies);
        }

        [HttpGet("/api/[controller]/active")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Movie>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetActive()
        {
            var activeMovies = await _db.Movies.Where(x => x.IsActive).ToListAsync();

            return Ok(activeMovies);
        }

        [HttpGet("/api/[controller]/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Movie))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            return await _db.Movies.FindAsync(id)
            is Movie movie
            ? Ok(movie)
            : NotFound();
        }

        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Movie))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] Movie movie)
        {
            await _db.Movies.AddAsync(movie);
            await _db.SaveChangesAsync();

            return Ok(movie);
        }

        [HttpPut("/api/[controller]/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Movie))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Put([FromRoute]int id, [FromBody]Movie putMovie)
        {
            var movie = await _db.Movies.FindAsync(id);
            
            if (movie is null) return NotFound();
            
            movie.Name = putMovie.Name;
            movie.Gender = putMovie.Gender; 
            movie.IsActive = putMovie.IsActive;
            
            await _db.SaveChangesAsync();
            
            return movie.IsActive  
                ? Ok(movie)  
                : NoContent();
        }

        [HttpDelete("/api/[controller]/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (await _db.Movies.FindAsync(id) is Movie movie)
            {
                _db.Movies.Remove(movie);
                await _db.SaveChangesAsync();
                return NoContent();
            }
            return NotFound();
        }
    }
}
