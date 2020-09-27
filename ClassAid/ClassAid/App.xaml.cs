using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ClassAid.Views;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System.IO;
using Newtonsoft.Json;
using ClassAid.Models.Engines;
using ClassAid.Views.AdminViews;
using Xamarin.Essentials;
using ClassAid.DataContex;
using ClassAid.Models.Users;
using ClassAid.Views.StudentViews;

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

            bool doesExist = File.Exists(authFile);
            var current = Connectivity.NetworkAccess;
            
            if (doesExist)
            {
                string text = File.ReadAllText(authFile);
                var loginInfo = JsonConvert.DeserializeObject<LoginAuthModel>(text);
                if (loginInfo.User == UserType.Admin)
                {
                    MainPage = new NavigationPage(new DashBoardPage(loginInfo));
                }
                else if (loginInfo.User == UserType.Student)
                {
                    MainPage = new NavigationPage(new StudentDashBoard());
                }
                File.Delete(authFile);
            }
            else
            {
                MainPage = new NavigationPage(new StartPage());
            }
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
