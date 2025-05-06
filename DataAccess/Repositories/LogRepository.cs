using Abstractions.Repositories;
using Itmo.Dev.Platform.Postgres.Connection;
using Models.Logging;
using Npgsql;

namespace DataAccess.Repositories;

public class LogRepository : ILogRepository
{
    private readonly IPostgresConnectionProvider _provider;

    public LogRepository(IPostgresConnectionProvider provider)
    {
        _provider = provider;
    }
    
    public async Task<IEnumerable<OperationLog>> FindAccountLogsByDate(DateTime dateTime, long accountNumber)
    {
        string query = """
                       SELECT account_number, type, amount, timestamp, status, details
                       FROM transactions
                       WHERE timestamp::date = @date AND account_number = @accountNumber
                       """;
        var logs = new List<OperationLog>();
    
        await using NpgsqlConnection connection = await _provider.GetConnectionAsync(default);
        await using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@date", dateTime.Date);
        command.Parameters.AddWithValue("@accountNumber", accountNumber);

        await using NpgsqlDataReader reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            logs.Add(new OperationLog(
                reader.GetInt64(0),
                Enum.Parse<OperationType>(reader.GetString(1)), 
                reader.IsDBNull(2) ? null : reader.GetDecimal(2),
                reader.GetDateTime(3),
                Enum.Parse<OperationStatus>(reader.GetString(4)),
                reader.GetString(5)));
        }
        return logs;
    }

    public async Task AddLog(OperationLog log)
    {
        string query = """
                                 INSERT INTO transactions (account_number, type, amount, timestamp, status, details)
                                 VALUES (@accountNumber, @type, @amount, @timestamp, @status, @details)
                             """;
        
        await using NpgsqlConnection connection = await _provider.GetConnectionAsync(default);
        await using var command = new NpgsqlCommand(query, connection);

        command.Parameters.AddWithValue("@accountNumber", log.AccountNumber);
        command.Parameters.AddWithValue("@type", log.Type.ToString());
        command.Parameters.AddWithValue("@amount", log.Amount.HasValue ? log.Amount : DBNull.Value);
        command.Parameters.AddWithValue("@timestamp", log.Timestamp);
        command.Parameters.AddWithValue("@status", log.Status.ToString());
        command.Parameters.AddWithValue("@details", log.Comment);

        await command.ExecuteNonQueryAsync();
    }
}