using Contracts.BankAccounts;
using Models.BankAccounts.ResultTypes;
using Spectre.Console;

namespace Console.Deposit;

public class DepositScenario : IScenario
{
    private readonly IBankAccountService _service;

    public DepositScenario(IBankAccountService service)
    {
        _service = service;
    }

    public string Name => "Deposit";

    public void Run()
    {
        decimal amount = AnsiConsole.Ask<decimal>("Enter an amount you want to deposit");
        DepositResult result = _service.Deposit(amount).GetAwaiter().GetResult();

        string message = result switch
        {
            DepositResult.Unauthorized => "You are not authorized",
            DepositResult.InvalidAmount => "You can only deposit a positive amount of money",
            DepositResult.DepositSuccess => "Money successfully deposited",
            _ => throw new ArgumentOutOfRangeException(),
        };

        AnsiConsole.WriteLine(message);
        System.Console.ReadKey();
    }
}