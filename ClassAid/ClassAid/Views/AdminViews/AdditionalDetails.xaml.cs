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
        public static LocalAdminStorageContex storageContex;
        public static LocalAdminStorageContex Database
        {
            get
            {
                if (storageContex == null)
                {
                    storageContex = new LocalAdminStorageContex();
                }
                return storageContex;
            }
        }
        public AdditionalDetails(Admin admin)
        {
            InitializeComponent();
            this.admin = admin;
        }

        private async void completeSignUp_Clicked(object sender, EventArgs e)
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
                    User = UserType.Admin
                };
                File.WriteAllText(App.authFile,JsonConvert.SerializeObject(authModel));
                storageContex.SaveItemAsync(admin);
            }
            catch (Exception)
            {
                // TODO: Custom error page with SVG
                resultText.Text = "Something bad happened. Please check back in a short.";
            }         
        }
    }
}