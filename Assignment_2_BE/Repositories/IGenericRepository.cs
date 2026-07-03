using System;
using System.Linq;

namespace Assignment_2_BE.Repositories
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
