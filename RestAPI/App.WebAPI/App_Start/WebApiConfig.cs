using App.Library.WebAPI.ExceptionLoggers;
using App.Library.WebAPI.MultipartDataMediaFormatter;
using App.WebAPI.Handlers;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.ExceptionHandling;

namespace App.WebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            configure_cors(config);

            configure_routes(config);

            configure_exception_handling(config);

            configure_custom_handlers(config);

            configure_custom_media_formatters(config);
        }

        private static void configure_cors(HttpConfiguration config)
        {
            //CORS
            //var cors = new EnableCorsAttribute("http://geonow.azurewebsites.net", "*", "*");
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);
        }

        private static void configure_routes(HttpConfiguration config)
        {
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

        private static void configure_exception_handling(HttpConfiguration config)
        {
            //Normal way of Registering ELMAH for webAPI
            // http://blog.elmah.io/logging-to-elmah-io-from-web-api/
            //config.Filters.Add(new ElmahHandleErrorApiAttribute());

            //It seems the above doesn't catch all exceptions due to nature of WebAPI v2
            //See http://www.jasonwatmore.com/post/2014/05/03/Getting-ELMAH-to-catch-ALL-unhandled-exceptions-in-Web-API-21.aspx
            // below is a better solution for WebAPI v2
            //config.Services.Add(typeof(IExceptionLogger), new ElmahExceptionLogger());

            config.Services.Add(typeof(IExceptionLogger), new TraceSourceExceptionLogger());
        }

        private static void configure_custom_handlers(HttpConfiguration config)
        {
            var usage_log_handler = NinjectHttpContainer.Resolve<ApiUsageLoggingHandler>();
            config.MessageHandlers.Add(usage_log_handler);

            var last_seen_handler = new LastSeenLoggingHandler();
            config.MessageHandlers.Add(last_seen_handler);

        }

        private static void configure_custom_media_formatters(HttpConfiguration config)
        {
            config.Formatters.Add(new FormMultipartEncodedMediaTypeFormatter());

            //support text/html media types by returning json
            //this prevents xml from being returned.
            //config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
        }
    }
}
