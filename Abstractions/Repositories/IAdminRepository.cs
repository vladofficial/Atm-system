namespace Abstractions.Repositories;

public interface IAdminRepository
{
    public Task<bool> IsAdmin(int password);

    public Task ChangeAdminPassword(int oldPassword, int newPassword);
}