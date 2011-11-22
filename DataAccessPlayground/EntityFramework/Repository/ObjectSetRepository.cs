using System;
using System.Data;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Linq.Expressions;

namespace DataAccessPlayground.EntityFramework.Repository
{
    /// <summary>
    /// Repository implementation using an ObjectSet (EF 4).
    /// </summary>
    /// <typeparam name="T">EntityObject type</typeparam>
    public class ObjectSetRepository<T> : IRepository<T> where T : EntityObject
    {
        private readonly IObjectSet<T> _objectSet;

        public ObjectSetRepository(IObjectSet<T> objectSet)
        {
            _objectSet = objectSet;
        }
 
        #region Implementation of IRepository<T>

        public IQueryable<T> Find()
        {
            return _objectSet;
        }

        public IQueryable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _objectSet.Where(predicate);
        }

        public T FindOne(EntityKey key)
        {
            return _objectSet.Where(o => o.EntityKey == key).FirstOrDefault();
        }

        public void Add(T entity)
        {
            _objectSet.AddObject(entity);
        }

        public void Remove(T entity)
        {
            _objectSet.DeleteObject(entity);
        }

        public void Update(T entity)
        {
            var original = _objectSet.Where(o => o.EntityKey == entity.EntityKey).FirstOrDefault();
            if (original!=null)
            {
                // TODO: set changes.   
            }
        }

        #endregion
    }
}
