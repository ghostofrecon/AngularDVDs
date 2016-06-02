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
    [Route("api/Dvds")]
    public class DvdsController : Controller
    {
        private readonly dvdContext _context;

        public DvdsController(dvdContext context)
        {
            _context = context;
        }

        // GET: api/Dvds
        [HttpGet]
        public IEnumerable<DVD> GetDVD()
        {
            return _context.DVD;
        }

        // GET: api/Dvds/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDVD([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            DVD dVD = await _context.DVD.SingleOrDefaultAsync(m => m.DVD_ID == id);

            if (dVD == null)
            {
                return NotFound();
            }

            return Ok(dVD);
        }

        // PUT: api/Dvds/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDVD([FromRoute] Guid id, [FromBody] DVD dVD)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != dVD.DVD_ID)
            {
                return BadRequest();
            }

            _context.Entry(dVD).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DVDExists(id))
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

        // POST: api/Dvds
        [HttpPost]
        public async Task<IActionResult> PostDVD([FromBody] DVD dVD)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.DVD.Add(dVD);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (DVDExists(dVD.DVD_ID))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetDVD", new { id = dVD.DVD_ID }, dVD);
        }

        // DELETE: api/Dvds/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDVD([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            DVD dVD = await _context.DVD.SingleOrDefaultAsync(m => m.DVD_ID == id);
            if (dVD == null)
            {
                return NotFound();
            }

            _context.DVD.Remove(dVD);
            await _context.SaveChangesAsync();

            return Ok(dVD);
        }

        private bool DVDExists(Guid id)
        {
            return _context.DVD.Any(e => e.DVD_ID == id);
        }
    }
}