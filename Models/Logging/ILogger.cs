namespace Models.Logging;

public interface ILogger
{
    public void Log(OperationLog log);
}