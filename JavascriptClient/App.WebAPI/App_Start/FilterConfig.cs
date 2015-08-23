using System.Web;
using System.Web.Mvc;

namespace App.WebAPI
{
    /// <summary>
    /// A filters configuration file
    /// </summary>
    public class FilterConfig
    {
        /// <summary>
        /// Registers the filters globally across every action
        /// </summary>
        /// <param name="filters"></param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}