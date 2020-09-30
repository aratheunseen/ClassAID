using ClassAid.DataContex;
using ClassAid.Models.Engines;
using ClassAid.Models.Schedule;
using ClassAid.Models.Users;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClassAid.Views.AdminViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdditionalDetails : ContentPage
    {
        Shared user;
        public AdditionalDetails(Shared admin)
        {
            InitializeComponent();
            this.user = admin;
        }

        private async void CompleteSignUp_Clicked()
        {
            user.Name = userRealName.Text;
            user.Phone = userPhone.Text;
            BatchDetails batch = new BatchDetails()
            {
                University = instName.Text,
                Department = deptName.Text,
                Semester = semName.Text,
                Section = secName.Text
            };
            user.BatchDetails = batch;
            user.ID = stuId.Text;
            user.Phone = userPhone.Text;
            try
            {
                await FirebaseHandler.UpdateAdmin(user);
                Application.Current.MainPage = new NavigationPage(new Dashboard(user));
                Preferences.Set(PrefKeys.isLoggedIn, true);
                Preferences.Set(PrefKeys.adminKey, user.Key);
            }
            catch (Exception)
            {
                resultText.Text = "Something bad happened. Please check back in a short.";
            }         
        }

        private void Form_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(userPhone.Text) ||
                string.IsNullOrWhiteSpace(userRealName.Text) || 
                string.IsNullOrWhiteSpace(instName.Text) ||
                string.IsNullOrWhiteSpace(deptName.Text) ||
                string.IsNullOrWhiteSpace(secName.Text) ||
                string.IsNullOrWhiteSpace(secName.Text) ||
                string.IsNullOrWhiteSpace(stuId.Text))
            {
                contToDashBtn.Command = null;
            }
            else
            {
                contToDashBtn.Command = new Command(() => CompleteSignUp_Clicked());
            }
        }
    }
}