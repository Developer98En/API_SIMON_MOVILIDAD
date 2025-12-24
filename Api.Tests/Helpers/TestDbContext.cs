using Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Api.Tests.Helpers
{
    public static class TestDbContext
    {
        public static AppDbContext Create()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }
    }
}
