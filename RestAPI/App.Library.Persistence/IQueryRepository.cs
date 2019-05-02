using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Library.Persistence
{
    public interface IQueryRepository<E>
    {
        /// <summary>
        ///     All the entities that are stored in the repository. 
        /// </summary>
        IQueryable<E> Entities { get; }
    }
}
