using Contracts.BankAccounts;
using System.Diagnostics.CodeAnalysis;

namespace Console.CreateAccount;

public class CreateAccountScenarioProvider : IScenarioProvider
{
    private readonly IBankAccountService _service;

    public CreateAccountScenarioProvider(IBankAccountService service)
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
        
        scenario = new CreateAccountScenario(_service);
        return true;
    }
}