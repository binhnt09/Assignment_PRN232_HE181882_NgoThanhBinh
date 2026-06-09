using Assignment_1.Models;

namespace Assignment_1.service
{
    public interface ISystemAccountService
    {
        IQueryable<SystemAccount> GetAll();
        SystemAccount GetById(short id);
        void Add(SystemAccount account);
        void Update(SystemAccount account);
        bool Delete(short id);
    }
}
