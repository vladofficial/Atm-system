using Contracts.Authorization;
using Models.BankAccounts.ResultTypes;
using Spectre.Console;

namespace Console.AdminLogin;

public class AdminLoginScenario : IScenario
{
    private readonly IAuthorizationService _service;

    public AdminLoginScenario(IAuthorizationService service)
    {
        _service = service;
    }

    public string Name => "Admin Login";

    public void Run()
    {
        int adminPassword = AnsiConsole.Ask<int>("Enter admin password");

        LoginResult adminLoginResult = _service.LoginAdmin(adminPassword).GetAwaiter().GetResult();
        
        if (adminLoginResult is LoginResult.LoginSuccess)
        {
            AnsiConsole.WriteLine("Logged in as admin");
            AnsiConsole.WriteLine("\n");
            System.Console.ReadKey();

        }
        else
        {
            AnsiConsole.WriteLine("Incorrect admin password");
            Environment.Exit(0);
        }
    }
}
