using ClassAid.DataContex;
using System;
using ClassAid.Models.Users;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.Diagnostics;

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
            TapCommand = new Command(() => btnAdd_Clicked());
        }
        private async void btnAdd_Clicked()
        {
            try
            {
                Admin admin = new Admin(userName.Text, userPass.Text);
                activityIndicator.IsRunning = true;
                var tempAdmin =
                    await AdminDbHandler.GetAdmin(
                        App.fireSharpClient.GetClient(), admin.Key);
                if (tempAdmin == null)
                {
                    activityIndicator.IsRunning = false;
                    await Navigation.PushAsync(
                        new AdditionalDetails(admin));
                    await AdminDbHandler.InsertData(
                        App.fireSharpClient.GetClient(), admin);
                }
                else
                {
                    Debug.WriteLine(tempAdmin.Name);
                    activityIndicator.IsRunning = false;
                    Application.Current.MainPage = 
                        new NavigationPage(new Dashboard(tempAdmin));
                    Preferences.Set("isLoggedin", "true");
                    Preferences.Set("adminKey", tempAdmin.Key);
                }
            }
            catch (Exception e)
            {
                resultText.Text = "Sorry something bad happened. " + e.Message;
            }

        }
        private void form_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(userName.Text) ||
                string.IsNullOrWhiteSpace(userPass.Text))

            {
                signInBtn.Command = null;
            }
            else
                signInBtn.Command = TapCommand;
        }

        private void adminLoginBypassBtn_Clicked(object sender, EventArgs e)
        {
            Admin ad = new Admin("HolaSenorita", "ILoveYou");
            Navigation.PushAsync(new AdditionalDetails(ad));
        }
    }
}