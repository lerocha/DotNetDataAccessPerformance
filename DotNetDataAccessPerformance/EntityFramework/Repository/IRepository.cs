using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace DotNetDataAccessPerformance.EntityFramework.Repository
{
    /// <summary>
    /// Generic repository interface.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Find();
        IQueryable<T> Find(Expression<Func<T, bool>> predicate);
        T FindOne(EntityKey key);
        void Add(T entity);
        void Remove(T entity);
        void Update(T entity);
    }
}
