using Models.BankAccounts.ResultTypes;

namespace Contracts.Authorization;

public interface IAuthorizationService
{
    public bool IsAuthorized(long accountNumber);

    public bool Admin { get; }

    public Task<LoginResult> Login(long accountNumber, int? pin);

    public Task<LoginResult> LoginAdmin(int password);

    public void Logout();

    public long? GetCurrentAccount();
}