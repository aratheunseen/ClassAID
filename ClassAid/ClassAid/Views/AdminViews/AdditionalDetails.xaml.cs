using ClassAid.DataContex;
using ClassAid.Models.Engines;
using ClassAid.Models.Users;
using Firebase.Database;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
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
            try
            {
                await AdminDbHandler.UpdateAdmin(App.fireSharpClient.GetClient(), admin);
                Application.Current.MainPage = new NavigationPage(new DashBoardPage(admin));
                LoginAuthModel authModel = new LoginAuthModel()
                {
                    isLoggedIn = true,
                    key = admin.Key,
                    User = UserType.Admin,
                    AdminData = admin
                };
                File.WriteAllText(App.authFile,JsonConvert.SerializeObject(authModel));
                
            }
            catch (Exception)
            {
                // TODO: Custom error page with SVG
                resultText.Text = "Something bad happened. Please check back in a short.";
            }         
        }
    }
}