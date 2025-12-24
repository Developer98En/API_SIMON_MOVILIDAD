using Api.Data;
using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/devices")]
    public class DevicesController : BaseController
    {
        private readonly AppDbContext _db;
        private readonly JwtService _jwt;

        public DevicesController(AppDbContext db, JwtService jwt)
        {
            _db = db;
            _jwt = jwt;
        }

        [HttpPost]
        public IActionResult Create(CreateDeviceRequest r)
        {
            GetUser(_jwt);

            var device = new Device
            {
                Id = Guid.NewGuid(),
                RealDeviceId = r.RealDeviceId,
                MaskedDeviceId = r.MaskedDeviceId,
                VehiclePlate = r.VehiclePlate
            };

            _db.Devices.Add(device);
            _db.SaveChanges();

            return Ok(device);
        }

        [HttpGet]
        public IActionResult List()
        {
            var user = GetUser(_jwt);

            var role = user.Claims.FirstOrDefault(c =>
                c.Type == "role" ||
                c.Type == ClaimTypes.Role
            )?.Value;

            var isAdmin = role == "ADMIN";

            var devices = _db.Devices
                .Select(d => new
                {
                    d.Id,
                    DeviceId = isAdmin
                        ? d.RealDeviceId
                        : d.MaskedDeviceId,
                    d.VehiclePlate,
                    UserId = _db.UserDevices
                        .Where(ud => ud.DeviceId == d.Id)
                        .Select(ud => (Guid?)ud.UserId)
                        .FirstOrDefault()
                })
                .ToList();

            return Ok(devices);
        }

        [HttpPost("assign")]
        public IActionResult Assign([FromBody] AssignRequest r)
        {
            GetUser(_jwt);

            var deviceExists = _db.Devices.Any(d => d.Id == r.DeviceId);
            if (!deviceExists)
                return NotFound("El dispositivo no existe");

            var alreadyAssigned = _db.UserDevices
                .Any(ud => ud.DeviceId == r.DeviceId);

            if (alreadyAssigned)
                return BadRequest("Este dispositivo ya está asignado");

            var userDevice = new UserDevice
            {
                UserId = r.UserId,
                DeviceId = r.DeviceId
            };

            _db.UserDevices.Add(userDevice);
            _db.SaveChanges();

            return Ok(new { message = "Dispositivo asignado correctamente" });
        }
    }

    public record CreateDeviceRequest(
        string RealDeviceId,
        string MaskedDeviceId,
        string VehiclePlate
    );

    public record AssignRequest(
        Guid UserId,
        Guid DeviceId
    );

    public record DeviceListResponse(
        Guid Id,
        string MaskedDeviceId,
        string VehiclePlate,
        Guid? UserId
    );
}
