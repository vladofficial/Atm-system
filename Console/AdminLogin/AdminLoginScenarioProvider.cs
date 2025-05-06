using Contracts.Authorization;
using System.Diagnostics.CodeAnalysis;

namespace Console.AdminLogin;

public class AdminLoginScenarioProvider : IScenarioProvider
{
    private readonly IAuthorizationService _service;

    public AdminLoginScenarioProvider(
        IAuthorizationService service)
    {
        _service = service;
    }

    public bool TryGetScenario(
        [NotNullWhen(true)] out IScenario? scenario)
    {
        if (_service.GetCurrentAccount() is not null)
        {
            scenario = null;
            return false;
        }

        scenario = new AdminLoginScenario(_service);
        return true;
    }
}