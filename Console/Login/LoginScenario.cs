using Contracts.Authorization;
using Models.BankAccounts.ResultTypes;
using Spectre.Console;

namespace Console.Login;

public class LoginScenario : IScenario
{
    private readonly IAuthorizationService _service;

    public LoginScenario(IAuthorizationService service)
    {
        _service = service;
    }

    public string Name => "Login";

    public void Run()
    {
        long accountNumber = AnsiConsole.Ask<long>("Enter your account number");
        int? pin = null;
        if (!_service.Admin)
        {
            pin = AnsiConsole.Ask<int>("Enter your account's pin");
        }

        LoginResult result = _service.Login(accountNumber, pin).GetAwaiter().GetResult();

        string resultMessage = result switch
        {
            LoginResult.LoginSuccess => "Logged in successfully",
            LoginResult.WrongPin => "Incorrect pin",
            LoginResult.AccountNotFound => "Can not find an account with this account number",
            _ => throw new ArgumentOutOfRangeException(nameof(result)),
        };

        AnsiConsole.Write(resultMessage);
        System.Console.ReadKey();
    }
    
}