using Microsoft.Data.SqlClient;
using ReminderApp.Application.Ports;
using ReminderApp.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace ReminderApp.Infrastructure.Repositories;

public class ReminderRepository : IReminderRepository
{
    private readonly string _connectionString;

    public ReminderRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    public async Task<IReadOnlyList<Reminder>> GetByUserAsync(Guid userId, CancellationToken ct)
    {
        const string sql = @"
            SELECT Id, UserId, Text, ScheduledAt, CreatedAt
            FROM Reminders
            WHERE UserId = @UserId
            ORDER BY ScheduledAt";
        
        var reminders = new List<Reminder>();
        
        await using var conn = new SqlConnection(_connectionString);
        await using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@UserId", userId);
        
        await conn.OpenAsync(ct);
        await using var reader = await cmd.ExecuteReaderAsync(ct);
        
        while (await reader.ReadAsync(ct))
        {
            reminders.Add(new Reminder
            {
                Id = reader.GetGuid(0),
                UserId = reader.GetGuid(1),
                Text = reader.GetString(2),
                ScheduledAt = reader.GetDateTimeOffset(3),
                CreatedAt = reader.GetDateTimeOffset(4)
            });
        }
        
        return reminders;
    }

    public async Task<Reminder?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        const string sql = @"
            SELECT Id, UserId, Text, ScheduledAt, CreatedAt
            FROM Reminders
            WHERE Id = @Id";
        
        await using var conn = new SqlConnection(_connectionString);
        await using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Id", id);
        
        await conn.OpenAsync(ct);
        await using var reader = await cmd.ExecuteReaderAsync(ct);
        
        if (!await reader.ReadAsync(ct)) return null;
        
        return new Reminder
        {
            Id = reader.GetGuid(0),
            UserId = reader.GetGuid(1),
            Text = reader.GetString(2),
            ScheduledAt = reader.GetDateTimeOffset(3),
            CreatedAt = reader.GetDateTimeOffset(4)
        };
    }

    public async Task AddAsync(Reminder reminder, CancellationToken ct)
    {
        const string sql = @"
            INSERT INTO Reminders (Id, UserId, Text, ScheduledAt, CreatedAt)
            VALUES (@Id, @UserId, @Text, @ScheduledAt, @CreatedAt)";
        
        await using var conn = new SqlConnection(_connectionString);
        await using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Id", reminder.Id);
        cmd.Parameters.AddWithValue("@UserId", reminder.UserId);
        cmd.Parameters.AddWithValue("@Text", reminder.Text);
        cmd.Parameters.AddWithValue("@ScheduledAt", reminder.ScheduledAt);
        cmd.Parameters.AddWithValue("@CreatedAt", reminder.CreatedAt);
        
        await conn.OpenAsync(ct);
        await cmd.ExecuteNonQueryAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct)
    {
        const string sql = "DELETE FROM Reminders WHERE Id = @Id";
        
        await using var conn = new SqlConnection(_connectionString);
        await using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Id", id);
        
        await conn.OpenAsync(ct);
        await cmd.ExecuteNonQueryAsync(ct);
    }
}
