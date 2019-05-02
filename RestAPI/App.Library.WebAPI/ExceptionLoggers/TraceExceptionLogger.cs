using System.Diagnostics;
using System.Web.Http.ExceptionHandling;

namespace App.Library.WebAPI.ExceptionLoggers
{
    public class TraceSourceExceptionLogger : ExceptionLogger
    {
        private readonly TraceSource _traceSource;

        public TraceSourceExceptionLogger()
        {
            _traceSource = new TraceSource("MyTraceSource", SourceLevels.All);
        }

        public override void Log(ExceptionLoggerContext context)
        {
            _traceSource.TraceEvent(TraceEventType.Error, 1,
                "Unhandled exception processing {0} for {1}: {2}",
                context.Request.Method,
                context.Request.RequestUri,
                context.Exception);

            Debug.WriteLine("Unhandled exception processing {0} for {1}: {2}",
                            context.Request.Method,
                            context.Request.RequestUri,
                            context.Exception);
        }
    }
}
