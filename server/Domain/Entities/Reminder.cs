namespace ReminderApp.Domain.Entities;

public class Reminder
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public string Text { get; set; } = string.Empty;
    public DateTimeOffset ScheduledAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    // Navigation (optional)
    public User? User { get; set; }

    public Reminder() { }

    public Reminder(Guid userId, string text, DateTimeOffset scheduledAt)
    {
        if (string.IsNullOrWhiteSpace(text)) throw new ArgumentException("Reminder text is required.");
        if (scheduledAt <= DateTimeOffset.UtcNow) throw new ArgumentException("Scheduled time must be in the future.");
        UserId = userId;
        Text = text.Trim();
        ScheduledAt = scheduledAt;
    }
}
