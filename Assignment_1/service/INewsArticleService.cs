using Assignment_1.Models;

namespace Assignment_1.service
{
    public interface INewsArticleService
    {
        IQueryable<NewsArticle> GetAll();
        NewsArticle GetById(string id);
        void Add(NewsArticle news);
        void Update(NewsArticle news);
        bool Delete(string id);
    }
}
