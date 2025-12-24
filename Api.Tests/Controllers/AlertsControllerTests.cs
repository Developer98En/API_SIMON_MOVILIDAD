using Api.Controllers;
using Api.Models;
using Api.Services;
using Api.Tests.Helpers;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace Api.Tests.Controllers
{
    public class AlertsControllerTests
    {
        [Fact]
        public void Resolve_ShouldMarkAlertAsResolved()
        {
            var db = TestDbContext.Create();
            var jwt = new JwtService();

            var alert = new Alert
            {
                Id = 1,
                DeviceId = Guid.NewGuid(),
                Message = "Low fuel",
                Resolved = false
            };

            db.Alerts.Add(alert);
            db.SaveChanges();

            var controller = new AlertsController(db, jwt);
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

            controller.Resolve(1);

            Assert.True(db.Alerts.First().Resolved);
        }
    }
}
