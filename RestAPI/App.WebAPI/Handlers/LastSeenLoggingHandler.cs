using System;
using System.Linq;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using App.Services.Connections;
using App.Services.Connections.Update;
using App.Library.DomainHelpers.Analytics;

namespace App.WebAPI.Handlers
{
    //http://stackoverflow.com/questions/13021089/why-is-the-body-of-a-web-api-request-read-once
    public class LastSeenLoggingHandler : DelegatingHandler
    {
        //THIS HANDLER IS QUITE DANGEROUS
        //It has the potential to execute service layer logic for any arbitrary request url (DDOS attacks)
        //Because it is fired befor the controllers get hit, so we need a way to look up the routes table before executing.
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Content.IsMimeMultipartContent())
            {
                //read content into a buffer does not work
                //so skipping 
                //see: http://stackoverflow.com/a/12387967/502130
                //await request.Content.LoadIntoBufferAsync();
            }

            else
            {
                //See below for how to resolve this dependency
                //http://stackoverflow.com/a/14762323/502130

                var dependency_scope = request.GetDependencyScope();

                update_connection_last_seen = dependency_scope.GetService(typeof(IUpdateConnectionLastSeen)) as IUpdateConnectionLastSeen;

                try
                {
                    if (request.Method == HttpMethod.Get)
                    {
                        from_url(request);
                    }
                    else if (request.Method == HttpMethod.Delete)
                    {
                        //Do bloody nothing for now
                    }
                    else
                    {
                        from_url(request);

                        await from_body(request, cancellationToken);
                    }
                }
                catch (Exception ex)
                {
                    new AnalyticsProvider().TrackException(ex);
                }
            }
            return await base.SendAsync(request, cancellationToken);
        }

        private void from_url(HttpRequestMessage request)
        {
            var items =
                        request.GetQueryNameValuePairs().Where(q => q.Key == "connection_id").ToList();

            if (items.Any())
            {
                update_last_seen(items.First().Value);
            }
        }

        private async Task from_body(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var body = await request.Content.ReadAsByteArrayAsync();

            var buffer = new MemoryStream(body);

            var formatters = request.GetConfiguration().Formatters;

            var reader = formatters.FindReader(typeof(ConnectionIdentity), request.Content.Headers.ContentType) ??
                         formatters.FormUrlEncodedFormatter;


            var connection_identity = await reader.ReadFromStreamAsync(typeof(ConnectionIdentity),
                                                                        buffer,
                                                                        request.Content,
                                                                        new DoNothingFormatterLogger(),
                                                                        cancellationToken)
                            as ConnectionIdentity;

            update_last_seen(connection_identity);
        }

        private void update_last_seen(string connection_id)
        {
            if (!string.IsNullOrWhiteSpace(connection_id))
            {
                update_last_seen(new ConnectionIdentity { connection_id = int.Parse(connection_id) });
            }
        }

        private void update_last_seen(ConnectionIdentity connection_id)
        {
            if (connection_id != null)
            {
                update_connection_last_seen.execute(connection_id);
            }
        }


        private IUpdateConnectionLastSeen update_connection_last_seen;
    }

    public class DoNothingFormatterLogger : IFormatterLogger
    {

        public void LogError(string errorPath, Exception exception)
        {
            //do notthing
        }

        public void LogError(string errorPath, string errorMessage)
        {
            //do nothing
        }
    }
}