using Contracts.BankAccounts;
using System.Diagnostics.CodeAnalysis;

namespace Console.Deposit;

public class DepositScenarioProvider : IScenarioProvider
{
    private readonly IBankAccountService _service;

    public DepositScenarioProvider(IBankAccountService service)
    {
        _service = service;
    }

    public bool TryGetScenario([NotNullWhen(true)] out IScenario? scenario)
    {
        if (_service.GetCurrentAccount() is null)
        {
            scenario = null;
            return false;
        }
        
        scenario = new DepositScenario(_service);
        return true;
    }
}