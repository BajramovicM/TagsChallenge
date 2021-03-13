using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TagsChallenge.DAL.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);

        IEnumerable<TEntity> Filter(Expression<Func<TEntity, bool>> predicate);
        IEnumerable<TEntity> GetAll();

        void Remove(TEntity entity);
        void RemoveAll(IEnumerable<TEntity> entities);

        
        Task<IEnumerable<TEntity>> FilterAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> GetAllAsync();
    }
}
