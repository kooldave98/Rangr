using App.Library.EntityFramework.Contexts.ConnectionStringProviders;

namespace App.Persistence.EF.Infrastructure
{

    public class AppConnectionStringProvider
                    : IConnectionStringProvider
    {

        public string connection_string
        {
            get { return "name=RangrAppDBConnection"; }//EF DbContext will pick this up from the Web.Config file
            //get { return @"Data Source=.\SQLEXPRESS;Initial Catalog=WORKSuite5_Experimental;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework"; }
            //get { return @"Data Source=localhost;Initial Catalog=WORKSuite5_Experimental;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework"; }
        }


    }

}