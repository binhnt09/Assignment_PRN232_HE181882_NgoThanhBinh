using Assignment_1.Models;

namespace Assignment_1.service
{
    public interface ICategoryService
    {
        IQueryable<Category> GetAll();
        Category GetById(short id);
        void Add(Category category);
        void Update(Category category);
        bool Delete(short id);
    }
}
