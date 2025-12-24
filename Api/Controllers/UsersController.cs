using Api.Data;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly JwtService _jwt;

        public UsersController(AppDbContext db, JwtService jwt)
        {
            _db = db;
            _jwt = jwt;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {

            var auth = Request.Headers["Authorization"].FirstOrDefault();
            if (auth == null || !auth.StartsWith("Bearer "))
                return Unauthorized();

            var token = auth.Replace("Bearer ", "");

            var principal = _jwt.Validate(token);
            if (principal == null)
                return Unauthorized();

            var roleClaim = principal.Claims.FirstOrDefault(c =>
                c.Type == "role" ||
                c.Type == ClaimTypes.Role
            );

            if (roleClaim == null)
                return Unauthorized("Role claim missing");

            if (roleClaim.Value != "ADMIN")
                return Forbid();

            var users = _db.Users.Select(u => new
            {
                u.Id,
                u.Name,
                u.Email,
                u.Role
            }).ToList();

            return Ok(users);
        }
    }
}
