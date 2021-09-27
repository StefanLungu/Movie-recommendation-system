using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Helpers;
using UsersManagementMicroservice.Data;
using UsersManagementMicroservice.Entities;
using UsersManagementMicroservice.Helpers;
using UsersManagementMicroservice.Models;
using UsersManagementMicroservice.Models.Account;
using UsersManagementMicroservice.Resources;
using UsersManagementMicroservice.Services;

namespace UsersManagementMicroservice.Controllers
{
    [Route("api/v1/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IJwtService _jwtService;

        public UsersController(DataContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            User user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound(Messages.NotFoundMessage("User", id));
            }

            return user;
        }

        [HttpPost]
        public async Task<ActionResult<User>> Register(RegisterRequest registerRequest)
        {
            if (UserWithEmailExists(registerRequest.Email) || UserWithUsernameExists(registerRequest.Username))
            {
                return BadRequest(Messages.DuplicateUsernameOrEmail);
            }

            registerRequest.Password = Crypto.SHA256(registerRequest.Password);

            User user = new User
            {
                Email = registerRequest.Email,
                Username = registerRequest.Username,
                Password = registerRequest.Password
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(AuthenticateRequest authenticateRequest)
        {
            User user = await _context.Users.SingleOrDefaultAsync(
                u => u.Username == authenticateRequest.Username &&
                u.Password == Crypto.SHA256(authenticateRequest.Password));

            if (user == null)
            {
                return BadRequest(Messages.InvalidCredentials);
            }

            AuthenticateResult authenticateResult = new AuthenticateResult
            {
                Id = user.Id,
                Username = user.Username,
                Token = _jwtService.GenerateJwtToken(user)
            };

            return Ok(authenticateResult);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            User user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound(Messages.NotFoundMessage("User", id));
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserWithEmailExists(string email)
        {
            return _context.Users.Where(u => u.Email == email).FirstOrDefault() != null;
        }

        private bool UserWithUsernameExists(string username)
        {
            return _context.Users.Where(u => u.Username == username).FirstOrDefault() != null;
        }
    }
}
