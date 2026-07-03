using RV_Assignment_1.Models;
using RV_Assignment_1.Repositories;

namespace RV_Assignment_1.service
{
    public interface IBulletinPostService
    {
        IQueryable<BulletinPost> GetAll();
        BulletinPost GetById(int id);
        void Add(BulletinPost news);
        void Update(BulletinPost news);
        bool Delete(string id);
    }
    public class BulletinPostService : IBulletinPostService
    {
        private readonly IGenericRepository<BulletinPost> _postRepo;

        public BulletinPostService(IGenericRepository<BulletinPost> postRepo)
        {
            _postRepo = postRepo;
        }

        public IQueryable<BulletinPost> GetAll() => _postRepo.GetAllQueryable();
        public BulletinPost GetById(int id) => _postRepo.Find(id);

        public void Add(BulletinPost news)
        {
            _postRepo.Add(news);
            _postRepo.SaveChanges();
        }

        public void Update(BulletinPost news)
        {
            _postRepo.Update(news);
            _postRepo.SaveChanges();
        }

        public bool Delete(string id)
        {
            var news = _postRepo.Find(id);
            if (news != null)
            {
                _postRepo.Remove(news);
                _postRepo.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
