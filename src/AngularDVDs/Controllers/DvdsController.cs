using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AngularDVDs.EF;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AngularDVDs.Controllers
{
    using System.Linq.Expressions;

    using AngularDVDs.EF;

    [Produces("application/json")]
    [Route("api/Dvds")]
    public class DvdsController : Controller
    {
        private readonly dvdContext _context;

        public DvdsController(dvdContext context)
        {
            this._context = context;
        }

        private IEnumerable<object> preparedDVD(List<DVD> dvd)
        {
            return
                dvd.Select(
                    x =>
                    new
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
            if (this._context.DVD.Any())
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
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            DVD dVD = await this._context.DVD.SingleOrDefaultAsync(m => m.DVD_ID == id);

            if (dVD == null)
            {
                return this.NotFound();
            }

            List<DVD> dvdList = new List<DVD>();
            dvdList.Add(dVD);
            return this.Ok(this.preparedDVD(dvdList).Single());
        }

        // PUT: api/Dvds/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDVD([FromRoute] Guid id, [FromBody] DVD dVD)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            if (id != dVD.DVD_ID)
            {
                return this.BadRequest();
            }

            this._context.Entry(dVD).State = EntityState.Modified;

            try
            {
                await this._context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!this.DVDExists(id))
                {
                    return this.NotFound();
                }
                else
                {
                    throw;
                }
            }

            return this.NoContent();
        }

        // POST: api/Dvds
        [HttpPost]
        public async Task<IActionResult> PostDVD([FromBody] DVD dVD)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            dVD.DVD_ID = Guid.NewGuid();
            dVD.DVD_ADDMOD_Datetime = DateTime.Now;
            this._context.DVD.Add(dVD);
            try
            {
                await this._context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (this.DVDExists(dVD.DVD_ID))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return this.CreatedAtAction("GetDVD", new { id = dVD.DVD_ID }, dVD);
        }

        // DELETE: api/Dvds/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDVD([FromRoute] Guid id)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            DVD dVD = await this._context.DVD.SingleOrDefaultAsync(m => m.DVD_ID == id);
            if (dVD == null)
            {
                return this.NotFound();
            }

            this._context.DVD.Remove(dVD);
            await this._context.SaveChangesAsync();

            return this.Ok(dVD);
        }

        private bool DVDExists(Guid id)
        {
            return this._context.DVD.Any(e => e.DVD_ID == id);
        }
    }
}