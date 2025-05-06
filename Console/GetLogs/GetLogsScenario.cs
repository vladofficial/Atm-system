using System.Globalization;
using Contracts.BankAccounts;
using Contracts.Logging;
using Models.Logging;
using Spectre.Console;

namespace Console.GetLogs;

public class GetLogsScenario : IScenario
{
    private readonly IBankAccountService _service;
    private readonly ILogService _logService;

    public GetLogsScenario(IBankAccountService service, ILogService logService)
    {
        _service = service;
        _logService = logService;
    }

    public string Name => "Get Logs";

    public void Run()
    {
        long? accountNumber = _service.GetCurrentAccount();
        if (accountNumber is null)
        {
            throw new ArgumentOutOfRangeException();
        }

        string dateString = AnsiConsole.Ask<string>("Enter a date");
        var date = DateTime.Parse(dateString);
        IEnumerable<OperationLog> logs = _logService.FindAccountLogsByDate(date, accountNumber.Value).GetAwaiter().GetResult();

        foreach (OperationLog log in logs)
        {
            string message = string.Empty;
            if (log.Type is OperationType.Creation)
            {
                message =
                    $"{log.Status.ToString()} in creation of account: {log.AccountNumber} in {log.Timestamp.ToString(new CultureInfo("ru-RU"))}: {log.Comment}";
            }
            else
            {
                message =
                    $"{log.Status.ToString()} in operation {log.Type.ToString()} on account {log.AccountNumber} with amount {log.Amount} in {log.Timestamp.ToString(new CultureInfo("ru-RU"))}: {log.Comment}";
            }
            
            AnsiConsole.Write(message);
            AnsiConsole.Write("\n");
        }
        
        System.Console.ReadKey();
    }
}