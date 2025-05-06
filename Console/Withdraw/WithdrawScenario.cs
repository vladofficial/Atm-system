using Contracts.BankAccounts;
using Models.BankAccounts.ResultTypes;
using Spectre.Console;

namespace Console.Withdraw;

public class WithdrawScenario : IScenario
{
    private readonly IBankAccountService _service;

    public WithdrawScenario(IBankAccountService service)
    {
        _service = service;
    }

    public string Name => "Withdraw";

    public void Run()
    {
        decimal amount = AnsiConsole.Ask<decimal>("Enter an amount you want to withdraw");
        WithdrawResult result = _service.Withdraw(amount).GetAwaiter().GetResult();

        string message = result switch
        {
            WithdrawResult.Unauthorized => "You are not authorized",
            WithdrawResult.InvalidAmount => "You can only withdraw a positive amount of money",
            WithdrawResult.NotEnoughMoney => "Insufficient funds",
            WithdrawResult.WithdrawSuccess => "Money successfully withdrawn",
            _ => throw new ArgumentOutOfRangeException(),
        };

        AnsiConsole.WriteLine(message);
        System.Console.ReadKey();
    }
}