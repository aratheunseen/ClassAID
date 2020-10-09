using System;
using Xamarin.Forms;
using ClassAid.Views;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Xamarin.Essentials;
using ClassAid.DataContex;
using ClassAid.Models;
using ClassAid.Models.Users;
using System.Threading.Tasks;

namespace ClassAid
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            bool loginState = Preferences.Get(PrefKeys.IsLoggedIn, false);
            var current = Connectivity.NetworkAccess;
            Connectivity.ConnectivityChanged += CheckConnection;

            if (!loginState && current == NetworkAccess.Internet)
            {
                MainPage = new NavigationPage(new StartPage());
                if (current != NetworkAccess.Internet)
                    DependencyService.Get<Toast>().Show("No INTERNET connection.");
            }
            else
            {
                MainPage = new NavigationPage(new Dashboard());
            }
        }

        private void CheckConnection(object sender, ConnectivityChangedEventArgs e)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                if (Preferences.Get(PrefKeys.IsSyncPending, false))
                {
                    try
                    {
                        Admin admin = LocalStorageEngine.ReadDataAsync<Admin>
                      (FileType.Admin);
                        FirebaseHandler.UpdateAdmin(admin);
                        DependencyService.Get<Toast>().Show("Synced successfully.");
                        Preferences.Set(PrefKeys.IsSyncPending, true);
                    }
                    catch (Exception)
                    {
                        DependencyService.Get<Toast>().Show("Sync failed. ERROR code PCAiDx06");
                        return;
                    }
                }
            }
            else
                return;
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
        public static void LogOut()
        {
            Preferences.Set(PrefKeys.IsLoggedIn, false);
            Application.Current.MainPage =
                new NavigationPage(new StartPage());
            LocalDbContex.DropTables();
        }
    }
}
