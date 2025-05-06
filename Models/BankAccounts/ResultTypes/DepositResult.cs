namespace Models.BankAccounts.ResultTypes;

public abstract record DepositResult
{
    public sealed record DepositSuccess() : DepositResult;

    public sealed record InvalidAmount(decimal Amount) : DepositResult;

    public sealed record Unauthorized : DepositResult;
}