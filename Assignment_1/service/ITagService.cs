using Assignment_1.Models;
using Assignment_1.Repositories;

namespace Assignment_1.service
{
    public interface ITagService
    {
        IEnumerable<Tag> GetAll();
    }
    public class TagService : ITagService
    {
        private readonly IGenericRepository<Tag> _tagRepo;
        public TagService(IGenericRepository<Tag> tagRepo) => _tagRepo = tagRepo;
        public IEnumerable<Tag> GetAll() => _tagRepo.GetAllQueryable();
    }
}
