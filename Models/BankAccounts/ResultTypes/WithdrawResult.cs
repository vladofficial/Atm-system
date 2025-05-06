namespace Models.BankAccounts.ResultTypes;

public abstract record WithdrawResult
{
    public sealed record WithdrawSuccess() : WithdrawResult;

    public sealed record NotEnoughMoney(decimal RequiredAmount, decimal RemainingAmount) : WithdrawResult;

    public sealed record InvalidAmount(decimal Amount) : WithdrawResult;

    public sealed record Unauthorized : WithdrawResult;
}