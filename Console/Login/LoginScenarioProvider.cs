using Contracts.Authorization;
using System.Diagnostics.CodeAnalysis;

namespace Console.Login;

public class LoginScenarioProvider : IScenarioProvider
{
    private readonly IAuthorizationService _service;

    public LoginScenarioProvider(
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

        scenario = new LoginScenario(_service);
        return true;
    }
}