using Microsoft.AspNetCore.Identity;

// shortcut: fixed in-memory demo accounts; use persistent Identity services for a real account lifecycle.
public sealed class DemoUserStore
{
    private readonly Dictionary<string, DemoUser> users = new();
    private readonly PasswordHasher<DemoUser> hasher = new();

    public DemoUserStore(IConfiguration configuration)
    {
        foreach (var (name, role, setting) in new[]
        {
            ("pemain", "Player", "Demo:PlayerPassword"),
            ("admin", "Admin", "Demo:AdminPassword")
        })
        {
            var password = configuration[setting];
            if (string.IsNullOrWhiteSpace(password) || password.Length < 12)
                throw new InvalidOperationException($"Set {setting.Replace(":", "__")} to at least 12 characters.");
            var user = new DemoUser(name, role);
            user.PasswordHash = hasher.HashPassword(user, password);
            users.Add(name, user);
        }
    }

    public DemoUser? VerifyCredentials(string username, string password)
    {
        if (!users.TryGetValue(username, out var user) ||
            hasher.VerifyHashedPassword(user, user.PasswordHash, password) == PasswordVerificationResult.Failed)
            return null;
        return user;
    }
}

public sealed class DemoUser(string username, string role)
{
    public string Username { get; } = username;
    public string Role { get; } = role;
    public string PasswordHash { get; set; } = "";
}
