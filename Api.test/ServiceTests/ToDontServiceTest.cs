using Api.Models;
using Api.Services;
using Microsoft.EntityFrameworkCore;

namespace Api.test.ServiceTests;

public class ToDontServiceTest(ServiceTestFixture fixture) : IClassFixture<ServiceTestFixture>
{
    private readonly ServiceTestFixture _fixture = fixture;

    [Fact]
    public async Task CreateAsync_WithValidData_ShouldCreateToDont()
    {
        using var context = _fixture.CreateContext();
        var user = await _fixture.CreateTestUserAsync(context);
        var service = new ToDontService(context);

        var toDont = new ToDont
        {
            Title = "Test ToDont",
            IsActive = true,
            UserId = user.Id,
        };

        var result = await service.CreateAsync(toDont);

        Assert.NotNull(result);
        Assert.Equal("Test ToDont", result.Title);
        Assert.True(result.IsActive);
        Assert.Equal(user.Id, result.UserId);

        var toDontInDb = await context.ToDonts.FirstOrDefaultAsync(
            t => t.Id == result.Id,
            TestContext.Current.CancellationToken
        );
        Assert.NotNull(toDontInDb);
    }

    [Fact]
    public async Task GetAllByUserIdAsync_WithValidUserId_ShouldReturnToDonts()
    {
        using var context = _fixture.CreateContext();
        var user = await _fixture.CreateTestUserAsync(context);
        var service = new ToDontService(context);

        var toDont1 = new ToDont
        {
            Title = "First ToDont",
            IsActive = true,
            UserId = user.Id,
        };
        var toDont2 = new ToDont
        {
            Title = "Second ToDont",
            IsActive = false,
            UserId = user.Id,
        };

        await service.CreateAsync(toDont1);
        await service.CreateAsync(toDont2);

        var result = await service.GetAllByUserIdAsync(user.Id);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Contains(result, t => t.Title == "First ToDont");
        Assert.Contains(result, t => t.Title == "Second ToDont");
    }

