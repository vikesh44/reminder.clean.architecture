
using ReminderApp.Domain.Entities;

namespace ReminderApp.Application.Ports;

public interface IReminderRepository
{
    Task<IReadOnlyList<Reminder>> GetByUserAsync(Guid userId, CancellationToken ct);
    Task<Reminder?> GetByIdAsync(Guid id, CancellationToken ct);
    Task AddAsync(Reminder reminder, CancellationToken ct);
    Task DeleteAsync(Guid id, CancellationToken ct);
}
