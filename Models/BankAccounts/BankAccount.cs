namespace Models.BankAccounts;

public record BankAccount(
long AccountNumber,
decimal Balance,
int Pin);