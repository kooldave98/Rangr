using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.Persistence.EF;
using App.Persistence.Models;

namespace App.Persistence.EF.Infrastructure
{
    public interface IDatabaseFactory : IDisposable
    {
        EntityDbContext Get();
    }
}
