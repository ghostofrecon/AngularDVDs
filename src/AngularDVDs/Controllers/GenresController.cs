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
    [Produces("application/json")]
    [Route("api/Genres")]
    public class GenresController : Controller
    {
        private readonly dvdContext _context;

        public GenresController(dvdContext context)
        {
            this._context = context;
        }

        // GET: api/Genres
        [HttpGet]
        public IEnumerable<GENRE> GetGENRE()
        {
            return this._context.GENRE;
        }

        // GET: api/Genres/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGENRE([FromRoute] Guid id)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            GENRE gENRE = await this._context.GENRE.SingleOrDefaultAsync(m => m.GENRE_ID == id);

            if (gENRE == null)
            {
                return this.NotFound();
            }

            return this.Ok(gENRE);
        }

        // PUT: api/Genres/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGENRE([FromRoute] Guid id, [FromBody] GENRE gENRE)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            if (id != gENRE.GENRE_ID)
            {
                return this.BadRequest();
            }

            this._context.Entry(gENRE).State = EntityState.Modified;

            try
            {
                await this._context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!this.GENREExists(id))
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

        // POST: api/Genres
        [HttpPost]
        public async Task<IActionResult> PostGENRE([FromBody] GENRE gENRE)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            gENRE.GENRE_ID = Guid.NewGuid();
            gENRE.GENRE_ADDMOD_Datetime = DateTime.Now;
            this._context.GENRE.Add(gENRE);
            try
            {
                await this._context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                var exception = ex;
                if (this.GENREExists(gENRE.GENRE_ID))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return this.CreatedAtAction("GetGENRE", new { id = gENRE.GENRE_ID }, gENRE);
        }

        // DELETE: api/Genres/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGENRE([FromRoute] Guid id)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            GENRE gENRE = await this._context.GENRE.SingleOrDefaultAsync(m => m.GENRE_ID == id);
            if (gENRE == null)
            {
                return this.NotFound();
            }

            this._context.GENRE.Remove(gENRE);
            await this._context.SaveChangesAsync();

            return this.Ok(gENRE);
        }

        private bool GENREExists(Guid id)
        {
            return this._context.GENRE.Any(e => e.GENRE_ID == id);
        }
    }
}