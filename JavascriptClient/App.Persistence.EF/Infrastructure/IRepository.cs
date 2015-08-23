using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace App.Persistence.EF.Infrastructure
{
    public interface IRepository<T> : IDisposable where T : class
    {
        /// <summary>
        /// Add a T type entity
        /// </summary>
        /// <param name="entity">entity of T type</param>        
        void Add(T entity);

        /// <summary>
        /// Update a T type entity
        /// </summary>
        /// <param name="entity">entity of T type</param>  
        void Update(T entity);

        /// <summary>
        /// Delete a T type entity
        /// </summary>
        /// <param name="entity">entity of T type</param>  
        void Delete<E> ( E entity ) where E : class;

        ///// <summary>
        ///// Save changes made to a T type entity
        ///// </summary>
        //void SaveChanges();

        /// <summary>
        /// Find a single entity object of T type using predicated condition
        /// </summary>
        /// <param name="where">predicated condition</param>  
        /// <returns>entity object of T type</returns>
        T GetSingle(Expression<Func<T, bool>> where);

        /// <summary>
        /// Find a list of all entity objects of T type in the repository
        /// </summary> 
        /// <returns>Returns a list object of T type</returns>
        IList<T> GetAll();

        /// <summary>
        /// Find a list of entity objects of T type in the repository using predicated condition
        /// </summary>
        /// <param name="where">predicated condition</param>  
        /// <returns>Returns a list object of T type</returns>
        IQueryable<T> GetMany(Expression<Func<T, bool>> where);

        /// <summary>
        /// Provides queryable access to the entities in the repository.
        /// </summary>
        IQueryable<T> Entries { get; }
    }
}
