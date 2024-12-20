using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReservationAPI.Database;
using ReservationAPI.Model;
using ReservationAPI.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace ReservationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class CafeController : ControllerBase
    {
        private readonly CafeContext _CafeContext;
        public CafeController(CafeContext cafeContext)
        {
            _CafeContext = cafeContext;
        }

        [HttpPost]
        [Route("Create-Cafe")]
        public async Task<IActionResult> PostUser(CreateCafe users)
        {
            try
            {
                var cafe = new Cafe
                {
                    Name = users.Name,
                    Address = users.Address,
                    OpeningHour = users.OpeningHour,
                    ClosingHour = users.ClosingHour,
                    Capacity = users.Capacity,
                    IsOpen = users.IsOpen,
                    Description = users.Description,
                };

                _CafeContext.Cafes.Add(cafe);
                await _CafeContext.SaveChangesAsync();

                return CreatedAtAction(nameof(GetCafe), new { id = cafe.Id }, cafe);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cafe>>> GetCafes()
        {
            if (_CafeContext.Cafes == null)
            {
                return NotFound();
            }
            return await _CafeContext.Cafes.ToListAsync();
        }

        [HttpGet("Get-Cafe/{id}")]
        public async Task<ActionResult<Cafe>> GetCafe(int id)
        {
            if (_CafeContext.Cafes == null)
            {
                return NotFound();
            }
            var cafe = await _CafeContext.Cafes.FindAsync(id);
            if (cafe == null)
            {
                return NotFound();
            }
            return cafe;
        }

        [HttpPut("Update-Cafe/{id}")]
        public async Task<IActionResult> UpdateCafe(int id, Cafe cafe)
        {
            if (id != cafe.Id)
            {
                return BadRequest();
            }
            _CafeContext.Entry(cafe).State = EntityState.Modified;
            try
            {
                await _CafeContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CafeExists(id))
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

        [HttpDelete("Delete-Cafe/{id}")]
        public async Task<IActionResult> DeleteCafe(int id)
        {
            var cafe = await _CafeContext.Cafes.FindAsync(id);
            if (cafe == null)
            {
                return NotFound();
            }
            _CafeContext.Cafes.Remove(cafe);
            await _CafeContext.SaveChangesAsync();
            return NoContent();
        }

        private bool CafeExists(int id)
        {
            return _CafeContext.Cafes.Any(e => e.Id == id);
        }
    }
}

