using Api.Data;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly JwtService _jwt;

        public AuthController(AppDbContext db, JwtService jwt)
        {
            _db = db;
            _jwt = jwt;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest r)
        {
            var exists = await _db.Users.AnyAsync(u => u.Email == r.Email);
            if (exists)
                return BadRequest("Email already registered");

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = r.Name,
                Email = r.Email,
                PasswordHash = Hash(r.Password),
                Role = r.Role,
                CreatedAt = DateTime.UtcNow
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest r)
        {
            var hash = Hash(r.Password);

            var user = await _db.Users
                .FirstOrDefaultAsync(u =>
                    u.Email == r.Email &&
                    u.PasswordHash == hash);

            if (user == null)
                return Unauthorized();

            return Ok(new
            {
                token = _jwt.Generate(user),
                role = user.Role,
                expiresIn = 3600
            });
        }

        private static string Hash(string v)
        {
            using var sha = SHA256.Create();
            return Convert.ToBase64String(
                sha.ComputeHash(Encoding.UTF8.GetBytes(v))
            );
        }
    }

    public record RegisterRequest(
        string Name,
        string Email,
        string Password,
        string Role
    );

    public record LoginRequest(
        string Email,
        string Password
    );
}
