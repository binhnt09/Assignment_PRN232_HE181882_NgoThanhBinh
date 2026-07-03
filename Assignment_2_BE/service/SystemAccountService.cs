using Assignment_2_BE.DTOs;
using Assignment_2_BE.Models;
using Assignment_2_BE.Repositories;
using AutoMapper;

namespace Assignment_2_BE.service
{
    public class SystemAccountService : ISystemAccountService
    {
        private readonly IGenericRepository<SystemAccount> _accountRepo;
        private readonly IGenericRepository<NewsArticle> _newsRepo;
        private readonly IMapper _mapper;

        public SystemAccountService(IGenericRepository<SystemAccount> accountRepo, IGenericRepository<NewsArticle> newsRepo, IMapper mapper)
        {
            _accountRepo = accountRepo;
            _newsRepo = newsRepo;
            _mapper = mapper;
        }

        public IQueryable<SystemAccount> GetAllAccounts()
        {
            return _accountRepo.GetAllQueryable();
        }

        public SystemAccountResponseDTO? GetAccountById(short id)
        {
            var account = _accountRepo.Find(id);
            return account == null ? null : _mapper.Map<SystemAccountResponseDTO>(account);
        }

        public SystemAccountResponseDTO CreateAccount(SystemAccount account)
        {
            _accountRepo.Add(account);
            _accountRepo.SaveChanges();
            return _mapper.Map<SystemAccountResponseDTO>(account);
        }

        public bool UpdateAccount(short id, SystemAccount account)
        {
            var existing = _accountRepo.Find(id);
            if (existing == null) return false;

            existing.AccountName = account.AccountName;
            existing.AccountEmail = account.AccountEmail;
            existing.AccountRole = account.AccountRole;
            // Optionally update password if provided
            if (!string.IsNullOrEmpty(account.AccountPassword))
            {
                existing.AccountPassword = account.AccountPassword;
            }

            _accountRepo.Update(existing);
            _accountRepo.SaveChanges();
            return true;
        }

        public bool DeleteAccount(short id)
        {
            // Requirement: Cannot delete if account created any news
            var hasNews = _newsRepo.GetAllQueryable().Any(n => n.CreatedById == id);
            if (hasNews) return false;

            var account = _accountRepo.Find(id);
            if (account == null) return false;

            _accountRepo.Remove(account);
            _accountRepo.SaveChanges();
            return true;
        }
    }
}
