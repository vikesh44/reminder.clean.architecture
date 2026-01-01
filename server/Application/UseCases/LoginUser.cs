
using ReminderApp.Application.Ports;

namespace ReminderApp.Application.UseCases;

public class LoginUser
{
    private readonly IUserRepository _users;
    private readonly IPasswordHasher _hasher;
    private readonly ITokenService _tokens;

    public LoginUser(IUserRepository users, IPasswordHasher hasher, ITokenService tokens)
    { _users = users; _hasher = hasher; _tokens = tokens; }

    public async Task<string> ExecuteAsync(string email, string password, CancellationToken ct)
    {
        var user = await _users.GetByEmailAsync(email, ct) ?? throw new UnauthorizedAccessException("Invalid credentials.");
        if (!_hasher.Verify(password, user.PasswordHash)) throw new UnauthorizedAccessException("Invalid credentials.");
        return _tokens.CreateToken(user.Id, user.Email);
    }
}
