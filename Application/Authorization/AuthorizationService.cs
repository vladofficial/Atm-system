using Abstractions.Repositories;
using Application.Utility;
using Contracts.Authorization;
using Models.BankAccounts;
using Models.BankAccounts.ResultTypes;

namespace Application.Authorization;

public class AuthorizationService : IAuthorizationService
{
    private readonly IBankAccountRepository _repository;

    private readonly IAdminRepository _adminRepository;

    private long? CurrentAccount { get; set; }

    public AuthorizationService(IBankAccountRepository repository, IAdminRepository adminRepository)
    {
        _repository = repository;
        _adminRepository = adminRepository;
        Admin = false;
        CurrentAccount = null;
    }

    public bool Admin { get; private set; }

    public bool IsAuthorized(long accountNumber)
    {
        return CurrentAccount == accountNumber;
    }

    public async Task<LoginResult> Login(long accountNumber, int? pin)
    {
        if (pin is null)
        {
            if (!Admin)
            {
                return new LoginResult.WrongPin();
            }

            CurrentAccount = accountNumber;
            return new LoginResult.LoginSuccess();
        }

        BankAccount? account = await _repository.FindByAccountNumber(accountNumber);

        if (account is null)
        {
            return new LoginResult.AccountNotFound();
        }

        int hashedPin = PinHasher.HashPin(pin.Value);

        if (account.Pin != hashedPin)
        {
            return new LoginResult.WrongPin();
        }

        CurrentAccount = accountNumber;
        return new LoginResult.LoginSuccess();
    }

    public async Task<LoginResult> LoginAdmin(int password)
    {
        if (await _adminRepository.IsAdmin(password))
        {
            Admin = true;
            return new LoginResult.LoginSuccess();
        }

        return new LoginResult.WrongPin();
    }

    public void Logout()
    {
        CurrentAccount = null;
        Admin = false;
    }

    public long? GetCurrentAccount()
    {
        return CurrentAccount;
    }
}