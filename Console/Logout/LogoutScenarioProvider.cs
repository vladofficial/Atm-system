using Contracts.Authorization;
using System.Diagnostics.CodeAnalysis;

namespace Console.Logout;

public class LogoutScenarioProvider : IScenarioProvider
{
    private readonly IAuthorizationService _service;

    public LogoutScenarioProvider(IAuthorizationService service)
    {
        _service = service;
    }

    public bool TryGetScenario([NotNullWhen(true)] out IScenario? scenario)
    {
        if (_service.Admin)
        {
            scenario = new LogoutScenario(_service);
            return true;
        }

        if (_service.GetCurrentAccount() is null)
        {
            scenario = null;
            return false;
        }

        scenario = new LogoutScenario(_service);
        return true;
    }
}