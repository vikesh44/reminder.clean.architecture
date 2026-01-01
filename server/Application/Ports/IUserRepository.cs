
using ReminderApp.Domain.Entities;

namespace ReminderApp.Application.Ports;

public interface IUserRepository
{
    Task<bool> ExistsByEmailAsync(string email, CancellationToken ct);
    Task<User?> GetByEmailAsync(string email, CancellationToken ct);
    Task AddAsync(User user, CancellationToken ct);
}
