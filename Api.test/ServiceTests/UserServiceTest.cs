using Api.Data;
using Api.Services;
using Microsoft.EntityFrameworkCore;

namespace Api.test.ServiceTests;

public class UserServiceTest(ServiceTestFixture fixture) : IClassFixture<ServiceTestFixture>
{
    private readonly ServiceTestFixture _fixture = fixture;

    [Fact]
    public async Task CreateUser_WithValidData_ShouldCreateUser()
    {
        using var context = _fixture.CreateContext();
        var service = new UserService(context);

        var result = await service.CreateUserAsync("testuser", "password123");

        Assert.NotNull(result);
        Assert.Equal("testuser", result.UserName);
        Assert.NotEmpty(result.PasswordHash);
        Assert.NotEqual("password123", result.PasswordHash);

        var userInDb = await context.Users.FirstOrDefaultAsync(
            u => u.UserName == "testuser",
            TestContext.Current.CancellationToken
        );
        Assert.NotNull(userInDb);
    }

    [Fact]
    public async Task CreateUserAsync_WithDuplicateUsername_ShouldThrowException()
    {
        using var context = _fixture.CreateContext();
        var service = new UserService(context);

        await service.CreateUserAsync("testuser", "password123");

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.CreateUserAsync("testuser", "password456")
        );
    }

    [Fact]
    public async Task AuthenticateAsync_WithValidData_ShouldReturnToken()
    {
        using var context = _fixture.CreateContext();
        var service = new UserService(context);
        await service.CreateUserAsync("testuser", "correctPassword");

        var result = await service.AuthenticateAsync("testuser", "correctPassword");

        Assert.NotNull(result);
        Assert.Equal("testuser", result.UserName);
    }

    [Fact]
    public async Task AuthenticateAsync_WithWrongPassword_ShouldReturnNull()
    {
        using var context = _fixture.CreateContext();
        var service = new UserService(context);
        await service.CreateUserAsync("testuser", "correctPassword");

        var result = await service.AuthenticateAsync("testuser", "wrongPassword");

        Assert.Null(result);
    }

    [Fact]
    public async Task AuthenticateAsync_WithInvalidData_ShouldReturnNull()
    {
        using var context = _fixture.CreateContext();
        var service = new UserService(context);

        var result = await service.AuthenticateAsync("testuser", "wrongPassword");

        Assert.Null(result);
    }

    [Fact]
    public async Task GetUserByIdAsync_WithValidData_ShouldReturnUser()
    {
        using var context = _fixture.CreateContext();
        var service = new UserService(context);
        await service.CreateUserAsync("testuser", "correctPassword");

        var userInDb = await context.Users.FirstOrDefaultAsync(
            u => u.UserName == "testuser",
            TestContext.Current.CancellationToken
        );

        Assert.NotNull(userInDb);

        var result = await service.GetUserByIdAsync(userInDb.Id);

        Assert.NotNull(result);
        Assert.Equal(userInDb, result);
    }

    [Fact]
    public async Task GetUserByIdAsync_WithInvalidData_ShouldReturnNull()
    {
        using var context = _fixture.CreateContext();
        var service = new UserService(context);

        var result = await service.GetUserByIdAsync(123);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetUserByUserNameAsync_WithValidData_ShouldReturnUser()
    {
        using var context = _fixture.CreateContext();
        var service = new UserService(context);
        await service.CreateUserAsync("testuser", "correctPassword");

        var userInDb = await context.Users.FirstOrDefaultAsync(
            u => u.UserName == "testuser",
            TestContext.Current.CancellationToken
        );

        Assert.NotNull(userInDb);

        var result = await service.GetUserByUserNameAsync(userInDb.UserName);

        Assert.NotNull(result);
        Assert.Equal(userInDb, result);
    }

    [Fact]
    public async Task GetUserByUserNameAsync_WithInvalidData_ShouldReturnNull()
    {
        using var context = _fixture.CreateContext();
        var service = new UserService(context);

        var result = await service.GetUserByUserNameAsync("testuser");

        Assert.Null(result);
    }
}
