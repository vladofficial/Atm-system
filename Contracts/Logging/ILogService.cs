using Models.Logging;

namespace Contracts.Logging;

public interface ILogService
{
    public Task<IEnumerable<OperationLog>> FindAccountLogsByDate(DateTime dateTime, long accountNumber);
}