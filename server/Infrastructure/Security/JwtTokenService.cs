
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using ReminderApp.Application.Ports;
using System.Text;

namespace ReminderApp.Infrastructure.Security;

public class JwtTokenService : ITokenService
{
    private readonly SymmetricSecurityKey _key;
    private readonly string? _issuer;
    private readonly string? _audience;

    public JwtTokenService(string secret, string? issuer, string? audience)
    {
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        _issuer = issuer;
        _audience = audience;
    }

    public string CreateToken(Guid userId, string email)
    {
        var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, email)
        };
        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: creds);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
