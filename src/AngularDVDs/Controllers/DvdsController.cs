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
    using System.Linq.Expressions;

    using AngularDVDs.ef;

    [Produces("application/json")]
    [Route("api/Dvds")]
    public class DvdsController : Controller
    {
        private readonly dvdContext _context;

        public DvdsController(dvdContext context)
        {
            _context = context;
        }

        private IEnumerable<object> preparedDVD(List<DVD> dvd)
        {
            return dvd.Select(x => new
            {
                x.DVD_ID,
                x.DVD_TITLE,
                x.DVD_DIRECTOR_ID,
                x.DVD_GENRE_ID,
                x.DVD_RELEASE_YEAR,
                x.DVD_ADDMOD_Datetime,
                this._context.DIRECTOR.Single(y => y.DIRECTOR_ID == x.DVD_DIRECTOR_ID).DIRECTOR_NAME,
                this._context.GENRE.Single(y => y.GENRE_ID == x.DVD_GENRE_ID).GENRE_NAME

            }).ToList();
        }
        // GET: api/Dvds
        [HttpGet]
        public IEnumerable<object> GetDVD()
        {
            if (_context.DVD.Any())
            {
                var result = this._context.DVD.ToList();

                return this.preparedDVD(result);
            }
            return null;
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
            List<DVD> dvdList = new List<DVD>();
            dvdList.Add(dVD);
            return Ok(this.preparedDVD(dvdList).Single());
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
            dVD.DVD_ID = Guid.NewGuid();
            dVD.DVD_ADDMOD_Datetime = DateTime.Now;
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