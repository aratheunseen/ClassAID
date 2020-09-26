using ClassAid.DataContex;
using System;
using ClassAid.Models.Users;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ClassAid.CustomComponents;

namespace ClassAid.Views.AdminViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminRegestrationPage : ContentPage
    {
        FireSharpDB dB;
        public AdminRegestrationPage()
        {
            InitializeComponent();
            string server = "https://classaidapp.firebaseio.com/";
            string authKey = "q4ckBo2jl1p2EB0qg9eTnAwXwPKYwt2DbcSCOc5V";
            dB = new FireSharpDB(server, authKey);
            Routing.RegisterRoute("aboutpage", typeof(AboutPage));

            
        }
        private async void btnAdd_Clicked(object sender, EventArgs e)
        {
            Admin admin = new Admin(userName.Text, userPass.Text);
            admin.ID = userId.Text;
            try
            {
                activityIndicator.IsRunning = true;
                string id = await dB.InsertData("Admin", admin);
                Application.Current.MainPage = new AdditionalDetails(admin, dB.GetClient());
                activityIndicator.IsRunning = false;
            }
            catch (Exception)
            {
                // TODO: Custom error page with SVG
                resultText.Text = "Something bad happened. Please check back in a short.";
            }

        }

        private void form_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(userName.Text) ||
                string.IsNullOrWhiteSpace(userId.Text) ||
                string.IsNullOrWhiteSpace(userPass.Text))
            {
                btnAdd.IsEnabled = false;
            }
            else
            {
                btnAdd.IsEnabled = true;
            }
        }

        private void userName_PropertyChanged()
        {

        }

        private void Frame_SizeChanged()
        {

        }
    }
}