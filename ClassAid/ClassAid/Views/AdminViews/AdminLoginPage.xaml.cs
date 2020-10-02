using ClassAid.DataContex;
using System;
using ClassAid.Models.Users;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.Diagnostics;
using ClassAid.Models;

namespace ClassAid.Views.AdminViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminLoginPage : ContentPage
    {
        public static Command TapCommand;
        public static bool isCliclable = false;
        public AdminLoginPage()
        {
            InitializeComponent();
            //privacyURI.Command = new Command(async () =>
            //await Launcher.OpenAsync(new Uri("https://mahmudx.com")));
            TapCommand = new Command(() => BtnAdd_Clicked());
        }
        private async void BtnAdd_Clicked()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                Shared user = new Shared(userName.Text + "admin", userPass.Text);
                user.IsAdmin = true;
                activityIndicator.IsRunning = true;
                var tempAdmin =
                    await FirebaseHandler.GetAdmin(user.Key, user.IsAdmin);
                if (tempAdmin == null)
                {
                    activityIndicator.IsRunning = false;
                    await Navigation.PushAsync(
                        new AdditionalDetails(user));
                    await FirebaseHandler.InsertData(user);
                }
                else
                {
                    Debug.WriteLine(tempAdmin.Name);
                    activityIndicator.IsRunning = false;
                    Application.Current.MainPage =
                        new NavigationPage(new Dashboard(tempAdmin));
                    Preferences.Set(PrefKeys.isLoggedIn, true);
                    Preferences.Set(PrefKeys.adminKey, tempAdmin.Key);
                }
            }
            else
            {
                DependencyService.Get<Toast>().Show("No INTERNET connection.");
            }
            ////}
            ////catch (Exception e)
            ////{
            ////    resultText.Text = "Sorry something bad happened. " + e.Message;
            ////}

        }
        private void Form_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(userName.Text) ||
                string.IsNullOrWhiteSpace(userPass.Text)||
                userName.Text.Length < 6 ||
                userPass.Text.Length < 6 )

            {
                signInBtn.Command = null;
            }
            else
                signInBtn.Command = TapCommand;
        }
    }
}