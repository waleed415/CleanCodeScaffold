using CleanCodeScaffold.Domain.Entities;
using CleanCodeScaffold.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CleanCodeScaffold.Infrastructure.Persistence.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        private readonly AppDBContext _dbContext;
        private readonly DbSet<T> _dbSet;
        public BaseRepository(AppDBContext dBContext)
        {
            _dbContext = dBContext;
            _dbSet = dBContext.Set<T>();
        }
        public async Task<T> CreateAsync(T model)
        {
            await _dbSet.AddAsync(model);
            await _dbContext.SaveChangesAsync();
            return model;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var model = await GetByIdAsync(id);
            if (model?.Id > 0)
            {
                model.IsActive = false;
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;

        }
        public IQueryable<T> GetAllAsync()
        {
            return _dbSet.Where(x => x.IsActive);
        }
        public IQueryable<T> GetAllAsync(Expression<Func<T, bool>> predicate)
        {
            return GetAllAsync().Where(predicate);
        }

        public async Task<T> GetByIdAsync(long id) => await _dbSet.FindAsync(id);

        public async Task<T> UpdateAsync(T model)
        {
            _dbSet.Update(model);
            await _dbContext.SaveChangesAsync();
            return model;
        }
    }
}
