using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CanbanAPI.Models
{
    [Route("api/[controller]")]
    [ApiController]
    public class UzytkowniciesController : ControllerBase
    {
        private readonly CanbanDBContext _context;

        public UzytkowniciesController(CanbanDBContext context)
        {
            _context = context;
        }

        // GET: api/Uzytkownicies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Uzytkownicy>>> GetUzytkownicies()
        {
          if (_context.Uzytkownicies == null)
          {
              return NotFound();
          }
            return await _context.Uzytkownicies.ToListAsync();
        }

        // GET: api/Uzytkownicies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Uzytkownicy>> GetUzytkownicy(int id)
        {
          if (_context.Uzytkownicies == null)
          {
              return NotFound();
          }
            var uzytkownicy = await _context.Uzytkownicies.FindAsync(id);

            if (uzytkownicy == null)
            {
                return NotFound();
            }

            return uzytkownicy;
        }

        // PUT: api/Uzytkownicies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUzytkownicy(int id, Uzytkownicy uzytkownicy)
        {
            if (id != uzytkownicy.IdUzytkownika)
            {
                return BadRequest();
            }

            _context.Entry(uzytkownicy).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UzytkownicyExists(id))
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

        // POST: api/Uzytkownicies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Uzytkownicy>> PostUzytkownicy(Uzytkownicy uzytkownicy)
        {
          if (_context.Uzytkownicies == null)
          {
              return Problem("Entity set 'CanbanDBContext.Uzytkownicies'  is null.");
          }
            _context.Uzytkownicies.Add(uzytkownicy);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUzytkownicy", new { id = uzytkownicy.IdUzytkownika }, uzytkownicy);
        }

        // DELETE: api/Uzytkownicies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUzytkownicy(int id)
        {
            if (_context.Uzytkownicies == null)
            {
                return NotFound();
            }
            var uzytkownicy = await _context.Uzytkownicies.FindAsync(id);
            if (uzytkownicy == null)
            {
                return NotFound();
            }

            _context.Uzytkownicies.Remove(uzytkownicy);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UzytkownicyExists(int id)
        {
            return (_context.Uzytkownicies?.Any(e => e.IdUzytkownika == id)).GetValueOrDefault();
        }
    }
}
