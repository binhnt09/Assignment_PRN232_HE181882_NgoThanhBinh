using Assignment_2_BE.Models;
using Assignment_2_BE.Repositories;

namespace Assignment_2_BE.service
{
    public interface INewsArticleService
    {
        IQueryable<NewsArticle> GetAll();
        NewsArticle GetById(string id);
        void Add(NewsArticle news);
        void Update(NewsArticle news);
        bool Delete(string id);
    }
    public class NewsArticleService : INewsArticleService
    {
        private readonly IGenericRepository<NewsArticle> _newsRepo;

        public NewsArticleService(IGenericRepository<NewsArticle> newsRepo)
        {
            _newsRepo = newsRepo;
        }

        public IQueryable<NewsArticle> GetAll() => _newsRepo.GetAllQueryable();
        public NewsArticle GetById(string id) => _newsRepo.Find(id);

        public void Add(NewsArticle news)
        {
            _newsRepo.Add(news);
            _newsRepo.SaveChanges();
        }

        public void Update(NewsArticle news)
        {
            _newsRepo.Update(news);
            _newsRepo.SaveChanges();
        }

        public bool Delete(string id)
        {
            var news = _newsRepo.Find(id);
            if (news != null)
            {
                _newsRepo.Remove(news);
                _newsRepo.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
