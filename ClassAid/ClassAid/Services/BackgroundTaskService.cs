using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using ClassAid.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xamarin.Forms;

namespace ClassAid.Services
{
    [Service]
    public class BackgroundTaskService : Service
    {
        public override IBinder OnBind(Intent intent)
        {
            throw new NotImplementedException();
        }
        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            var t = new Thread(() =>
            {
                DependencyService.Get<Toast>().Show("Doing work");
                Thread.Sleep(5000);
                DependencyService.Get<Toast>().Show("Work complete");
                StopSelf();
            }
        );
            t.Start();
            return base.OnStartCommand(intent, flags, startId);
        }
    }
}
