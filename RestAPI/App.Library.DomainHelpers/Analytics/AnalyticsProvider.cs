using App.Library.CodeStructures.Behavioral;
using System;

namespace App.Library.DomainHelpers.Analytics
{
    /// <summary>
    /// A facade over 'TelemetryClient'.
    /// So we can change it in the future if we need to.
    /// </summary>
    public class AnalyticsProvider
    {
        public void TrackException(Exception exception)
        {
            Guard.IsNotNull(exception, "exception");


            //telemetry_client.TrackException(exception);
        }

        public AnalyticsProvider()
        {
            //telemetry_client = new TelemetryClient();
        }

        //private TelemetryClient telemetry_client;
    }
}
