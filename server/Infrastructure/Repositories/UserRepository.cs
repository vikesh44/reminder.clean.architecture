using Microsoft.Data.SqlClient;
using ReminderApp.Application.Ports;
using ReminderApp.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace ReminderApp.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly string _connectionString;

    public UserRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken ct)
    {
        const string sql = "SELECT COUNT(1) FROM Users WHERE Email = @Email";
        
        await using var conn = new SqlConnection(_connectionString);
        await using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Email", email.ToLower());
        
        await conn.OpenAsync(ct);
        var count = (int)await cmd.ExecuteScalarAsync(ct);
        return count > 0;
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken ct)
    {
        const string sql = "SELECT Id, Name, Email, PasswordHash, CreatedAt FROM Users WHERE Email = @Email";
        
        await using var conn = new SqlConnection(_connectionString);
        await using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Email", email.ToLower());
        
        await conn.OpenAsync(ct);
        await using var reader = await cmd.ExecuteReaderAsync(ct);
        
        if (!await reader.ReadAsync(ct)) return null;
        
        return new User
        {
            Id = reader.GetGuid(0),
            Name = reader.GetString(1),
            Email = reader.GetString(2),
            PasswordHash = reader.GetString(3),
            CreatedAt = reader.GetDateTime(4)
        };
    }

    public async Task AddAsync(User user, CancellationToken ct)
    {
        const string sql = @"
            INSERT INTO Users (Id, Name, Email, PasswordHash, CreatedAt)
            VALUES (@Id, @Name, @Email, @PasswordHash, @CreatedAt)";
        
        await using var conn = new SqlConnection(_connectionString);
        await using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Id", user.Id);
        cmd.Parameters.AddWithValue("@Name", user.Name);
        cmd.Parameters.AddWithValue("@Email", user.Email);
        cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
        cmd.Parameters.AddWithValue("@CreatedAt", user.CreatedAt);
        
        await conn.OpenAsync(ct);
        await cmd.ExecuteNonQueryAsync(ct);
    }
}
