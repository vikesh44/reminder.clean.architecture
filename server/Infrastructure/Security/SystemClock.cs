
using ReminderApp.Application.Ports;

namespace ReminderApp.Infrastructure.Security;

public class SystemClock : IClock
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
