using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservationAPI.Database;
using ReservationAPI.Helper;
using ReservationAPI.Model;

namespace ReservationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly CafeContext _CafeContext;
        public ReservationsController(CafeContext CafeC)
        {

            _CafeContext = CafeC;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetReservations()
        {
            if (_CafeContext.Reservations == null)
            {
                return NotFound("Reservations database is not available");
            }
            return await _CafeContext.Reservations.ToListAsync();
        }

        [HttpGet]
        [Route("UserId")]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetUserReservations(int userId)
        {
            if (_CafeContext.Reservations == null)
            {
                return NotFound("Reservations database is not available");
            }

            var userReservations = await _CafeContext.Reservations
                .Where(r => r.UsersId == userId)  // Menggunakan 'usersId' sesuai nama field di database
                .ToListAsync();

            if (!userReservations.Any())
            {
                return NotFound($"No reservations found for user with ID {userId}");
            }

            return Ok(userReservations);
        }


        [HttpPost]
        [Route("Reservation")]
        public async Task<ActionResult<Reservation>> PostUser(CreateReservation users)
        {

            var book = _CafeContext.Reservations.Where(i => i.Status == "AVAILABLE");
            if (book == null) return NotFound();


            _CafeContext.Reservations.Add(new Reservation
            {
                CafeId = users.CafeId,
                UsersId = Convert.ToInt16(User.Id()),
                Username = users.Username,
                ReservationDate = users.ReservationDate,
                StartTime = users.StartTime,
                NumberOfGuests = users.NumberOfGuests,
                Notes = users.Notes
            });

            try
            {
                await _CafeContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                return Ok(ex.Message);
            }

            return Ok();
        }

        [HttpPut("Update-Reservation/{id}")]
        public async Task<ActionResult> PutReservations(int id, Reservation reservations)
        {
            if (id != reservations.Id)
            {
                return BadRequest();
            }
            _CafeContext.Entry(reservations).State = EntityState.Modified;
            try
            {
                await _CafeContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return Ok();
        }

        [HttpDelete("Delete-Reservation/{id}")]
        public async Task<ActionResult> DeleteReservations(int id)
        {
            if (_CafeContext.Reservations == null)
            {
                return NotFound();
            }
            var reservation = await _CafeContext.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            _CafeContext.Reservations.Remove(reservation);
            await _CafeContext.SaveChangesAsync();
            return Ok();
        }
    }
}
