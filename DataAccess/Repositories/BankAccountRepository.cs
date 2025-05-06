using Abstractions.Repositories;
using Itmo.Dev.Platform.Postgres.Connection;
using Models.BankAccounts;
using Npgsql;

namespace DataAccess.Repositories;

public class BankAccountRepository : IBankAccountRepository
{
    private readonly IPostgresConnectionProvider _provider;

    public BankAccountRepository(IPostgresConnectionProvider provider)
    {
        _provider = provider;
    }

    public async Task<BankAccount?> FindByAccountNumber(long accountNumber)
    {
        string query = """
                       SELECT account_number, pin_hash, balance
                       FROM accounts
                       WHERE account_number = @accountNumber
                       """;

        await using NpgsqlConnection connection = await _provider.GetConnectionAsync(default);
        await using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@accountNumber", accountNumber);

        await using NpgsqlDataReader reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new BankAccount(
                reader.GetInt64(0),
                reader.GetDecimal(2),
                reader.GetInt32(1));
        }

        return null;
    }

    public async Task CreateAccount(BankAccount account)
    {
        string query = """
                       INSERT INTO accounts (account_number, pin_hash, balance)
                       VALUES (@accountNumber, @pinHash, @balance)
                       """;

        await using NpgsqlConnection connection = await _provider.GetConnectionAsync(default);
        await using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@accountNumber", account.AccountNumber);
        command.Parameters.AddWithValue("@pinHash", account.Pin);
        command.Parameters.AddWithValue("@balance", account.Balance);

        await command.ExecuteNonQueryAsync();
    }

    public async Task SetBalance(long accountNumber, decimal newBalance)
    {
        string query = """
                       UPDATE accounts
                       SET balance = @balance
                       WHERE account_number = @accountNumber
                       """;

        await using NpgsqlConnection connection = await _provider.GetConnectionAsync(default);
        await using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@balance", newBalance);
        command.Parameters.AddWithValue("@accountNumber", accountNumber);

        await command.ExecuteNonQueryAsync();
    }

    public async Task<decimal?> GetBalance(long accountNumber)
    {
        string query = """
                       SELECT balance
                       FROM accounts
                       WHERE  account_number = @accountNumber
                       """;

        await using NpgsqlConnection connection = await _provider.GetConnectionAsync(default);
        await using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@accountNumber", accountNumber);

        await using NpgsqlDataReader reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return reader.GetDecimal(0);
        }

        return null;
    }
}