using Abstractions.Repositories;
using Contracts.Logging;
using Models.Logging;

namespace Application.Logging;

public class LogService : ILogService
{
    private readonly ILogRepository _repository;

    public LogService(ILogRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<OperationLog>> FindAccountLogsByDate(DateTime dateTime, long accountNumber)
    {
        return _repository.FindAccountLogsByDate(dateTime, accountNumber);
    }
}