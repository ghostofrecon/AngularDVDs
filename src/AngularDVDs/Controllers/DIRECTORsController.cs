using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AngularDVDs.EF;

namespace AngularDVDs.Controllers
{
    [Produces("application/json")]
    [Route("api/DIRECTORs")]
    public class DIRECTORsController : Controller
    {
        private readonly dvdContext _context;

        public DIRECTORsController(dvdContext context)
        {
            _context = context;
        }

        // GET: api/DIRECTORs
        [HttpGet]
        public IEnumerable<DIRECTOR> GetDIRECTOR()
        {
            return _context.DIRECTOR;
        }

        // GET: api/DIRECTORs/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDIRECTOR([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            DIRECTOR dIRECTOR = await _context.DIRECTOR.SingleOrDefaultAsync(m => m.DIRECTOR_ID == id);

            if (dIRECTOR == null)
            {
                return NotFound();
            }

            return Ok(dIRECTOR);
        }

        // PUT: api/DIRECTORs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDIRECTOR([FromRoute] Guid id, [FromBody] DIRECTOR dIRECTOR)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != dIRECTOR.DIRECTOR_ID)
            {
                return BadRequest();
            }

            _context.Entry(dIRECTOR).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DIRECTORExists(id))
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

        // POST: api/DIRECTORs
        [HttpPost]
        public async Task<IActionResult> PostDIRECTOR([FromBody] DIRECTOR dIRECTOR)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            dIRECTOR.DIRECTOR_ID = Guid.NewGuid();
            dIRECTOR.DIRECTOR_ADDMOD_Datetime = DateTime.Now;
            _context.DIRECTOR.Add(dIRECTOR);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (DIRECTORExists(dIRECTOR.DIRECTOR_ID))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetDIRECTOR", new { id = dIRECTOR.DIRECTOR_ID }, dIRECTOR);
        }

        // DELETE: api/DIRECTORs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDIRECTOR([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            DIRECTOR dIRECTOR = await _context.DIRECTOR.SingleOrDefaultAsync(m => m.DIRECTOR_ID == id);
            if (dIRECTOR == null)
            {
                return NotFound();
            }

            _context.DIRECTOR.Remove(dIRECTOR);
            await _context.SaveChangesAsync();

            return Ok(dIRECTOR);
        }

        private bool DIRECTORExists(Guid id)
        {
            return _context.DIRECTOR.Any(e => e.DIRECTOR_ID == id);
        }
    }
}