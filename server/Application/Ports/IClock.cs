
namespace ReminderApp.Application.Ports;

public interface IClock
{
    DateTimeOffset UtcNow { get; }
}
