using Assignment_2_BE.DTOs;
using Assignment_2_BE.Models;
using Assignment_2_BE.Repositories;
using AutoMapper;

namespace Assignment_2_BE.service
{
    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepository<Category> _categoryRepo;
        private readonly IGenericRepository<NewsArticle> _newsRepo;
        private readonly IMapper _mapper;

        public CategoryService(IGenericRepository<Category> categoryRepo, IGenericRepository<NewsArticle> newsRepo, IMapper mapper)
        {
            _categoryRepo = categoryRepo;
            _newsRepo = newsRepo;
            _mapper = mapper;
        }

        public IQueryable<Category> GetAllCategories()
        {
            return _categoryRepo.GetAllQueryable();
        }

        public CategoryDTO? GetCategoryById(short id)
        {
            var category = _categoryRepo.Find(id);
            return category == null ? null : _mapper.Map<CategoryDTO>(category);
        }

        public CategoryDTO CreateCategory(CategoryDTO categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            _categoryRepo.Add(category);
            _categoryRepo.SaveChanges();
            return _mapper.Map<CategoryDTO>(category);
        }

        public bool UpdateCategory(short id, CategoryDTO categoryDto)
        {
            var existing = _categoryRepo.Find(id);
            if (existing == null) return false;

            _mapper.Map(categoryDto, existing);
            existing.CategoryId = id; // Ensure ID doesn't change
            
            _categoryRepo.Update(existing);
            _categoryRepo.SaveChanges();
            return true;
        }

        public bool DeleteCategory(short id)
        {
            // Requirement: Cannot delete if it belongs to any news article
            var hasNews = _newsRepo.GetAllQueryable().Any(n => n.CategoryId == id);
            if (hasNews) return false;

            var category = _categoryRepo.Find(id);
            if (category == null) return false;

            _categoryRepo.Remove(category);
            _categoryRepo.SaveChanges();
            return true;
        }
    }
}
