using System.Data.Entity;
using System.Linq;
using App.Library.CodeStructures.Behavioral;

namespace App.Library.Persistence.EF
{
    /// <summary>
    ///     Entity framework implementation of the Repository
    /// </summary>
    /// <typeparam name="E">
    ///     Entity type the repository is for.
    /// </typeparam>
    public class QueryRepository<E> : IQueryRepository<E>
        where E : class
    {

        public QueryRepository(DbContext db_context)
        {

            Guard.IsNotNull(db_context, "db_context");

            context = db_context;
        }

        public IQueryable<E> Entities { get { return context.Set<E>().AsNoTracking(); } }

        private readonly DbContext context;
    }
}
