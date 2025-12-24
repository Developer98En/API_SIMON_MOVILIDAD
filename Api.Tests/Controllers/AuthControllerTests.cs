using Api.Controllers;
using Api.Models;
using Api.Services;
using Api.Tests.Helpers;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Api.Tests.Controllers
{
    public class AuthControllerTests
    {
        [Fact]
        public async Task Login_ShouldReturnToken_WhenCredentialsAreValid()
        {
            var db = TestDbContext.Create();
            var jwt = new JwtService();

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = "test@test.com",
                PasswordHash = Convert.ToBase64String(
                    System.Security.Cryptography.SHA256.HashData(
                        System.Text.Encoding.UTF8.GetBytes("1234")
                    )
                ),
                Role = "USER"
            };

            db.Users.Add(user);
            await db.SaveChangesAsync();

            var controller = new AuthController(db, jwt);

            var result = await controller.Login(
                new LoginRequest("test@test.com", "1234")
            );

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Login_ShouldReturnUnauthorized_WhenInvalidCredentials()
        {
            var db = TestDbContext.Create();
            var jwt = new JwtService();

            var controller = new AuthController(db, jwt);

            var result = await controller.Login(
                new LoginRequest("fail@test.com", "wrong")
            );

            Assert.IsType<UnauthorizedResult>(result);
        }
    }
}
