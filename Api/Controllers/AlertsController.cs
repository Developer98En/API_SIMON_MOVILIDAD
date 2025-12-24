using Api.Data;
using Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/alerts")]
    public class AlertsController : BaseController
    {
        private readonly AppDbContext _db;
        private readonly JwtService _jwt;

        public AlertsController(AppDbContext db, JwtService jwt)
        {
            _db = db;
            _jwt = jwt;
        }

        [HttpGet("device/{id}")]
        public IActionResult Get(Guid id)
        {
            GetUser(_jwt);
            return Ok(_db.Alerts.Where(a => a.DeviceId == id && !a.Resolved));
        }

        [HttpPut("{id}/resolve")]
        public IActionResult Resolve(long id)
        {
            GetUser(_jwt);
            var a = _db.Alerts.Find(id);
            if (a == null) return NotFound();

            a.Resolved = true;
            _db.SaveChanges();
            return Ok();
        }
    }
}