    [Fact]
    public async Task GetAllByUserIdAsync_WithUserIdWithNoToDonts_ShouldReturnEmptyList()
    {
        using var context = _fixture.CreateContext();
        var user = await _fixture.CreateTestUserAsync(context);
        var service = new ToDontService(context);

        var result = await service.GetAllByUserIdAsync(user.Id);

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetByIdAsync_WithValidData_ShouldReturnToDont()
    {
        using var context = _fixture.CreateContext();
        var user = await _fixture.CreateTestUserAsync(context);
        var service = new ToDontService(context);

        var toDont = new ToDont
        {
            Title = "Test ToDont",
            IsActive = true,
            UserId = user.Id,
        };
        var created = await service.CreateAsync(toDont);

        var result = await service.GetByIdAsync(created.Id, user.Id);

        Assert.NotNull(result);
        Assert.Equal(created.Id, result.Id);
        Assert.Equal("Test ToDont", result.Title);
    }

    [Theory]
    [ClassData(typeof(InvalidIdsTestData))]
    public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull(int invalidId)
    {
        using var context = _fixture.CreateContext();
        var user = await _fixture.CreateTestUserAsync(context);
        var service = new ToDontService(context);

        var result = await service.GetByIdAsync(invalidId, user.Id);

        Assert.Null(result);
    }

    [Theory]
    [ClassData(typeof(InvalidIdsTestData))]
    public async Task GetByIdAsync_WithWrongUserId_ShouldReturnNull(int wrongUserId)
    {
        using var context = _fixture.CreateContext();
        var user = await _fixture.CreateTestUserAsync(context);
        var service = new ToDontService(context);

        var toDont = new ToDont
        {
            Title = "Test ToDont",
            IsActive = true,
            UserId = user.Id,
        };
        var created = await service.CreateAsync(toDont);

        var result = await service.GetByIdAsync(created.Id, wrongUserId);

        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateAsync_WithValidData_ShouldUpdateToDont()
    {
        using var context = _fixture.CreateContext();
        var user = await _fixture.CreateTestUserAsync(context);
        var service = new ToDontService(context);

        var toDont = new ToDont
        {
            Title = "Original Title",
            IsActive = true,
            UserId = user.Id,
        };
        var created = await service.CreateAsync(toDont);

        var updatedToDont = new ToDont { Title = "Updated Title", IsActive = false };

        var result = await service.UpdateAsync(created.Id, user.Id, updatedToDont);

        Assert.NotNull(result);
        Assert.Equal("Updated Title", result.Title);
        Assert.False(result.IsActive);
        Assert.True(result.UpdatedAt > result.CreatedAt);
    }

    [Theory]
    [ClassData(typeof(InvalidIdsTestData))]
    public async Task UpdateAsync_WithInvalidId_ShouldReturnNull(int invalidId)
    {
        using var context = _fixture.CreateContext();
        var user = await _fixture.CreateTestUserAsync(context);
        var service = new ToDontService(context);

        var updatedToDont = new ToDont { Title = "Updated Title", IsActive = false };

        var result = await service.UpdateAsync(invalidId, user.Id, updatedToDont);

        Assert.Null(result);
    }

    [Theory]
    [ClassData(typeof(InvalidIdsTestData))]
    public async Task UpdateAsync_WithWrongUserId_ShouldReturnNull(int wrongUserId)
    {
        using var context = _fixture.CreateContext();
        var user = await _fixture.CreateTestUserAsync(context);
        var service = new ToDontService(context);

        var toDont = new ToDont
        {
            Title = "Original Title",
            IsActive = true,
            UserId = user.Id,
        };
        var created = await service.CreateAsync(toDont);

        var updatedToDont = new ToDont { Title = "Updated Title", IsActive = false };

        var result = await service.UpdateAsync(created.Id, wrongUserId, updatedToDont);

        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_WithValidData_ShouldDeleteToDont()
    {
        using var context = _fixture.CreateContext();
        var user = await _fixture.CreateTestUserAsync(context);
        var service = new ToDontService(context);

        var toDont = new ToDont
        {
            Title = "Test ToDont",
            IsActive = true,
            UserId = user.Id,
        };
        var created = await service.CreateAsync(toDont);

        var result = await service.DeleteAsync(created.Id, user.Id);

        Assert.True(result);

        var toDontInDb = await context.ToDonts.FirstOrDefaultAsync(
            t => t.Id == created.Id,
            TestContext.Current.CancellationToken
        );
        Assert.Null(toDontInDb);
    }

    [Theory]
    [ClassData(typeof(InvalidIdsTestData))]
    public async Task DeleteAsync_WithInvalidId_ShouldReturnFalse(int invalidId)
    {
        using var context = _fixture.CreateContext();
        var user = await _fixture.CreateTestUserAsync(context);
        var service = new ToDontService(context);

        var result = await service.DeleteAsync(invalidId, user.Id);

        Assert.False(result);
    }

    [Theory]
    [ClassData(typeof(InvalidIdsTestData))]
    public async Task DeleteAsync_WithWrongUserId_ShouldReturnFalse(int wrongUserId)
    {
        using var context = _fixture.CreateContext();
        var user = await _fixture.CreateTestUserAsync(context);
        var service = new ToDontService(context);

        var toDont = new ToDont
        {
            Title = "Test ToDont",
            IsActive = true,
            UserId = user.Id,
        };
        var created = await service.CreateAsync(toDont);

        var result = await service.DeleteAsync(created.Id, wrongUserId);

        Assert.False(result);
    }

    [Fact]
    public async Task ToggleActiveAsync_WithValidData_ShouldToggleIsActive()
    {
        using var context = _fixture.CreateContext();
        var user = await _fixture.CreateTestUserAsync(context);
        var service = new ToDontService(context);

        var toDont = new ToDont
        {
            Title = "Test ToDont",
            IsActive = true,
            UserId = user.Id,
        };
        var created = await service.CreateAsync(toDont);

        var result = await service.ToggleActiveAsync(created.Id, user.Id);

        Assert.True(result);

        var toDontInDb = await context.ToDonts.FirstOrDefaultAsync(
            t => t.Id == created.Id,
            TestContext.Current.CancellationToken
        );
        Assert.NotNull(toDontInDb);
        Assert.False(toDontInDb.IsActive);

        var result2 = await service.ToggleActiveAsync(created.Id, user.Id);
        Assert.True(result2);

        var toDontInDb2 = await context.ToDonts.FirstOrDefaultAsync(
            t => t.Id == created.Id,
            TestContext.Current.CancellationToken
        );
        Assert.NotNull(toDontInDb2);
        Assert.True(toDontInDb2.IsActive);
    }

    [Theory]
    [ClassData(typeof(InvalidIdsTestData))]
    public async Task ToggleActiveAsync_WithInvalidId_ShouldReturnFalse(int invalidId)
    {
        using var context = _fixture.CreateContext();
        var user = await _fixture.CreateTestUserAsync(context);
        var service = new ToDontService(context);

        var result = await service.ToggleActiveAsync(invalidId, user.Id);

        Assert.False(result);
    }

    [Theory]
    [ClassData(typeof(InvalidIdsTestData))]
    public async Task ToggleActiveAsync_WithWrongUserId_ShouldReturnFalse(int wrongUserId)
    {
        using var context = _fixture.CreateContext();
        var user = await _fixture.CreateTestUserAsync(context);
        var service = new ToDontService(context);

        var toDont = new ToDont
        {
            Title = "Test ToDont",
            IsActive = true,
            UserId = user.Id,
        };
        var created = await service.CreateAsync(toDont);

        var result = await service.ToggleActiveAsync(created.Id, wrongUserId);

        Assert.False(result);
    }
}
