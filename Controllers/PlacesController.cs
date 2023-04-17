using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Places.Models;

namespace Places.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PlacesController : ControllerBase
	{
		private readonly MapContext _context;

		public PlacesController(MapContext context)
		{
			_context = context;
		}

		// GET: api/Places
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Place>>> GetPlaces()
		{
			if (_context.Places != null)
			{
				return await _context.Places.ToListAsync();
			}

			return NoContent();
		}

		// GET: api/Places/5
		[HttpGet("{id}")]
		public async Task<ActionResult<Place>> GetPlace(uint id)
		{
			if (_context.Places != null)
			{
				try
				{
					var place = await _context.Places.FindAsync(id);
					if (place == null)
					{
						return NotFound();
					}

					return place;
				}
				catch (Exception e)
				{
					throw new Exception($"Places data source is unreadable: {e.Message}");
				}
			}
			else
			{
				return NotFound();
			}

		}

		// PUT: api/Places/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
		public async Task<IActionResult> PutPlace(uint id, Place place)
		{
			if (id != place.PlaceID)
			{
				return BadRequest();
			}

			_context.Entry(place).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!PlaceExists(id))
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

		// POST: api/Places
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		public async Task<ActionResult<Place>> PostPlace(Place place)
		{
			if (_context.Places != null)
			{
				_context.Places.Add(place);
				await _context.SaveChangesAsync();

				return CreatedAtAction("GetPlace", new { id = place.PlaceID }, place);
			}
			return StatusCode(400);
		}

		// DELETE: api/Places/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeletePlace(uint id)
		{
			if (_context.Places != null)
			{
				var place = await _context.Places.FindAsync(id);
				if (place == null)
				{
					return NotFound();
				}

				_context.Places.Remove(place);
				await _context.SaveChangesAsync();

				return NoContent();
			}
			return StatusCode(StatusCodes.Status400BadRequest);

		}

		private bool PlaceExists(uint id)
		{
			if (_context.Places != null)
			{
				return _context.Places.Any(e => e.PlaceID == id);
			}
			return false;
		}
	}
}
