using System;

using Xamarin;
using System.Collections;
using System.Collections.Generic;

#if __ANDROID__
using Android.Content;
#endif

namespace App.Common
{
    public class Analytics : SingletonBase<Analytics>
    {
        private Analytics(){}

        bool initialized;

        #if __ANDROID__
        public void Initialize (Context context)
        {
            if (!initialized) {
                try {
                    Insights.Initialize (Resources.XAMARIN_INSIGHTS_API_KEY, context);
                } catch {}
            }
            initialized = true;
        }

        #elif __IOS__
        public void Initialize ()
        {
            if (!initialized) {
                try {
                    Insights.Initialize (Resources.XAMARIN_INSIGHTS_API_KEY);
                } catch {}
            }
            initialized = true;
        }
        #endif
        public void Track(string track_id)
        {
            Insights.Track(track_id);
        }

        public void Track(string track_id, Dictionary<string, string> data)
        {
            Insights.Track(track_id, data);
        }

        public void Track(string track_id, string key, string value)
        {
            Insights.Track(track_id, key, value);
        }

        public void Report(Exception e)
        {
            Insights.Report(e);
        }

        public void Report(Exception e, IDictionary data)
        {
            Insights.Report(e, data);
        }

        public void Report(Exception e, string key, string value)
        {
            Insights.Report(e, key, value);
        }

        public IDisposable TrackTime(string track_id)
        {
            return Insights.TrackTime(track_id);
        }

        public void LogException (string tag, Exception e)
        {
            try {
                Insights.Report (e, "Tag", tag);
            } catch {}
        }
    }
}

