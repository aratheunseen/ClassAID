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
        Admin admin;
        public AdditionalDetails(Admin admin)
        {
            InitializeComponent();
            this.admin = admin;
        }

        private async void CompleteSignUp_Clicked()
        {
            admin.Name = userRealName.Text;
            admin.Phone = userPhone.Text;
            BatchDetails batch = new BatchDetails()
            {
                University = instName.Text,
                Department = deptName.Text,
                Semester = semName.Text,
                Section = secName.Text
            };
            resultText.Text = "Creating Unique ID";
            admin.TeamCode = await FirebaseHandler.GetTeamCode(instName.Text,admin.Key);
            resultText.Text = "Creating Profile";
            admin.BatchDetails = batch;
            admin.ID = stuId.Text;
            admin.Phone = userPhone.Text;
            try
            {
                Preferences.Set(PrefKeys.IsLoggedIn, true);
                Preferences.Set(PrefKeys.AdminKey, admin.Key);
                Preferences.Set(PrefKeys.Key, admin.Key);
                Preferences.Set(PrefKeys.IsAdmin, true);
                FirebaseHandler.UpdateAdmin(admin);
                Application.Current.MainPage = new NavigationPage(new Dashboard(admin));
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

        protected override void OnAppearing()
        {
            base.OnAppearing();
            userRealName.Focus();
        }
    }
}