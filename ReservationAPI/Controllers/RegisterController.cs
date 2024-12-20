using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservationAPI.Database;
using ReservationAPI.Model;

namespace ReservationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RegisterController : ControllerBase
    {
        private readonly CafeContext _CafeContext;

        public RegisterController(CafeContext cafeContext)
        {
            _CafeContext = cafeContext;
        }


        // GET: api/Users/username
        [HttpGet("{username}")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers(string username)
        {
            var users = await _CafeContext.Users
                .Where(i => i.Username == username)
                .ToListAsync();

            if (!users.Any())
            {
                return NotFound();
            }

            return users;
        }

        [HttpPost]
        [Route("register")]
        [AllowAnonymous]
        public async Task<ActionResult<User>> PostUser(CreateUser users)
        {
            _CafeContext.Users.Add(new User { 
                Username = users.Username, 
                Email = users.Email, 
                PasswordHash = users.PasswordHash 
            });

            await _CafeContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUsers), new { username = users.Username }, users);
        }

        // DELETE: api/Users/5
        [HttpDelete("Delete-Regis/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _CafeContext.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _CafeContext.Users.Remove(user);
            await _CafeContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("username")]
        public async Task<ActionResult> PutCafe(string username, CreateUser users)
        {
            if (username != users.Username)
            {
                return BadRequest();
            }
            _CafeContext.Entry(users).State = EntityState.Modified;
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
    }
}

