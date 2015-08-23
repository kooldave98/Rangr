using App.Persistence.EF.Infrastructure;

namespace App.Integration.Configuration
{
    public class WebConnectionSettings : IConnectionSettings
    {

        public string ConnectionString
        {
            get { return @"Data Source=.\SQLEXPRESS;Initial Catalog=GeoNowDB;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework"; }
            //get { return @"Data Source=localhost;Initial Catalog=AppDB;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework"; }
        }

    }
}
