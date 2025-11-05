using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Api.test.ServiceTests;

[Collection("ServiceTests")]
public class JwtServiceTest(ServiceTestFixture fixture, JwtFixture jwtFixture)
{
    private readonly ServiceTestFixture _fixture = fixture;
    private readonly JwtFixture _jwtFixture = jwtFixture;

    [Fact]
    public async Task GenerateToken_ShouldReturnValidJwtFormat()
    {
        using var context = _fixture.CreateContext();
        var user = await _fixture.CreateTestUserAsync(context, username: "jwtuser");

        var token = _jwtFixture.JwtService.GenerateToken(user);

        var tokenParts = token.Split('.');
        Assert.Equal(3, tokenParts.Length);
    }

    [Fact]
    public async Task GenerateToken_ShouldContainCorrectClaims()
    {
        using var context = _fixture.CreateContext();
        var testUsername = "claimtestuser";
        var user = await _fixture.CreateTestUserAsync(context, username: testUsername);

        var token = _jwtFixture.JwtService.GenerateToken(user);

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        var usernameClaim = jwtToken.Claims.FirstOrDefault(c =>
            c.Type == ClaimTypes.Name || c.Type == "unique_name"
        );
        Assert.NotNull(usernameClaim);
        Assert.Equal(testUsername, usernameClaim.Value);

        // Check issuer
        Assert.Equal("TestIssuer", jwtToken.Issuer);

        // Check audience
        Assert.Contains("TestAudience", jwtToken.Audiences);
    }

    [Fact]
    public async Task GenerateToken_ShouldHaveValidExpiryTime()
    {
        using var context = _fixture.CreateContext();
        var user = await _fixture.CreateTestUserAsync(context);

        var token = _jwtFixture.JwtService.GenerateToken(user);

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        var expectedExpiry = DateTime.UtcNow.AddMinutes(30);
        var actualExpiry = jwtToken.ValidTo;

        Assert.True(
            actualExpiry >= expectedExpiry.AddMinutes(-1)
                && actualExpiry <= expectedExpiry.AddMinutes(1),
            $"Expected expiry around {expectedExpiry}, but got {actualExpiry}"
        );
    }

    [Fact]
    public async Task GenerateToken_WithDifferentUsers_ShouldGenerateDifferentTokens()
    {
        using var context = _fixture.CreateContext();
        var user1 = await _fixture.CreateTestUserAsync(context, username: "user1");
        var user2 = await _fixture.CreateTestUserAsync(context, username: "user2");

        var token1 = _jwtFixture.JwtService.GenerateToken(user1);
        var token2 = _jwtFixture.JwtService.GenerateToken(user2);

        Assert.NotEqual(token1, token2);
    }
}
