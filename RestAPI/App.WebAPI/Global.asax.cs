using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using App.Library.Azure;

namespace App.WebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            SqlServerTypes.Utilities.LoadNativeAssemblies(Server.MapPath("~/bin"));

            //The order of these is very important...
            //Obviously, ninject has to be setup first, i think

            AreaRegistration.RegisterAllAreas();

            NinjectHttpContainer.RegisterModules(NinjectHttpModules.Modules);

            GlobalConfiguration.Configure(WebApiConfig.Register);

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //Change to singleton static initialiser
            new StorageInitializer().InitializeStorage();
        }

        
    }
}
