using Api.Services;
using Microsoft.Extensions.Configuration;

namespace Api.test.ServiceTests;

public class JwtFixture : IDisposable
{
    public IConfiguration Configuration { get; }
    public JwtService JwtService { get; }

    public JwtFixture()
    {
        var settings = new Dictionary<string, string?>
        {
            { "Jwt:Key", "supersecretkey12345678901234567890" },
            { "Jwt:Issuer", "TestIssuer" },
            { "Jwt:Audience", "TestAudience" },
            { "Jwt:ExpiryInMinutes", "30" },
        };

        Configuration = new ConfigurationBuilder().AddInMemoryCollection(settings).Build();

        JwtService = new JwtService(Configuration);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
