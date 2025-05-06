using Models.BankAccounts;

namespace Abstractions.Repositories;

public interface IBankAccountRepository
{
    public Task<BankAccount?> FindByAccountNumber(long accountNumber);

    public Task CreateAccount(BankAccount account);

    public Task SetBalance(long accountNumber, decimal newBalance);

    public Task<decimal?> GetBalance(long accountNumber);
}