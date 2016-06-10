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
    using AngularDVDs.EF;

    [Produces("application/json")]
    [Route("api/DIRECTORs")]
    public class DIRECTORsController : Controller
    {
        private readonly dvdContext _context;

        public DIRECTORsController(dvdContext context)
        {
            this._context = context;
        }

        // GET: api/DIRECTORs
        [HttpGet]
        public IEnumerable<DIRECTOR> GetDIRECTOR()
        {
            return this._context.DIRECTOR;
        }

        // GET: api/DIRECTORs/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDIRECTOR([FromRoute] Guid id)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            DIRECTOR dIRECTOR = await this._context.DIRECTOR.SingleOrDefaultAsync(m => m.DIRECTOR_ID == id);

            if (dIRECTOR == null)
            {
                return this.NotFound();
            }

            return this.Ok(dIRECTOR);
        }

        // PUT: api/DIRECTORs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDIRECTOR([FromRoute] Guid id, [FromBody] DIRECTOR dIRECTOR)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            if (id != dIRECTOR.DIRECTOR_ID)
            {
                return this.BadRequest();
            }

            this._context.Entry(dIRECTOR).State = EntityState.Modified;

            try
            {
                await this._context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!this.DIRECTORExists(id))
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

        // POST: api/DIRECTORs
        [HttpPost]
        public async Task<IActionResult> PostDIRECTOR([FromBody] DIRECTOR dIRECTOR)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            dIRECTOR.DIRECTOR_ID = Guid.NewGuid();
            dIRECTOR.DIRECTOR_ADDMOD_Datetime = DateTime.Now;
            this._context.DIRECTOR.Add(dIRECTOR);
            try
            {
                await this._context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (this.DIRECTORExists(dIRECTOR.DIRECTOR_ID))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return this.CreatedAtAction("GetDIRECTOR", new { id = dIRECTOR.DIRECTOR_ID }, dIRECTOR);
        }

        // DELETE: api/DIRECTORs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDIRECTOR([FromRoute] Guid id)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            DIRECTOR dIRECTOR = await this._context.DIRECTOR.SingleOrDefaultAsync(m => m.DIRECTOR_ID == id);
            if (dIRECTOR == null)
            {
                return this.NotFound();
            }

            this._context.DIRECTOR.Remove(dIRECTOR);
            await this._context.SaveChangesAsync();

            return this.Ok(dIRECTOR);
        }

        private bool DIRECTORExists(Guid id)
        {
            return this._context.DIRECTOR.Any(e => e.DIRECTOR_ID == id);
        }
    }
}