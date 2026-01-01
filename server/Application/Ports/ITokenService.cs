
namespace ReminderApp.Application.Ports;

public interface ITokenService
{
    string CreateToken(Guid userId, string email);
}
