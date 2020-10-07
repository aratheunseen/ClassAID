using System;
using Xamarin.Forms;
using ClassAid.Views;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System.IO;
using Xamarin.Essentials;
using ClassAid.DataContex;
using ClassAid.Models;
using ClassAid.Models.Users;

namespace ClassAid
{
    public partial class App : Application
    {
        internal static string authFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ClassAiD_Auth.nsdl");
        public App()
        {
            InitializeComponent();
            bool loginState = Preferences.Get(PrefKeys.IsLoggedIn, false);
            var current = Connectivity.NetworkAccess;
            Connectivity.ConnectivityChanged += CheckConnection;
            if (!loginState && current == NetworkAccess.Internet)
                MainPage = new NavigationPage(new StartPage());
            else if (!loginState)
            {
                DependencyService.Get<Toast>().Show("No INTERNET connection.");
                MainPage = new NavigationPage(new StartPage());
            }
            else
            {
                Admin user = new Admin();
                try
                {
                  Action action = new Action(async ()=> 
                  user = LocalStorageEngine.ReadDataAsync<Admin>
                        (FileType.Shared));
                    action.Invoke();
                    MainPage = new NavigationPage(new Dashboard(user));
                }
                catch (Exception)
                {
                    if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                    {
                        string key = Preferences.Get("adminKey", "");
                        Action action = new Action(async()=> 
                        user = await FirebaseHandler.GetUser(key, user.IsAdmin));
                        action.Invoke();
                    }
                    else
                    {
                        DependencyService.Get<Toast>().Show("ERROR. Please connect to Internet to resolve the issue.");
                        MainPage = new StartPage();
                        return;
                    }
                }
            }
        }

        private async void CheckConnection(object sender, ConnectivityChangedEventArgs e)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                if (Preferences.Get(PrefKeys.IsSyncPending, false))
                {
                    try
                    {
                        Admin admin = LocalStorageEngine.ReadDataAsync<Admin>
                      (FileType.Shared);
                        await FirebaseHandler.UpdateUser(admin);
                        DependencyService.Get<Toast>().Show("Synced successfully.");
                        Preferences.Set(PrefKeys.IsSyncPending, true);
                    }
                    catch (Exception ex)
                    {
                        DependencyService.Get<Toast>().Show("Something Went wrong. " + ex.Message);
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
    }
}
