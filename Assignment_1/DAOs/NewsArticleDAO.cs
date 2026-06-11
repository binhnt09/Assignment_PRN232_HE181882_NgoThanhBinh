using Assignment_1.Models;
using Microsoft.EntityFrameworkCore;

namespace Assignment_1.DAOs
{
    public class NewsArticleDAO
    {
        // 1. Áp dụng Singleton Pattern
        private static NewsArticleDAO instance = null;
        private static readonly object instanceLock = new object();

        private NewsArticleDAO() { }

        public static NewsArticleDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new NewsArticleDAO();
                    }
                    return instance;
                }
            }
        }

        // 2. Các hàm tương tác Database (Mỗi lần gọi khởi tạo 1 Context mới để tránh lỗi Thread-safe)
        public IEnumerable<NewsArticle> GetAll()
        {
            using var context = new FunewsManagementContext();
            return context.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .ToList();
        }

        public NewsArticle? GetById(string id)
        {
            using var context = new FunewsManagementContext();
            return context.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .FirstOrDefault(x => x.NewsArticleId == id);
        }
        public void Add(NewsArticle newsArticle)
        {
            using var context = new FunewsManagementContext();
            context.NewsArticles.Add(newsArticle);
            context.SaveChanges();
        }

        // Cập nhật bài viết
        public void Update(NewsArticle newsArticle)
        {
            using var context = new FunewsManagementContext();

            // Entity Framework sẽ tự động nhận diện các trường thay đổi dựa trên Khóa chính (NewsArticleId)
            context.NewsArticles.Update(newsArticle);
            context.SaveChanges();
        }

        // Xóa bài viết
        public void Delete(NewsArticle newsArticle)
        {
            using var context = new FunewsManagementContext();
            context.NewsArticles.Remove(newsArticle);
            context.SaveChanges();
        }
    }
}
