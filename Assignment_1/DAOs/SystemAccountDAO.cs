using Assignment_1.Models;
using System;

namespace Assignment_1.DAOs
{
    public class SystemAccountDAO
    {
        private static SystemAccountDAO? instance = null;
        private static readonly object instanceLock = new object();

        private SystemAccountDAO() { }

        public static SystemAccountDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new SystemAccountDAO();
                    }
                    return instance;
                }
            }
        }

        public IEnumerable<SystemAccount> GetAllAccounts()
        {
            using var context = new FunewsManagementContext();
            return context.SystemAccounts.ToList();
        }

        public SystemAccount? GetAccountById(short id)
        {
            using var context = new FunewsManagementContext();
            return context.SystemAccounts.FirstOrDefault(a => a.AccountId == id);
        }

        public void DeleteAccount(short id)
        {
            using var context = new FunewsManagementContext();
            var account = context.SystemAccounts.FirstOrDefault(a => a.AccountId == id);
            if (account != null)
            {
                context.SystemAccounts.Remove(account);
                context.SaveChanges();
            }
        }
    }
}
