using RV_Assignment_1.Models;

namespace RV_Assignment_1.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAllQueryable();
        T Find(params object[] keyValues);
        void Add(T entity);
        void Update(T entity);
        void Remove(T entity);
        void SaveChanges();
    }
}
