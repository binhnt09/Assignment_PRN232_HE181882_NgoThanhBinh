using RV_Assignment_1.DAOs;
using RV_Assignment_1.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace RV_Assignment_1.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly CampusBulletinTestContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(CampusBulletinTestContext context)
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
