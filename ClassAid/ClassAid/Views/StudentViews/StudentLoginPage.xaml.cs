using ClassAid.DataContex;
using ClassAid.Models.Users;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClassAid.Views.StudentViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StudentLoginPage : ContentPage
    {
        public static Command TapCommand;
        public StudentLoginPage()
        {
            InitializeComponent();
            TapCommand = new Command(() => ProceedBtn());
        }

        private void Form_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(userName.Text) ||
                string.IsNullOrWhiteSpace(userPass.Text) ||
                userName.Text.Length < 6 ||
                userPass.Text.Length < 6)

            {
                studentSignIn.Command = null;
            }
            else
                studentSignIn.Command = TapCommand;
        }
        private async void ProceedBtn()
        {
            try
            {
                Shared user = new Shared(userName.Text + "student", userPass.Text)
                {
                    IsAdmin = false
                };
                activityIndicator.IsRunning = true;
                var tempAdmin =
                    await FirebaseHandler.GetUser(user.Key, user.IsAdmin);
                if (tempAdmin == null)
                {
                    activityIndicator.IsRunning = false;
                    await Navigation.PushAsync(
                        new AdditionalDetails(user));
                    await FirebaseHandler.InsertData(user);
                }
                else
                {
                    activityIndicator.IsRunning = false;
                    Application.Current.MainPage =
                        new NavigationPage(new Dashboard(tempAdmin));
                    Preferences.Set(PrefKeys.isLoggedIn, true);
                    Preferences.Set(PrefKeys.adminKey, tempAdmin.TeamCode);
                }
            }
            catch (Exception e)
            {
                resultText.Text = "Sorry something bad happened. " + e.Message;
            }

        }

        private void userName_Focused(object sender, FocusEventArgs e)
        {

        }

        private void userPass_Unfocused(object sender, FocusEventArgs e)
        {

        }
    }
}