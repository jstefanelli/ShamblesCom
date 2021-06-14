using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShamblesCom.Server.DB;
using ShamblesCom.Server.DB.Models;

namespace ShamblesCom.Server.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [ValidateModel]
    public class SeasonController : ControllerBase
    {
        private readonly ShamblesDBContext _context;

        public SeasonController(ShamblesDBContext context)
        {
            _context = context;
        }

        // GET: api/Season
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Season>>> GetSeasons()
        {
            return await _context.Seasons.ToListAsync();
        }

        // GET: api/Season/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Season>> GetSeason(int id)
        {
            var season = await _context.Seasons.FindAsync(id);

            if (season == null)
            {
                return NotFound();
            }

            return season;
        }

        // PUT: api/Season/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSeason(int id, Season season)
        {
            if (id != season.Id)
            {
                return BadRequest();
            }

            _context.Entry(season).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SeasonExists(id))
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

        // POST: api/Season
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Season>> PostSeason(Season season)
        {
            _context.Seasons.Add(season);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSeason", new { id = season.Id }, season);
        }

        // DELETE: api/Season/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSeason(int id)
        {
            var season = await _context.Seasons.FindAsync(id);
            if (season == null)
            {
                return NotFound();
            }

            _context.Seasons.Remove(season);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SeasonExists(int id)
        {
            return _context.Seasons.Any(e => e.Id == id);
        }
    }
}
