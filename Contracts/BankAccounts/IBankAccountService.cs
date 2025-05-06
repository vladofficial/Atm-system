using Models.BankAccounts.ResultTypes;

namespace Contracts.BankAccounts;

public interface IBankAccountService
{
    public long? GetAccountNumber();

    public Task<DepositResult> Deposit(decimal amount);

    public Task<WithdrawResult> Withdraw(decimal amount);

    public Task<CreateResult> CreateAccount(long accountNumber, int pin);

    public Task<GetBalanceResult> GetBalance();

    public Task<LoginResult> Login(long accountNumber, int? pin);

    public Task<LoginResult> LoginAdmin(int password);

    public void Logout();

    public long? GetCurrentAccount();
}