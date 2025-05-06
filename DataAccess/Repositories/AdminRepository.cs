using Abstractions.Repositories;
using Application.Utility;
using Itmo.Dev.Platform.Postgres.Connection;
using Npgsql;

namespace DataAccess.Repositories;

public class AdminRepository : IAdminRepository
{
    private readonly IPostgresConnectionProvider _provider;

    public AdminRepository(IPostgresConnectionProvider provider)
    {
        _provider = provider;
    }

    public async Task<bool> IsAdmin(int password)
    {
        string query = "SELECT password_hash FROM admin_credentials WHERE id = 1;";

        await using NpgsqlConnection connection = await _provider.GetConnectionAsync(default);
        await using var command = new NpgsqlCommand(query, connection);

        object? result = await command.ExecuteScalarAsync();
        if (result is int storedHash)
        {
            return storedHash == PinHasher.HashPin(password);
        }

        return false;
    }

    public async Task ChangeAdminPassword(int oldPassword, int newPassword)
    {
        await using NpgsqlConnection connection = await _provider.GetConnectionAsync(default);

        const string checkQuery = "SELECT COUNT(*) FROM admin_credentials WHERE id = 1;";
        await using var checkCommand = new NpgsqlCommand(checkQuery, connection);
        long? count = (long?)await checkCommand.ExecuteScalarAsync();

        int newHash = PinHasher.HashPin(newPassword);

        if (count == 0) 
        {
            const string insertQuery = """
                                           INSERT INTO admin_credentials (id, password_hash) 
                                           VALUES (1, @newHash);
                                       """;

            await using var insertCommand = new NpgsqlCommand(insertQuery, connection);
            insertCommand.Parameters.AddWithValue("@newHash", newHash);
            await insertCommand.ExecuteNonQueryAsync();
        }
        else
        {
            const string updateQuery = """
                                           UPDATE admin_credentials 
                                           SET password_hash = @newHash 
                                           WHERE id = 1 AND password_hash = @oldHash;
                                       """;

            int oldHash = PinHasher.HashPin(oldPassword);

            await using var updateCommand = new NpgsqlCommand(updateQuery, connection);
            updateCommand.Parameters.AddWithValue("@oldHash", oldHash);
            updateCommand.Parameters.AddWithValue("@newHash", newHash);

            int rowsAffected = await updateCommand.ExecuteNonQueryAsync();
            if (rowsAffected == 0)
            {
                throw new UnauthorizedAccessException("Invalid old admin password.");
            }
        }
    }
}