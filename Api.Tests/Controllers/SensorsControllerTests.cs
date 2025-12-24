using Api.Controllers;
using Api.Data;
using Api.Hubs;
using Api.Models;
using Api.Services;
using Api.Tests.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Moq;
using Xunit;

namespace Api.Tests.Controllers
{
    public class SensorsControllerTests
    {
        private SensorsController CreateController(
            AppDbContext db,
            JwtService jwt
        )
        {
            var hubMock = new Mock<IHubContext<TelemetryHub>>();
            var controller = new SensorsController(db, jwt, hubMock.Object);

            controller.ControllerContext = new()
            {
                HttpContext = new DefaultHttpContext()
            };

            var token = jwt.Generate(new User
            {
                Id = Guid.NewGuid(),
                Email = "admin@test.com",
                Role = "ADMIN"
            });

            controller.Request.Headers["Authorization"] = $"Bearer {token}";

            return controller;
        }

        [Fact]
        public async Task Create_ShouldGenerateAlert_WhenFuelIsLow()
        {
            var db = TestDbContext.Create();
            var jwt = new JwtService();

            var controller = CreateController(db, jwt);

            await controller.Create(new SensorRequest(
                Guid.NewGuid(),
                4.6,
                -74.1,
                60,
                10, // 🔴 bajo
                30
            ));

            Assert.Single(db.Alerts);
        }

        [Fact]
        public async Task Create_ShouldNotGenerateAlert_WhenFuelIsNormal()
        {
            var db = TestDbContext.Create();
            var jwt = new JwtService();

            var controller = CreateController(db, jwt);

            await controller.Create(new SensorRequest(
                Guid.NewGuid(),
                4.6,
                -74.1,
                60,
                80, // ✅ normal
                30
            ));

            Assert.Empty(db.Alerts);
        }
    }
}
