using FluentMigrator;
using Itmo.Dev.Platform.Postgres.Migrations;

namespace DataAccess.Migrations;

[Migration(1, "Initial")]
public class Initial : SqlMigration
{
    protected override string GetUpSql(IServiceProvider serviceProvider) =>
        """
        CREATE TABLE accounts
        (
            account_id      BIGSERIAL PRIMARY KEY,
            account_number  BIGINT UNIQUE NOT NULL,
            pin_hash        INT NOT NULL,
            balance         DECIMAL(15,2) NOT NULL DEFAULT 0.00
        );

        CREATE TABLE transactions
        (
            transaction_id  BIGSERIAL PRIMARY KEY,
            account_number      BIGINT,
            type            TEXT, 
            amount          DECIMAL(15,2),
            timestamp       TIMESTAMP NOT NULL DEFAULT NOW(),
            status          TEXT,
            details         TEXT
        );

        CREATE TABLE admin_credentials
        (
            id SMALLINT PRIMARY KEY DEFAULT 1,
            password_hash INT NOT NULL
        );
        """;

    protected override string GetDownSql(IServiceProvider serviceProvider) =>
        """
        DROP TABLE transactions;
        DROP TABLE accounts;
        DROP TABLE admin_credentials;
        """;
}
