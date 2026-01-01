
using ReminderApp.Application.Ports;

namespace ReminderApp.Application.UseCases;

public class DeleteReminder
{
    private readonly IReminderRepository _repo;

    public DeleteReminder(IReminderRepository repo) { _repo = repo; }

    public async Task ExecuteAsync(Guid userId, Guid id, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity is null) return; // idempotent delete
        if (entity.UserId != userId) throw new UnauthorizedAccessException("Cannot delete another user's reminder.");
        await _repo.DeleteAsync(id, ct);
    }
}
