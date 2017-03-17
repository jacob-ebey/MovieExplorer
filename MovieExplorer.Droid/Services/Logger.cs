using System;
using MovieExplorer.Services;
using Microsoft.Azure.Mobile.Analytics;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using Microsoft.Azure.Mobile.Crashes;
using MvvmCross.Platform;
using System.Threading.Tasks;
using Microsoft.Azure.Mobile;

namespace MovieExplorer.Droid.Services
{
    class Logger : ILogger
    {
        bool _alreadySentLastException;

        public async void Init()
        {
            MobileCenter.Start("e709b5ec-fb3b-434c-a1c9-2e7e18decca1",
                   typeof(Analytics), typeof(Crashes));

            Crashes.ShouldAwaitUserConfirmation = () => true;

            if (Crashes.HasCrashedInLastSession && !_alreadySentLastException)
            {
                _alreadySentLastException = true;
                Mvx.Resolve<IToastService>().ShowToast(
                    "Sorry about that crash, our top engineers are on it!",
                    ToastDuration.Long);

                await Task.Run(async () =>
                {
                    var report = await Crashes.GetLastSessionCrashReportAsync();
                    Mvx.Resolve<ILogger>().LogException(report.Exception);
                });
            }
        }

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