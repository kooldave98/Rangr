using System;
using System.Collections.Generic;

namespace App.Core.Portable.Infrastructure
{
    public interface IRepository<T> where T : class
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
        /// Find a single entity object of T type using predicated condition
        /// </summary>
        /// <param name="where">predicated condition</param>  
        /// <returns>entity object of T type</returns>
        T GetSingle(Func<T, bool> where);

        /// <summary>
        /// Find a list of entity objects of T type in the repository using predicated condition
        /// </summary>
        /// <param name="where">predicated condition</param>  
        /// <returns>Returns a list object of T type</returns>
        IEnumerable<T> GetMany(Func<T, bool> where);

        /// <summary>
        /// Provides queryable access to the entities in the repository.
        /// </summary>
        IEnumerable<T> Entries { get; }
    }
}
