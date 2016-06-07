using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AngularDVDs.ef;

namespace AngularDVDs.Controllers
{
    [Produces("application/json")]
    [Route("api/Genres")]
    public class GenresController : Controller
    {
        private readonly dvdContext _context;

        public GenresController(dvdContext context)
        {
            _context = context;
        }

        // GET: api/Genres
        [HttpGet]
        public IEnumerable<GENRE> GetGENRE()
        {
            return _context.GENRE;
        }

        // GET: api/Genres/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGENRE([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            GENRE gENRE = await _context.GENRE.SingleOrDefaultAsync(m => m.GENRE_ID == id);

            if (gENRE == null)
            {
                return NotFound();
            }

            return Ok(gENRE);
        }

        // PUT: api/Genres/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGENRE([FromRoute] Guid id, [FromBody] GENRE gENRE)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != gENRE.GENRE_ID)
            {
                return BadRequest();
            }

            _context.Entry(gENRE).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GENREExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Genres
        [HttpPost]
        public async Task<IActionResult> PostGENRE([FromBody] GENRE gENRE)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            gENRE.GENRE_ID = Guid.NewGuid();
            gENRE.GENRE_ADDMOD_Datetime = DateTime.Now;
            _context.GENRE.Add(gENRE);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                var exception = ex;
                if (GENREExists(gENRE.GENRE_ID))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetGENRE", new { id = gENRE.GENRE_ID }, gENRE);
        }

        // DELETE: api/Genres/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGENRE([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            GENRE gENRE = await _context.GENRE.SingleOrDefaultAsync(m => m.GENRE_ID == id);
            if (gENRE == null)
            {
                return NotFound();
            }

            _context.GENRE.Remove(gENRE);
            await _context.SaveChangesAsync();

            return Ok(gENRE);
        }

        private bool GENREExists(Guid id)
        {
            return _context.GENRE.Any(e => e.GENRE_ID == id);
        }
    }
}