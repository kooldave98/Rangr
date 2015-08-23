using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace App.WebAPI
{
    /// <summary>
    /// The Web API configuration class
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// Registers the routes of entry into the API action methods
        /// </summary>
        /// <param name="config"></param>
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}/{format}",
                defaults: new { id = RouteParameter.Optional, format = RouteParameter.Optional }
            );

            // Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable<T> return type.
            // To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
            // For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
            //config.EnableQuerySupport();

            // To disable tracing in your application, please comment out or remove the following line of code
            // For more information, refer to: http://www.asp.net/web-api
            config.EnableSystemDiagnosticsTracing();
        }
    }
}
