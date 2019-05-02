using System;
using System.Linq;
using System.Linq.Expressions;

namespace App.Library.Persistence
{
    /// <summary>
    ///     Repository interface for entities that a persistence layer must implement.
    /// </summary>
    /// <typeparam name="E">
    ///     The entity type
    /// </typeparam>
    public interface IEntityRepository<E>
    {

        /// <summary>
        ///     All the entities that are stored in the repository. 
        /// </summary>
        IQueryable<E> Entities { get; }


        /// <summary>
        ///     Adds an entity to the repository. This has the 
        /// side effect of setting the entity's identity.
        /// </summary>         
        /// <param name="entity">
        ///     The entity to be added to the repository.
        /// </param>
        void add(E entity);


        /// <summary>
        ///     Permanently removes an entity from the repository. This 
        /// means that the entity will no longer appear in the repositories
        /// Entities collection.
        /// </summary>         
        /// <param name="entity">
        ///     The entity to be removed from the repository.
        /// </param>
        void remove<P>(P entity) where P : class;


        /// <summary>
        /// Update a T type entity
        /// </summary>
        /// <param name="entity">entity of T type</param>  
        void Update(E entity);


        /// <summary>
        /// Find a single entity object of E type using predicated condition
        /// </summary>
        /// <param name="where">predicated condition</param>  
        /// <returns>entity object of E type</returns>
        E GetSingle(Expression<Func<E, bool>> where);
    }
}
