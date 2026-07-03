using RV_Assignment_1.Models;
using Microsoft.EntityFrameworkCore;

namespace RV_Assignment_1.DAOs
{
    public class BulletinPostDAO
    {
        // 1. Áp dụng Singleton Pattern
        private static BulletinPostDAO instance = null;
        private static readonly object instanceLock = new object();

        private BulletinPostDAO() { }

        public static BulletinPostDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new BulletinPostDAO();
                    }
                    return instance;
                }
            }
        }

        // 2. Các hàm tương tác Database (Mỗi lần gọi khởi tạo 1 Context mới để tránh lỗi Thread-safe)
        public IEnumerable<BulletinPost> GetAll()
        {
            using var context = new CampusBulletinTestContext();
            return context.BulletinPosts
                .Include(n => n.Category)
                .Include(n => n.CreatedById)
                .ToList();
        }

        public BulletinPost? GetById(int id)
        {
            using var context = new CampusBulletinTestContext();
            return context.BulletinPosts
                .Include(n => n.Category)
                .Include(n => n.CreatedById)
                .FirstOrDefault(x => x.PostId == id);
        }
        public void Add(BulletinPost newsArticle)
        {
            using var context = new CampusBulletinTestContext();
            context.BulletinPosts.Add(newsArticle);
            context.SaveChanges();
        }

        // Cập nhật bài viết
        public void Update(BulletinPost bulletinPost)
        {
            using var context = new CampusBulletinTestContext();

            // Entity Framework sẽ tự động nhận diện các trường thay đổi dựa trên Khóa chính (NewsArticleId)
            context.BulletinPosts.Update(bulletinPost);
            context.SaveChanges();
        }

        // Xóa bài viết
        public void Delete(BulletinPost bulletinPost)
        {
            using var context = new CampusBulletinTestContext();
            context.BulletinPosts.Remove(bulletinPost);
            context.SaveChanges();
        }
    }
}
