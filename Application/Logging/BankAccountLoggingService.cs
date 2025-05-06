using Abstractions.Repositories;
using Contracts.BankAccounts;
using Models.BankAccounts.ResultTypes;
using Models.Logging;

namespace Application.Logging;

public class BankAccountLoggingService : IBankAccountService
{
    private readonly IBankAccountService _innerService;
    private readonly ILogRepository _repository;

    public BankAccountLoggingService(IBankAccountService innerService, ILogRepository repository)
    {
        _innerService = innerService;
        _repository = repository;
    }
    
    public long? GetAccountNumber()
    {
        return _innerService.GetAccountNumber();
    }

    public async Task<DepositResult> Deposit(decimal amount)
    {
        DepositResult result = await _innerService.Deposit(amount);
        OperationStatus status = OperationStatus.Success;
        string comment = "Success";
        long? number = _innerService.GetAccountNumber();

        if (result is DepositResult.Unauthorized || number is null)
        {
            status = OperationStatus.Failure;
            comment = "You are not authorized to deposit from this account";
            return new DepositResult.Unauthorized();
        }

        if (result is DepositResult.InvalidAmount)
        {
            status = OperationStatus.Failure;
            comment = "Can only withdraw positive amount of money";
        }

        _repository.AddLog(new OperationLog(
            number.Value,
            OperationType.Deposit,
            amount,
            DateTime.Now,
            status,
            comment)).GetAwaiter().GetResult();

        return result;
    }

    public async Task<WithdrawResult> Withdraw(decimal amount)
    {
        WithdrawResult result = await _innerService.Withdraw(amount);
        OperationStatus status = OperationStatus.Success;
        string comment = "Success";
        long? number = _innerService.GetAccountNumber();

        if (result is WithdrawResult.Unauthorized || number is null)
        {
            status = OperationStatus.Failure;
            comment = "You are not authorized to deposit from this account";
            return new WithdrawResult.Unauthorized();
        }

        if (result is WithdrawResult.InvalidAmount)
        {
            status = OperationStatus.Failure;
            comment = "Can only withdraw positive amount of money";
        }

        if (result is WithdrawResult.NotEnoughMoney)
        {
            status = OperationStatus.Failure;
            comment = "Insufficient funds on account";
        }

        _repository.AddLog(new OperationLog(
            number.Value,
            OperationType.Withdrawal,
            amount,
            DateTime.Now,
            status,
            comment)).GetAwaiter().GetResult();

        return result;
    }

    public async Task<CreateResult> CreateAccount(long accountNumber, int pin)
    {
        CreateResult result = await _innerService.CreateAccount(accountNumber, pin);
        OperationStatus status = OperationStatus.Success;
        string comment = "Success";

        if (result is CreateResult.InvalidPin)
        {
            status = OperationStatus.Failure;
            comment = "Failure: Invalid pin";
        }

        if (result is CreateResult.NumberAlreadyTaken)
        {
            status = OperationStatus.Failure;
            comment = "Failure: Account number already reserved";
        }

        _repository.AddLog(new OperationLog(accountNumber, OperationType.Creation, null, DateTime.Now, status,
            comment)).GetAwaiter().GetResult();
        
        return result;
    }

    public async Task<GetBalanceResult> GetBalance()
    {
        return await _innerService.GetBalance();
    }

    public Task<LoginResult> Login(long accountNumber, int? pin)
    {
        return _innerService.Login(accountNumber, pin);
    }

    public Task<LoginResult> LoginAdmin(int password)
    {
        return _innerService.LoginAdmin(password);
    }

    public void Logout()
    {
        _innerService.Logout();
    }

    public long? GetCurrentAccount()
    {
        return _innerService.GetCurrentAccount();
    }
}