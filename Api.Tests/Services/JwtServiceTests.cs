using Api.Models;
using Api.Services;
using Xunit;

namespace Api.Tests.Services
{
    public class JwtServiceTests
    {
        private readonly JwtService _jwt = new();

        [Fact]
        public void Generate_ShouldReturnToken()
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = "admin@test.com",
                Role = "ADMIN"
            };

            var token = _jwt.Generate(user);

            Assert.False(string.IsNullOrEmpty(token));
        }

        [Fact]
        public void Validate_ShouldReturnPrincipal_WhenTokenIsValid()
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = "user@test.com",
                Role = "USER"
            };

            var token = _jwt.Generate(user);
            var principal = _jwt.Validate(token);

            Assert.NotNull(principal);
        }

        [Fact]
        public void Validate_ShouldReturnNull_WhenTokenIsInvalid()
        {
            var principal = _jwt.Validate("invalid.token");

            Assert.Null(principal);
        }
    }
}
