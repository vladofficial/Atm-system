using Contracts.Authorization;
using Spectre.Console;

namespace Console.Logout;

public class LogoutScenario : IScenario
{
    private readonly IAuthorizationService _service;

    public LogoutScenario(IAuthorizationService service)
    {
        _service = service;
    }

    public string Name => "Logout";

    public void Run()
    {
        _service.Logout();
        string logoutMessage = "Logged out";

        AnsiConsole.WriteLine(logoutMessage);
        System.Console.ReadKey();
    }
}