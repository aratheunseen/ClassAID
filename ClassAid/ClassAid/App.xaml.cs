using System;
using Xamarin.Forms;
using ClassAid.Views;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System.IO;
using Xamarin.Essentials;
using ClassAid.DataContex;

namespace ClassAid
{
    public partial class App : Application
    {
        internal static FireSharpDB fireSharpClient;
        internal static string authFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ClassAiD_Auth.nsdl");

        public App()
        {
            string server = "https://classaidapp.firebaseio.com/";
            string authKey = "q4ckBo2jl1p2EB0qg9eTnAwXwPKYwt2DbcSCOc5V";
            fireSharpClient = new FireSharpDB(server, authKey);
            InitializeComponent();
            string  loginState = Preferences.Get("isLoggedin", "false");
            if (loginState == "false")
                MainPage = new NavigationPage(new StartPage());
            else
                MainPage = new NavigationPage(new Dashboard());
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
