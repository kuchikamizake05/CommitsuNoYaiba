using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
const string issuer = "tribun-auth";
const string audience = "tribun-client";
var keyText = builder.Configuration["Jwt:Key"];
if (string.IsNullOrWhiteSpace(keyText) || Encoding.UTF8.GetByteCount(keyText) < 32)
    throw new InvalidOperationException("Set Jwt__Key to a random signing key of at least 32 bytes.");
var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyText));

var hasher = new PasswordHasher<DemoUser>();
var users = new Dictionary<string, DemoUser>();
foreach (var (name, role, setting) in new[]
{
    ("pemain", "Player", "Demo:PlayerPassword"),
    ("admin", "Admin", "Demo:AdminPassword")
})
{
    var password = builder.Configuration[setting];
    if (string.IsNullOrWhiteSpace(password) || password.Length < 12)
        throw new InvalidOperationException($"Set {setting.Replace(":", "__")} to at least 12 characters.");
    var user = new DemoUser(name, role);
    user.PasswordHash = hasher.HashPassword(user, password);
    users.Add(name, user);
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.MapInboundClaims = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = signingKey,
        ValidateIssuer = true,
        ValidIssuer = issuer,
        ValidateAudience = true,
        ValidAudience = audience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha256 },
        NameClaimType = "sub",
        RoleClaimType = "role"
    };
});
builder.Services.AddAuthorization(options => options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin")));
var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/login", (LoginRequest request) =>
{
    if (string.IsNullOrWhiteSpace(request.Username) || request.Username.Length > 100 ||
        string.IsNullOrWhiteSpace(request.Password) || request.Password.Length > 256)
        return Results.BadRequest(new { message = "Username dan password wajib diisi dengan panjang yang sesuai." });
    if (!users.TryGetValue(request.Username, out var user) ||
        hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
        return Results.Unauthorized();

    var token = new JwtSecurityToken(issuer, audience,
        new[] { new Claim("sub", user.Username), new Claim("role", user.Role), new Claim("jti", Guid.NewGuid().ToString()) },
        expires: DateTime.UtcNow.AddMinutes(15),
        signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256));
    return Results.Ok(new { accessToken = new JwtSecurityTokenHandler().WriteToken(token), tokenType = "Bearer", expiresIn = 900 });
});

app.MapGet("/profil", (ClaimsPrincipal user) => Results.Ok(new
{
    username = user.FindFirstValue("sub"),
    role = user.FindFirstValue("role")
})).RequireAuthorization();

app.MapPost("/admin/pertandingan", () => Results.Ok(new
{
    message = "Akses admin diterima; latihan ini tidak menyimpan pertandingan."
})).RequireAuthorization("AdminOnly");

app.Run();

public record LoginRequest(string? Username, string? Password);
public sealed class DemoUser(string username, string role)
{
    public string Username { get; } = username;
    public string Role { get; } = role;
    public string PasswordHash { get; set; } = "";
}
public partial class Program { }
