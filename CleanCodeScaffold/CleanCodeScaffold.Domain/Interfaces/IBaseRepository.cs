using CleanCodeScaffold.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CleanCodeScaffold.Domain.Interfaces
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(long id);
        Task<T> CreateAsync(T model);
        Task<T> UpdateAsync(T model);
        Task<bool> DeleteAsync(long id);
        IQueryable<T> GetAllAsync();
        IQueryable<T> GetAllAsync(Expression<Func<T, bool>> predicate);
    }
}
