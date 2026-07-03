using RV_Assignment_1.Models;
using RV_Assignment_1.Repositories;

namespace RV_Assignment_1.service
{
    public interface IMemberAccountService
    {
        IQueryable<MemberAccount> GetAll();
        MemberAccount GetById(short id);
        void Add(MemberAccount account);
        void Update(MemberAccount account);
        bool Delete(short id);
    }
    public class MemberAccountService : IMemberAccountService
    {
        private readonly IGenericRepository<MemberAccount> _accountRepo;
        private readonly IGenericRepository<BulletinPost> _postRepo;

        public MemberAccountService(IGenericRepository<MemberAccount> accountRepo, IGenericRepository<BulletinPost> newsRepo)
        {
            _accountRepo = accountRepo;
            _postRepo = newsRepo;
        }

        public IQueryable<MemberAccount> GetAll() => _accountRepo.GetAllQueryable();
        public MemberAccount GetById(short id) => _accountRepo.Find(id);

        public void Add(MemberAccount account)
        {
            _accountRepo.Add(account);
            _accountRepo.SaveChanges();
        }

        public void Update(MemberAccount account)
        {
            _accountRepo.Update(account);
            _accountRepo.SaveChanges();
        }

        public bool Delete(short id)
        {
            // YÊU CẦU ĐỀ BÀI: Không cho xóa nếu tài khoản đã tạo bài báo
            bool hasNews = _postRepo.GetAllQueryable().Any(n => n.CreatedById == id);
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
