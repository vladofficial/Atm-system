using Models.Logging;

namespace Abstractions.Repositories;

public interface ILogRepository
{
    public Task<IEnumerable<OperationLog>> FindAccountLogsByDate(DateTime dateTime, long accountNumber);

    public Task AddLog(OperationLog log);
}