using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Xunit;

public sealed class AuthFactory : WebApplicationFactory<Program>
{
    public string PlayerPassword { get; } = Convert.ToBase64String(RandomNumberGenerator.GetBytes(24));
    public string AdminPassword { get; } = Convert.ToBase64String(RandomNumberGenerator.GetBytes(24));
    public string SigningKey { get; } = Convert.ToBase64String(RandomNumberGenerator.GetBytes(48));
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureHostConfiguration(config => config.AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["Jwt:Key"] = SigningKey,
            ["Demo:PlayerPassword"] = PlayerPassword,
            ["Demo:AdminPassword"] = AdminPassword
        }));
        return base.CreateHost(builder);
    }
}

public sealed class AuthTests : IClassFixture<AuthFactory>
{
    private readonly AuthFactory factory;
    public AuthTests(AuthFactory factory) => this.factory = factory;

    private async Task<string> Login(HttpClient client, string name, string password)
    {
        var response = await client.PostAsJsonAsync("/login", new { username = name, password });
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal("Bearer", body.GetProperty("tokenType").GetString());
        return body.GetProperty("accessToken").GetString()!;
    }

    [Theory]
    [InlineData("/profil", "GET")]
    [InlineData("/admin/pertandingan", "POST")]
    public async Task MissingTokenReturns401(string path, string method)
    {
        using var client = factory.CreateClient();
        var response = await client.SendAsync(new HttpRequestMessage(new HttpMethod(method), path));
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Theory]
    [InlineData("pemain")]
    [InlineData("unknown")]
    public async Task BadCredentialsReturn401(string name)
    {
        using var client = factory.CreateClient();
        var response = await client.PostAsJsonAsync("/login", new { username = name, password = "incorrect" });
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Theory]
    [InlineData("", "anything")]
    [InlineData("pemain", "")]
    [InlineData(null, null)]
    public async Task MissingFieldsReturn400(string? name, string? password)
    {
        using var client = factory.CreateClient();
        var response = await client.PostAsJsonAsync("/login", new { username = name, password });
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task PlayerCanReadOwnProfileButCannotUseAdminEndpoint()
    {
        using var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
            await Login(client, "pemain", factory.PlayerPassword));
        var profile = await client.GetFromJsonAsync<JsonElement>("/profil");
        Assert.Equal("pemain", profile.GetProperty("username").GetString());
        Assert.Equal("Player", profile.GetProperty("role").GetString());
        Assert.Equal(HttpStatusCode.Forbidden,
            (await client.PostAsync("/admin/pertandingan", null)).StatusCode);
    }

    [Fact]
    public async Task AdminCanUseAdminEndpoint()
    {
        using var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
            await Login(client, "admin", factory.AdminPassword));
        Assert.Equal(HttpStatusCode.OK,
            (await client.PostAsync("/admin/pertandingan", null)).StatusCode);
    }

    [Fact]
    public async Task TamperedSignatureReturns401()
    {
        using var client = factory.CreateClient();
        var parts = (await Login(client, "pemain", factory.PlayerPassword)).Split('.');
        parts[2] = (parts[2][0] == 'A' ? "B" : "A") + parts[2][1..];
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", string.Join('.', parts));
        Assert.Equal(HttpStatusCode.Unauthorized, (await client.GetAsync("/profil")).StatusCode);
    }

    [Theory]
    [InlineData("wrong-issuer", "tribun-client", false)]
    [InlineData("tribun-auth", "wrong-audience", false)]
    [InlineData("tribun-auth", "tribun-client", true)]
    public async Task InvalidTokenClaimsReturn401(string issuer, string audience, bool expired)
    {
        using var client = factory.CreateClient();
        var token = new JwtSecurityToken(issuer, audience,
            new[] { new Claim("sub", "admin"), new Claim("role", "Admin") },
            expires: DateTime.UtcNow.AddMinutes(expired ? -10 : 10),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(factory.SigningKey)), SecurityAlgorithms.HmacSha256));
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", new JwtSecurityTokenHandler().WriteToken(token));
        Assert.Equal(HttpStatusCode.Unauthorized, (await client.GetAsync("/profil")).StatusCode);
    }
}
