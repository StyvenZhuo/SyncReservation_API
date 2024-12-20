using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReservationAPI.Database;
using Microsoft.EntityFrameworkCore;
using ReservationAPI.Model;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

using ReservationAPI.Helper;

namespace ReservationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LoginController : ControllerBase
    {
        private readonly CafeContext _CafeContext;

        public LoginController(CafeContext cafeContext)
        {
            _CafeContext = cafeContext;
        }

        // GET: api/Users/username
        [HttpGet]
        [Route("Get-Login")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsersByUsername(string username)
        {
            var users = await _CafeContext.Users
                .Where(i => i.Username == username)
                .ToListAsync();

            if (!users.Any())
            {
                return NotFound();
            }

            return Ok(users);
        }


        // PUT: api/Users/5
        [HttpPut("Update-Login/{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest("ID in URL does not match ID in request body.");
            }

            // Pastikan entitas ada di database sebelum memodifikasi
            var existingUser = await _CafeContext.Users.FindAsync(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            // Update properti entitas yang ada
            _CafeContext.Entry(existingUser).CurrentValues.SetValues(user);

            try
            {
                await _CafeContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handle jika ada masalah konkruensi
                if (!UserExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }


        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] LoginUser request)
        {
            // Validate user input
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.PasswordHash))
            {
                return BadRequest("Username and password are required.");
            }

            // Check user credentials (replace with your database validation logic)
            var user = await _CafeContext.Users
                .FirstOrDefaultAsync(u => u.Username == request.Username && u.PasswordHash == request.PasswordHash); // Ensure you hash passwords in production

            if (user == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            var claim = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username.ToString()),
                new Claim(ClaimTypes.Email, user.Email.ToString()),
            };

            var claimsIdentity = new ClaimsIdentity(claim, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = request.RememberMe
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);


            return Ok();
        }

        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }

        [HttpGet("test")]
        public JsonResult PingAuth()
        {
            return new JsonResult(new
            {
                Username = User.Name(),
                Email = User.Email(),
                Id = User.Id(),
            });
        }


        private bool UserExists(int id)
        {
            return _CafeContext.Users.Any(e => e.Id == id);
        }
    }
}
