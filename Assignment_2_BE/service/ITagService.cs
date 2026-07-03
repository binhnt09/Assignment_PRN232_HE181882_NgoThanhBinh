using Assignment_2_BE.Models;
using Assignment_2_BE.Repositories;

namespace Assignment_2_BE.service
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
