using Assignment_2_BE.Repositories;
using Assignment_2_BE.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Assignment_2_BE.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly FunewsManagementContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(FunewsManagementContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }
        public IQueryable<T> GetAllQueryable() => _dbSet.AsQueryable();
        public T Find(params object[] keyValues) => _dbSet.Find(keyValues);

        public void Add(T entity) => _dbSet.Add(entity);

        public void Update(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Remove(T entity) => _dbSet.Remove(entity);

        public void SaveChanges() => _context.SaveChanges();
    }
}
