using RV_Assignment_1.Models;
using System;

namespace RV_Assignment_1.DAOs
{
    public class MemberAccountDAO
    {
        private static MemberAccountDAO? instance = null;
        private static readonly object instanceLock = new object();

        private MemberAccountDAO() { }

        public static MemberAccountDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new MemberAccountDAO();
                    }
                    return instance;
                }
            }
        }

        public IEnumerable<MemberAccount> GetAllAccounts()
        {
            using var context = new CampusBulletinTestContext();
            return context.MemberAccounts.ToList();
        }

        public MemberAccount? GetAccountById(short id)
        {
            using var context = new CampusBulletinTestContext();
            return context.MemberAccounts.FirstOrDefault(a => a.AccountId == id);
        }

        public void DeleteAccount(short id)
        {
            using var context = new CampusBulletinTestContext();
            var account = context.MemberAccounts.FirstOrDefault(a => a.AccountId == id);
            if (account != null)
            {
                context.MemberAccounts.Remove(account);
                context.SaveChanges();
            }
        }
    }
}
