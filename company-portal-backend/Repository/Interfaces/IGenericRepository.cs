using Repository.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task AddAsync(TEntity entity);
        //void Delete(TEntity entity);
        void Update(TEntity entity);
        //Task<TEntity>? GetByIdAsync(int id);
        //Task<PaginatedResult<TEntity>> GetAllAsync(
        //     int? take = 10,
        //     int? skip = 0,
        //     Expression<Func<TEntity, bool>> criteria = null,
        //     Expression<Func<TEntity, object>> orderBy = null,
        //     string orderByDirection = OrderBy.Ascending);

        //Task<int> CountAsync();
        public Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> criteria);
        //public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
        //public Task<List<TEntity>> GetFilteredAsync(Expression<Func<TEntity, bool>> criteria = null);
        public Task<TEntity> FindAsync(
            Expression<Func<TEntity, bool>> criteria,
            Expression<Func<TEntity, object>> orderBy = null,
            string orderByDirection = OrderBy.Ascending);

    }
}
