namespace Models.BankAccounts.ResultTypes;

public abstract record CreateResult
{
    public sealed record CreateSuccess : CreateResult;

    public sealed record NumberAlreadyTaken : CreateResult;

    public sealed record InvalidPin : CreateResult;
}