using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThinBlue;

namespace ThinBlueLie.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimelineinfoController : ControllerBase
    {
        private readonly ThinbluelieContext _context;

        public TimelineinfoController(ThinbluelieContext context)
        {
            _context = context;
        }

        // GET: api/Timelineinfo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Timelineinfo>>> GetTimelineinfo()
        {
            return await _context.Timelineinfo.ToListAsync();
        }

        // GET: api/Timelineinfo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Timelineinfo>> GetTimelineinfo(int id)
        {
            var timelineinfo = await _context.Timelineinfo.FindAsync(id);

            if (timelineinfo == null)
            {
                return NotFound();
            }

            return timelineinfo;
        }

        // PUT: api/Timelineinfo/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTimelineinfo(int id, Timelineinfo timelineinfo)
        {
            if (id != timelineinfo.IdTimelineInfo)
            {
                return BadRequest();
            }

            _context.Entry(timelineinfo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TimelineinfoExists(id))
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

        // POST: api/Timelineinfo
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Timelineinfo>> PostTimelineinfo(Timelineinfo timelineinfo)
        {
            _context.Timelineinfo.Add(timelineinfo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTimelineinfo", new { id = timelineinfo.IdTimelineInfo }, timelineinfo);
        }

        // DELETE: api/Timelineinfo/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Timelineinfo>> DeleteTimelineinfo(int id)
        {
            var timelineinfo = await _context.Timelineinfo.FindAsync(id);
            if (timelineinfo == null)
            {
                return NotFound();
            }

            _context.Timelineinfo.Remove(timelineinfo);
            await _context.SaveChangesAsync();

            return timelineinfo;
        }

        private bool TimelineinfoExists(int id)
        {
            return _context.Timelineinfo.Any(e => e.IdTimelineInfo == id);
        }
    }
}
