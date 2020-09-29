using ClassAid.DataContex;
using ClassAid.Models.Engines;
using ClassAid.Models.Schedule;
using ClassAid.Models.Users;
using Firebase.Database;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
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
            contToDashBtn.Command = new Command(() => completeSignUp_Clicked());
        }

        private async void completeSignUp_Clicked()
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
                Application.Current.MainPage = new NavigationPage(new DashBoardPage(admin));
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
    }
}