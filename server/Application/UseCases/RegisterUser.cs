
using ReminderApp.Application.Ports;
using ReminderApp.Domain.Entities;

namespace ReminderApp.Application.UseCases;

public class RegisterUser
{
    private readonly IUserRepository _users;
    private readonly IPasswordHasher _hasher;

    public RegisterUser(IUserRepository users, IPasswordHasher hasher)
    { _users = users; _hasher = hasher; }

    public async Task<Guid> ExecuteAsync(string name, string email, string plainPassword, CancellationToken ct)
    {
        if (await _users.ExistsByEmailAsync(email, ct))
            throw new InvalidOperationException("Email already registered.");
        var hash = _hasher.Hash(plainPassword);
        var user = new User(name, email, hash);
        await _users.AddAsync(user, ct);
        return user.Id;
    }
}
