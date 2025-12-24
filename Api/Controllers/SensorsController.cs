using Api.Data;
using Api.Models;
using Api.Services;
using Api.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/sensors")]
    public class SensorsController : BaseController
    {
        private readonly AppDbContext _db;
        private readonly JwtService _jwt;
        private readonly IHubContext<TelemetryHub> _hub;

        public SensorsController(
            AppDbContext db,
            JwtService jwt,
            IHubContext<TelemetryHub> hub
        )
        {
            _db = db;
            _jwt = jwt;
            _hub = hub;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SensorRequest r)
        {

            GetUser(_jwt);

            var reading = new SensorReading
            {
                DeviceId = r.DeviceId,
                Latitude = r.Latitude,
                Longitude = r.Longitude,
                Speed = r.Speed,
                FuelLevel = r.FuelLevel,
                Temperature = r.Temperature,
                CreatedAt = DateTime.UtcNow
            };

            _db.SensorReadings.Add(reading);

            if (r.FuelLevel < 20)
            {
                _db.Alerts.Add(new Alert
                {
                    DeviceId = r.DeviceId,
                    Message = "Low fuel",
                    PredictedMinutesLeft = 25,
                    Resolved = false,
                    CreatedAt = DateTime.UtcNow
                });
            }

            _db.SaveChanges();

            await _hub.Clients
                .Group(r.DeviceId.ToString())
                .SendAsync("telemetryUpdate", new
                {
                    deviceId = r.DeviceId,
                    latitude = r.Latitude,
                    longitude = r.Longitude,
                    speed = r.Speed,
                    fuelLevel = r.FuelLevel,
                    temperature = r.Temperature,
                    timestamp = reading.CreatedAt
                });

            return Ok();
        }
    }

    public record SensorRequest(
        Guid DeviceId,
        double Latitude,
        double Longitude,
        double Speed,
        double FuelLevel,
        double Temperature
    );
}
