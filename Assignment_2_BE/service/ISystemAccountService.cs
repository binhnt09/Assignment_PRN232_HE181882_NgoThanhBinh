using Assignment_2_BE.DTOs;
using Assignment_2_BE.Models;

namespace Assignment_2_BE.service
{
    public interface ISystemAccountService
    {
        IQueryable<SystemAccount> GetAllAccounts();
        SystemAccountResponseDTO? GetAccountById(short id);
        SystemAccountResponseDTO CreateAccount(SystemAccount account);
        bool UpdateAccount(short id, SystemAccount account);
        bool DeleteAccount(short id);
    }
}
