using RV_Assignment_1.Models;
using RV_Assignment_1.Repositories;

namespace RV_Assignment_1.service
{
    public interface IBulletinCategoryService
    {
        IQueryable<BulletinCategory> GetAll();
        BulletinCategory GetById(short id);
        void Add(BulletinCategory category);
        void Update(BulletinCategory category);
        bool Delete(short id);
    }
    public class BulletinCategoryService : IBulletinCategoryService
    {
        private readonly IGenericRepository<BulletinCategory> _bulletCategoryRepo;
        private readonly IGenericRepository<BulletinPost> _postRepo;

        public BulletinCategoryService(IGenericRepository<BulletinCategory> bulletCategoryRepo, IGenericRepository<BulletinPost> postRepo)
        {
            _bulletCategoryRepo = bulletCategoryRepo;
            _postRepo = postRepo;
        }

        public IQueryable<BulletinCategory> GetAll() => _bulletCategoryRepo.GetAllQueryable();
        public BulletinCategory GetById(short id) => _bulletCategoryRepo.Find(id);

        public void Add(BulletinCategory category)
        {
            //bool isExist = _categoryRepo.GetAllQueryable().Any(c => c.CategoryName.ToLower().Trim() == category.CategoryName.ToLower().Trim());
            //if (isExist) throw new Exception("Tên danh mục đã tồn tại!");
            _bulletCategoryRepo.Add(category);
            _bulletCategoryRepo.SaveChanges();
        }

        public void Update(BulletinCategory category)
        {
            _bulletCategoryRepo.Update(category);
            _bulletCategoryRepo.SaveChanges();
        }

        public bool Delete(short id)
        {
            // YÊU CẦU ĐỀ BÀI: Không cho xóa Category nếu đã có bài báo thuộc Category này
            bool hasNews = _postRepo.GetAllQueryable().Any(n => n.CategoryId == id);
            if (hasNews) return false;

            var category = _bulletCategoryRepo.Find(id);
            if (category != null)
            {
                _bulletCategoryRepo.Remove(category);
                _bulletCategoryRepo.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
