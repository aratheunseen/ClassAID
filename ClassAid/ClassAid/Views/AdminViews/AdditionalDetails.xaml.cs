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
            admin.BatchDetails = batch;
            admin.ID = stuId.Text;
            admin.Phone = userPhone.Text;
            try
            {
                await AdminDbHandler.UpdateAdmin(App.fireSharpClient.GetClient(), admin);
                Application.Current.MainPage = new NavigationPage(new Dashboard(admin));
                Preferences.Set("isLoggedin", "true");
                Preferences.Set("adminKey", admin.Key);
                LoginAuthModel authModel = new LoginAuthModel()
                {
                    isLoggedIn = true,
                    key = admin.Key,
                    User = UserType.Admin,
                    AdminData = admin
                };
            }
            catch (Exception)
            {
                resultText.Text = "Something bad happened. Please check back in a short.";
            }         
        }

        private void Form_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (userPhone.Text.Length < 6 ||
                userRealName.Text.Length < 6 ||
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