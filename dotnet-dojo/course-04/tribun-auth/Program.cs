using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var tokenSettings = new TokenSettings(builder.Configuration);
builder.Services.AddSingleton(tokenSettings);
builder.Services.AddSingleton<DemoUserStore>();
builder.Services.AddTransient<TokenService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.MapInboundClaims = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = tokenSettings.SigningKey,
        ValidateIssuer = true,
        ValidIssuer = TokenSettings.Issuer,
        ValidateAudience = true,
        ValidAudience = TokenSettings.Audience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha256 },
        NameClaimType = "sub",
        RoleClaimType = "role"
    };
});
builder.Services.AddAuthorization(options => options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin")));
var app = builder.Build();
// Validate the demo credentials at startup, rather than failing on the first login.
_ = app.Services.GetRequiredService<DemoUserStore>();
app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/login", (LoginRequest request, DemoUserStore users, TokenService tokens) =>
{
    if (string.IsNullOrWhiteSpace(request.Username) || request.Username.Length > 100 ||
        string.IsNullOrWhiteSpace(request.Password) || request.Password.Length > 256)
        return Results.BadRequest(new { message = "Username dan password wajib diisi dengan panjang yang sesuai." });
    var user = users.VerifyCredentials(request.Username, request.Password);
    if (user is null)
        return Results.Unauthorized();
    return Results.Ok(tokens.CreateToken(user));
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
public partial class Program { }
