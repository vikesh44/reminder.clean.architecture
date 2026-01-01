using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReminderApp.Application.UseCases;
using ReminderApp.Domain.Entities;
using System.Security.Claims;

namespace ReminderApp.Controllers;

[ApiController]
[Route("api/reminders")]
[Authorize]
public class RemindersController : ControllerBase
{
    private readonly AddReminder _addReminder;
    private readonly ListReminders _listReminders;
    private readonly DeleteReminder _deleteReminder;

    public RemindersController(AddReminder addReminder, ListReminders listReminders, DeleteReminder deleteReminder)
    { _addReminder = addReminder; _listReminders = listReminders; _deleteReminder = deleteReminder; }

    [HttpGet]
    public async Task<IActionResult> GetMine(CancellationToken ct)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var reminders = await _listReminders.ExecuteAsync(userId, ct);
        var dto = reminders.Select(r => new ReminderDto(r.Id, r.Text, r.ScheduledAt, r.CreatedAt));
        return Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateReminderRequest req, CancellationToken ct)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var id = await _addReminder.ExecuteAsync(userId, req.Text, req.ScheduledAtUtc, ct);
        return CreatedAtAction(nameof(GetMine), new { id }, new { id });
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        await _deleteReminder.ExecuteAsync(userId, id, ct);
        return NoContent();
    }
}

public record ReminderDto(Guid Id, string Text, DateTimeOffset ScheduledAt, DateTimeOffset CreatedAt);
public record CreateReminderRequest(string Text, DateTimeOffset ScheduledAtUtc);
