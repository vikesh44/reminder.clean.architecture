namespace ReminderApp.Domain.Entities;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User() { }

    public User(string name, string email, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name required");
        if (!email.Contains('@')) throw new ArgumentException("Invalid email");
        Name = name.Trim();
        Email = email.Trim().ToLowerInvariant();
        PasswordHash = passwordHash;
    }
}
