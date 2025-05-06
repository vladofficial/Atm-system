using Contracts.BankAccounts;
using System.Diagnostics.CodeAnalysis;

namespace Console.Withdraw;

public class WithdrawScenarioProvider : IScenarioProvider
{
    private readonly IBankAccountService _service;

    public WithdrawScenarioProvider(IBankAccountService service)
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
        
        scenario = new WithdrawScenario(_service);
        return true;
    }
}