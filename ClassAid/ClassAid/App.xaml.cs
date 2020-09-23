using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ClassAid.Views;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
namespace ClassAid
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new StartPage());
        }

        protected override void OnStart()
        {
            AppCenter.Start("android=6c9e3d95-2ad6-4960-bdd8-d63483b3a7a2;" +
                  "uwp={Your UWP App secret here};" +
                  "ios={Your iOS App secret here}",
                  typeof(Analytics), typeof(Crashes));
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
