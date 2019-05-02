using System.Data.Entity;
using App.Library.CodeStructures.Behavioral;

namespace App.Library.Persistence.EF
{
    /// <summary>
    ///     Entity framework implementation of the UnitOfWork
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {

        /// <summary>
        ///     Creates an instance of the Unit of Work setting the context 
        /// that will be used to commit all changes with.
        /// </summary>
        /// <param name="db_context">
        ///     Context that all changes are applied through.
        /// </param>
        public UnitOfWork(DbContext db_context)
        {
            Guard.IsNotNull(db_context, "db_context");

            context = db_context;
        }

        /// <inheritdoc/>
        public void Commit()
        {

            //  DbContext.SaveChanges is committed inside a transaction
            context.SaveChanges();
        }

        private readonly DbContext context;
    }
}
