using System.Diagnostics.CodeAnalysis;
using Contracts.BankAccounts;
using Contracts.Logging;

namespace Console.GetLogs;

public class GetLogsScenarioProvider : IScenarioProvider
{
    private readonly IBankAccountService _service;
    private readonly ILogService _logService;

    public GetLogsScenarioProvider(IBankAccountService service, ILogService logService)
    {
        _service = service;
        _logService = logService;
    }

    public bool TryGetScenario([NotNullWhen(true)] out IScenario? scenario)
    {
        if (_service.GetCurrentAccount() is null)
        {
            scenario = null;
            return false;
        }
        
        scenario = new GetLogsScenario(_service, _logService);
        return true;
    }
}