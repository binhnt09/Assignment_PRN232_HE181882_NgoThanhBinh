using Assignment_1.Models;
using Assignment_1.Repositories;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Assignment_1.service
{
    public class SystemAccountService : ISystemAccountService
    {
        private readonly IGenericRepository<SystemAccount> _accountRepo;
        private readonly IGenericRepository<NewsArticle> _newsRepo;

        public SystemAccountService(IGenericRepository<SystemAccount> accountRepo, IGenericRepository<NewsArticle> newsRepo)
        {
            _accountRepo = accountRepo;
            _newsRepo = newsRepo;
        }

        public IQueryable<SystemAccount> GetAll() => _accountRepo.GetAllQueryable();
        public SystemAccount GetById(short id) => _accountRepo.Find(id);

        public void Add(SystemAccount account)
        {
            _accountRepo.Add(account);
            _accountRepo.SaveChanges();
        }

        public void Update(SystemAccount account)
        {
            _accountRepo.Update(account);
            _accountRepo.SaveChanges();
        }

        public bool Delete(short id)
        {
            // YÊU CẦU ĐỀ BÀI: Không cho xóa nếu tài khoản đã tạo bài báo
            bool hasNews = _newsRepo.GetAllQueryable().Any(n => n.CreatedById == id);
            if (hasNews) return false;

            var account = _accountRepo.Find(id);
            if (account != null)
            {
                _accountRepo.Remove(account);
                _accountRepo.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
