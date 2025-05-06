using Contracts.BankAccounts;
using Models.BankAccounts.ResultTypes;
using Spectre.Console;

namespace Console.Balance;

public class BalanceScenario : IScenario
{
    private readonly IBankAccountService _service;

    public BalanceScenario(IBankAccountService service)
    {
        _service = service;
    }

    public string Name => "Balance";

    public void Run()
    {
        GetBalanceResult result = _service.GetBalance().GetAwaiter().GetResult();
        string message;

        if (result is GetBalanceResult.Unauthorized)
        {
            message = "You are not authorized";
        }
        else if (result is GetBalanceResult.Success success)
        {
            message = $"Balance: {success.Balance.ToString()}";
        }
        else
        {
            throw new ArgumentOutOfRangeException();
        }

        AnsiConsole.WriteLine(message);
        System.Console.ReadKey();
    }
}