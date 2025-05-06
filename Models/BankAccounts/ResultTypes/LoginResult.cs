namespace Models.BankAccounts.ResultTypes;

public abstract record LoginResult
{
    public sealed record LoginSuccess : LoginResult;

    public sealed record AccountNotFound : LoginResult;

    public sealed record WrongPin : LoginResult;
}