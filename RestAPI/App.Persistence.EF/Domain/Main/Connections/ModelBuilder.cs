

using App.Library.EntityFramework.Configuration;
using App.Persistence.Main;

namespace App.Persistence.EF.Domain.Main.Connections
{
    public class ModelBuilder : ModelConfiguration<Connection>
    {

        public ModelBuilder(string schema)
        {

            Map(m => m.ToTable("Connection", schema));
        }
    }
}
