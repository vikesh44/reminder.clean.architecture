
using ReminderApp.Application.Ports;
using ReminderApp.Domain.Entities;

namespace ReminderApp.Application.UseCases;

public class AddReminder
{
    private readonly IReminderRepository _repo;
    private readonly IClock _clock;

    public AddReminder(IReminderRepository repo, IClock clock)
    { _repo = repo; _clock = clock; }

    public async Task<Guid> ExecuteAsync(Guid userId, string text, DateTimeOffset scheduledAtUtc, CancellationToken ct)
    {
        var reminder = new Reminder(userId, text, scheduledAtUtc);
        await _repo.AddAsync(reminder, ct);
        return reminder.Id;
    }
}
