using Assignment_2_BE.DTOs;
using Assignment_2_BE.Models;

namespace Assignment_2_BE.service
{
    public interface ICategoryService
    {
        IQueryable<Category> GetAllCategories();
        CategoryDTO? GetCategoryById(short id);
        CategoryDTO CreateCategory(CategoryDTO categoryDto);
        bool UpdateCategory(short id, CategoryDTO categoryDto);
        bool DeleteCategory(short id);
    }
}
