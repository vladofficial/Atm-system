namespace Models.BankAccounts.ResultTypes;

public abstract record GetBalanceResult
{
    public sealed record Success(decimal Balance) : GetBalanceResult;

    public sealed record Unauthorized : GetBalanceResult;
}