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
    public class RaceResultController : ControllerBase
    {
        private readonly ShamblesDBContext _context;

        public RaceResultController(ShamblesDBContext context)
        {
            _context = context;
        }

        // GET: api/RaceResult
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RaceResult>>> GetRaceResults()
        {
            return await _context.RaceResults.ToListAsync();
        }

        // GET: api/RaceResult/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RaceResult>> GetRaceResult(int id)
        {
            var raceResult = await _context.RaceResults.FindAsync(id);

            if (raceResult == null)
            {
                return NotFound();
            }

            return raceResult;
        }

        // PUT: api/RaceResult/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRaceResult(int id, RaceResult raceResult)
        {
            if (id != raceResult.Id)
            {
                return BadRequest();
            }

            _context.Entry(raceResult).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RaceResultExists(id))
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

        // POST: api/RaceResult
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RaceResult>> PostRaceResult(RaceResult raceResult)
        {
            _context.RaceResults.Add(raceResult);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRaceResult", new { id = raceResult.Id }, raceResult);
        }

        // DELETE: api/RaceResult/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRaceResult(int id)
        {
            var raceResult = await _context.RaceResults.FindAsync(id);
            if (raceResult == null)
            {
                return NotFound();
            }

            _context.RaceResults.Remove(raceResult);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RaceResultExists(int id)
        {
            return _context.RaceResults.Any(e => e.Id == id);
        }
    }
}
