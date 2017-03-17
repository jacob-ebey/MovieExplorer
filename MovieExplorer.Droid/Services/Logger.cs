using System;
using MovieExplorer.Services;
using Microsoft.Azure.Mobile.Analytics;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace MovieExplorer.Droid.Services
{
    class Logger : ILogger
    {
        public void LogException(Exception e, [CallerMemberName] string callerMemberName = null)
        {
            Dictionary<string, string> props = new Dictionary<string, string>
            {
                { "Message", e.Message ?? "Unknown" },
                { "StackTrace", e.StackTrace ?? "Unknown" }
            };

            Analytics.TrackEvent("Exception_" + callerMemberName ?? "Unknown", props);
        }
    }
}