using ClassAid.DataContex;
using ClassAid.Models.Users;
using Firebase.Database;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClassAid.Views.AdminViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdditionalDetails : ContentPage
    {
        Admin admin;
        FirebaseClient client;
        public AdditionalDetails(Admin admin, FirebaseClient client)
        {
            InitializeComponent();
            this.admin = admin;
            this.client = client;
        }

        private async void completeSignUp_Clicked(object sender, EventArgs e)
        {
            admin.Name = userRealName.Text;
            admin.Email = userEmail.Text;
            admin.Phone = long.Parse(userPhone.Text);
            try
            {
                await AdminDbHandler.UpdateAdmin(client, admin);
                Application.Current.MainPage = new NavigationPage(new AboutPage(admin));
            }
            catch (Exception)
            {
                // TODO: Custom error page with SVG
                resultText.Text = "Something bad happened. Please check back in a short.";
            }         
        }
    }
}