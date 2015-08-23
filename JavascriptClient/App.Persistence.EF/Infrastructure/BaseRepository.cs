using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Linq.Expressions;


namespace App.Persistence.EF.Infrastructure
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        private EntityDbContext _context;

        /// <inheritdoc/>
        public BaseRepository(IDatabaseFactory databaseFactory)
        {
            DatabaseFactory = databaseFactory;
        }

        /// <inheritdoc/>
        public IDatabaseFactory DatabaseFactory
        {
            get;
            private set;
        }

        /// <inheritdoc/>
        public EntityDbContext DataContext
        {
            get
            {
                return _context ?? (_context = DatabaseFactory.Get());
            }
        }

        /// <inheritdoc/>
        public virtual IList<T> GetAll()
        {
            return DataContext.Set<T>().ToList();
        }

        /// <inheritdoc/>
        public virtual void Add(T entity)
        {
            DataContext.Set<T>().Add(entity);
        }

        /// <inheritdoc/>
        public virtual void Update(T entity)
        {
            DataContext.Set<T>().Attach(entity);
            DataContext.Entry(entity).State = EntityState.Modified;

        }

        /// <inheritdoc/>
        public virtual void Delete<E>(E entity) where E : class
        {
            DataContext.Set<E>().Attach(entity);
            DataContext.Entry(entity).State = EntityState.Deleted;
            DataContext.Set<E>().Remove(entity);

        }

        /// <inheritdoc/>
        public T GetSingle(Expression<Func<T, bool>> predicate)
        {
            return DataContext.Set<T>().Where(predicate).FirstOrDefault<T>();
        }

        /// <inheritdoc/>
        public IQueryable<T> GetMany(Expression<Func<T, bool>> predicate)
        {
            return DataContext.Set<T>().Where(predicate);
        }

        ///Use RepositorySet.CommitChanges Instead
        /// <inheritdoc/>
        //public void SaveChanges()
        //{
        //    // May need Unit of Work some where?
        //    DataContext.SaveChanges();
        //}

        /// <inheritdoc/>
        public void Dispose()
        {
            if (DataContext != null)
            {
                DataContext.Dispose();
            }
        }


        /// <inheritdoc/>
        public IQueryable<T> Entries {

            get {
                return DataContext.Set<T>();
            }
        }
    }
}
