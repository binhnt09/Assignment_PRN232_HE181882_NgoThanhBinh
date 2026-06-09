using Assignment_1.Models;
using Assignment_1.Repositories;

namespace Assignment_1.service
{
    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepository<Category> _categoryRepo;
        private readonly IGenericRepository<NewsArticle> _newsRepo;

        public CategoryService(IGenericRepository<Category> categoryRepo, IGenericRepository<NewsArticle> newsRepo)
        {
            _categoryRepo = categoryRepo;
            _newsRepo = newsRepo;
        }

        public IQueryable<Category> GetAll() => _categoryRepo.GetAllQueryable();
        public Category GetById(short id) => _categoryRepo.Find(id);

        public void Add(Category category)
        {
            _categoryRepo.Add(category);
            _categoryRepo.SaveChanges();
        }

        public void Update(Category category)
        {
            _categoryRepo.Update(category);
            _categoryRepo.SaveChanges();
        }

        public bool Delete(short id)
        {
            // YÊU CẦU ĐỀ BÀI: Không cho xóa Category nếu đã có bài báo thuộc Category này
            bool hasNews = _newsRepo.GetAllQueryable().Any(n => n.CategoryId == id);
            if (hasNews) return false;

            var category = _categoryRepo.Find(id);
            if (category != null)
            {
                _categoryRepo.Remove(category);
                _categoryRepo.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
