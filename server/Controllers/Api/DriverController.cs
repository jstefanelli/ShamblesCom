using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShamblesCom.Server.DB;
using ShamblesCom.Server.DB.DTO;
using ShamblesCom.Server.DB.Models;

namespace ShamblesCom.Server.Controllers.Api
{
	[Route("api/[controller]")]
	[ApiController]
	[ValidateModel]
	public class DriverController : ControllerBase
	{
		private readonly ShamblesDBContext _context;

		public DriverController(ShamblesDBContext context)
		{
			_context = context;
		}

		// GET: api/Driver
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Driver>>> GetDrivers()
		{
			return await _context.Drivers.ToListAsync();
		}

		// GET: api/Driver/5
		[HttpGet("{id}")]
		public async Task<ActionResult<Driver>> GetDriver(int id)
		{
			var driver = await _context.Drivers.FindAsync(id);

			if (driver == null)
			{
				return NotFound();
			}

			return driver;
		}

		// GET: api/Driver/5/Results?skip=0&count=10
		[HttpGet("{id}/Results")]
		public async Task<ActionResult<List<DTORaceResult>>> GetDriverResults(int id, [FromQuery] int skip = 0, [FromQuery] int count = 10) {
			var results = await _context.RaceResults.Where(rr => rr.DriverId == id).Include(rr => rr.Race).Include(rr => rr.Team).OrderByDescending(rr => rr.Race.DateTime).Skip(skip).Take(count).ToListAsync();

			if (results == null)
			{
				return NotFound();
			}

			return await Task.Run(() => {
				return results.Select(rr => new DTORaceResult(rr) {
					Team = new DTOTeam(rr.Team),
					Race = new DTORace(rr.Race)
				}).ToList();
			});
		}

		// PUT: api/Driver/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
		public async Task<IActionResult> PutDriver(int id, Driver driver)
		{
			if (id != driver.Id)
			{
				return BadRequest();
			}

			_context.Entry(driver).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!DriverExists(id))
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

		// POST: api/Driver
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		public async Task<ActionResult<Driver>> PostDriver(Driver driver)
		{
			_context.Drivers.Add(driver);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetDriver", new { id = driver.Id }, driver);
		}

		// DELETE: api/Driver/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteDriver(int id)
		{
			var driver = await _context.Drivers.FindAsync(id);
			if (driver == null)
			{
				return NotFound();
			}

			_context.Drivers.Remove(driver);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		private bool DriverExists(int id)
		{
			return _context.Drivers.Any(e => e.Id == id);
		}
	}
}
