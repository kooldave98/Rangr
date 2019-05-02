using System;
using App.Library.CodeStructures.Behavioral;
using App.Library.WebAPI.Handlers;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using App.WebAPI.Infrastructure.UsageLogs;

namespace App.WebAPI.Handlers
{
    /// <summary>
    /// A better implementation of WebAPI DelegatingHandler exists here->
    /// http://weblogs.asp.net/pglavich/asp-net-web-api-request-response-usage-logging
    /// TODO: Improve this implementation with above url
    /// Also, see: http://www.asp.net/web-api/overview/advanced/http-message-handlers
    /// for how to possibly implement API key authentication
    /// </summary>
    public class ApiUsageLoggingHandler : MessageHandler
    {
        protected override async Task IncommingMessageAsync(string correlationId, string requestInfo, byte[] message)
        {

            await Task.Run(() =>
            {
                var log = string.Format("{0} - Request: {1}\r\n{2}", correlationId, requestInfo, Encoding.UTF8.GetString(message));

                Debug.WriteLine(log);

                usage_log_repository.requests.Add(new Log() { log_summary = log, log_time = DateTime.UtcNow });
            });
        }


        protected override async Task OutgoingMessageAsync(string correlationId, string requestInfo, byte[] message)
        {
            await Task.Run(() =>
            {
                var log = string.Format("{0} - Response: {1}\r\n{2}", correlationId, requestInfo, Encoding.UTF8.GetString(message));

                Debug.WriteLine(log);

                usage_log_repository.responses.Add(new Log() { log_summary = log, log_time = DateTime.UtcNow });
            });
        }

        public ApiUsageLoggingHandler(UsageLogRepository the_usage_log_repository)
        {
            usage_log_repository = Guard.IsNotNull(the_usage_log_repository, "the_usage_log_repository");
        }

        private readonly UsageLogRepository usage_log_repository;
    }
}