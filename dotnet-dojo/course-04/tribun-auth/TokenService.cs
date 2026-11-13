using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

public sealed class TokenSettings
{
    public const string Issuer = "tribun-auth";
    public const string Audience = "tribun-client";
    public const int LifetimeSeconds = 900;
    public SymmetricSecurityKey SigningKey { get; }

    public TokenSettings(IConfiguration configuration)
    {
        var keyText = configuration["Jwt:Key"];
        if (string.IsNullOrWhiteSpace(keyText) || Encoding.UTF8.GetByteCount(keyText) < 32)
            throw new InvalidOperationException("Set Jwt__Key to a random signing key of at least 32 bytes.");
        SigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyText));
    }
}

public sealed class TokenService(TokenSettings settings)
{
    public TokenResponse CreateToken(DemoUser user)
    {
        var token = new JwtSecurityToken(TokenSettings.Issuer, TokenSettings.Audience,
            new[] { new Claim("sub", user.Username), new Claim("role", user.Role), new Claim("jti", Guid.NewGuid().ToString()) },
            expires: DateTime.UtcNow.AddSeconds(TokenSettings.LifetimeSeconds),
            signingCredentials: new SigningCredentials(settings.SigningKey, SecurityAlgorithms.HmacSha256));
        return new TokenResponse(new JwtSecurityTokenHandler().WriteToken(token), "Bearer", TokenSettings.LifetimeSeconds);
    }
}

public record TokenResponse(string AccessToken, string TokenType, int ExpiresIn);
