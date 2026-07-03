using Microsoft.EntityFrameworkCore;
using RV_Assignment_1.Models;

namespace RV_Assignment_1.Repositories
{
    public interface ICategoryRepository
    {
        IEnumerable<BulletinCategory> GetAll();
        BulletinCategory? GetById(int id);
        void Add(BulletinCategory category);
        void Delete(int id);
        bool HasPosts(int categoryId);
    }
    public interface IBulletinPostRepository
    {
        IEnumerable<BulletinPost> GetAll();
        BulletinPost? GetById(int id);
        void Add(BulletinPost post);
        void Update(BulletinPost post);
    }
    public class CategoryRepository : ICategoryRepository
    {
        private readonly CampusBulletinTestContext _context;
        public CategoryRepository(CampusBulletinTestContext context) => _context = context;

        public IEnumerable<BulletinCategory> GetAll() => _context.BulletinCategories.ToList();
        public BulletinCategory? GetById(int id) => _context.BulletinCategories.Find(id);
        public void Add(BulletinCategory category) { _context.BulletinCategories.Add(category); _context.SaveChanges(); }
        public void Delete(int id) { var cat = _context.BulletinCategories.Find(id); if (cat != null) { _context.BulletinCategories.Remove(cat); _context.SaveChanges(); } }
        public bool HasPosts(int categoryId) => _context.BulletinPosts.Any(p => p.CategoryId == categoryId);
    }

    public class BulletinPostRepository : IBulletinPostRepository
    {
        private readonly CampusBulletinTestContext _context;
        public BulletinPostRepository(CampusBulletinTestContext context) => _context = context;

        public IEnumerable<BulletinPost> GetAll() => _context.BulletinPosts.Include(p => p.Category).ToList();
        public BulletinPost? GetById(int id) => _context.BulletinPosts.Include(p => p.Category).FirstOrDefault(p => p.PostId == id);
        public void Add(BulletinPost post) { _context.BulletinPosts.Add(post); _context.SaveChanges(); }
        public void Update(BulletinPost post) { _context.Entry(post).State = EntityState.Modified; _context.SaveChanges(); }
    }
}
