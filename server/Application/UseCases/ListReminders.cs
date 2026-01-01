
using ReminderApp.Application.Ports;
using ReminderApp.Domain.Entities;

namespace ReminderApp.Application.UseCases;

public class ListReminders
{
    private readonly IReminderRepository _repo;

    public ListReminders(IReminderRepository repo) { _repo = repo; }

    public Task<IReadOnlyList<Reminder>> ExecuteAsync(Guid userId, CancellationToken ct)
        => _repo.GetByUserAsync(userId, ct);
}
