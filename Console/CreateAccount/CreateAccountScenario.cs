using Contracts.BankAccounts;
using Models.BankAccounts.ResultTypes;
using Spectre.Console;

namespace Console.CreateAccount;

public class CreateAccountScenario : IScenario
{
    private readonly IBankAccountService _service;

    public CreateAccountScenario(IBankAccountService service)
    {
        _service = service;
    }

    public string Name => "Create Account";

    public void Run()
    {
        long accountNumber = AnsiConsole.Ask<long>("Give your account an id");
        int pin = AnsiConsole.Ask<int>("Give your account a pin code");

        CreateResult result = _service.CreateAccount(accountNumber, pin).GetAwaiter().GetResult();

        string message = result switch
        {
            CreateResult.NumberAlreadyTaken => "Id you provided has already been taken",
            CreateResult.InvalidPin => "Pin should be a positive number",
            CreateResult.CreateSuccess => "Account successfully created",
            _ => throw new ArgumentOutOfRangeException(),
        };

        AnsiConsole.WriteLine(message);
        AnsiConsole.WriteLine("\n");
        System.Console.ReadKey();
    }
}