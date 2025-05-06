using Abstractions.Repositories;
using Application.Utility;
using Contracts.Authorization;
using Contracts.BankAccounts;
using Models.BankAccounts;
using Models.BankAccounts.ResultTypes;

namespace Application.BankAccounts;

public class BankAccountService : IBankAccountService
{
    private readonly IBankAccountRepository _repository;
    private readonly IAuthorizationService _authService;

    public BankAccountService(IBankAccountRepository repository, IAuthorizationService authService)
    {
        _repository = repository;
        _authService = authService;
    }

    public long? GetAccountNumber()
    {
        return _authService.GetCurrentAccount();
    }

    public async Task<CreateResult> CreateAccount(long accountNumber, int pin)
    {
        BankAccount? existingAccount = await _repository.FindByAccountNumber(accountNumber);
        if (existingAccount != null)
        {
            return new CreateResult.NumberAlreadyTaken();
        }

        if (accountNumber <= 0)
        {
            return new CreateResult.InvalidPin();
        }

        var newAccount = new BankAccount(accountNumber, 0, PinHasher.HashPin(pin));
        await _repository.CreateAccount(newAccount);

        return new CreateResult.CreateSuccess();
    }

    public async Task<DepositResult> Deposit(decimal amount)
    {
        long? accountNumber = _authService.GetCurrentAccount();
        if (accountNumber == null)
        {
            return new DepositResult.Unauthorized();
        }

        BankAccount? account = await _repository.FindByAccountNumber(accountNumber.Value);
        if (account == null)
        {
            return new DepositResult.Unauthorized();
        }

        if (amount <= 0)
        {
            return new DepositResult.InvalidAmount(amount);
        }

        BankAccount updatedAccount = account with { Balance = account.Balance + amount };
        await _repository.SetBalance(updatedAccount.AccountNumber, updatedAccount.Balance);

        return new DepositResult.DepositSuccess();
    }

    public async Task<WithdrawResult> Withdraw(decimal amount)
    {
        long? accountNumber = _authService.GetCurrentAccount();
        if (accountNumber == null)
        {
            return new WithdrawResult.Unauthorized();
        }

        BankAccount? account = await _repository.FindByAccountNumber(accountNumber.Value);
        if (account == null)
        {
            return new WithdrawResult.Unauthorized();
        }

        if (amount <= 0)
        {
            return new WithdrawResult.InvalidAmount(amount);
        }

        if (account.Balance < amount)
        {
            return new WithdrawResult.NotEnoughMoney(amount, account.Balance);
        }

        BankAccount updatedAccount = account with { Balance = account.Balance - amount };
        await _repository.SetBalance(updatedAccount.AccountNumber, updatedAccount.Balance);

        return new WithdrawResult.WithdrawSuccess();
    }

    public async Task<GetBalanceResult> GetBalance()
    {
        long? accountNumber = _authService.GetCurrentAccount();
        if (accountNumber == null)
        {
            return new GetBalanceResult.Unauthorized();
        }

        BankAccount? account = await _repository.FindByAccountNumber(accountNumber.Value);
        if (account == null)
        {
            return new GetBalanceResult.Unauthorized();
        }

        return new GetBalanceResult.Success(account.Balance);
    }

    public Task<LoginResult> Login(long accountNumber, int? pin)
    {
        return _authService.Login(accountNumber, pin);
    }

    public Task<LoginResult> LoginAdmin(int password)
    {
        return _authService.LoginAdmin(password);
    }

    public void Logout()
    {
        _authService.Logout();
    }

    public long? GetCurrentAccount()
    {
        return _authService.GetCurrentAccount();
    }
}