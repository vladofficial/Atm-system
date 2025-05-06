namespace Models.Logging;

public record OperationLog(long AccountNumber, OperationType Type, decimal? Amount, DateTime Timestamp, OperationStatus Status, string Comment);