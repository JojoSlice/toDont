using Api.Data;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.test.ServiceTests;

public class ServiceTestFixture : IDisposable
{
    public AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    public async Task<User> CreateTestUserAsync(
        AppDbContext context,
        string username = "testuser",
        string password = "password123"
    )
    {
        var user = new User { UserName = username, PasswordHash = password };
        context.Users.Add(user);
        await context.SaveChangesAsync();
        return user;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
