using Contracts.BankAccounts;
using System.Diagnostics.CodeAnalysis;

namespace Console.Balance;

public class BalanceScenarioProvider : IScenarioProvider
{
    private readonly IBankAccountService _service;

    public BalanceScenarioProvider(IBankAccountService service)
    {
        _service = service;
    }

    public bool TryGetScenario(
        [NotNullWhen(true)] out IScenario? scenario)
    {
        if (_service.GetCurrentAccount() is null)
        {
            scenario = null;
            return false;
        }
        
        scenario = new BalanceScenario(_service);
        return true;
    }
}