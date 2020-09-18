using ClassAid.DataContex;
using ClassAid.Models.Users;
using ClassAid.Views;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClassAid
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        FireSharpDB dB;
        public MainPage()
        {
            InitializeComponent();
            string server = "https://classaidapp.firebaseio.com/";
            string authKey = "q4ckBo2jl1p2EB0qg9eTnAwXwPKYwt2DbcSCOc5V";
            dB = new FireSharpDB(server, authKey);
            Routing.RegisterRoute("aboutpage", typeof(AboutPage));
        }
        //protected override void OnAppearing()
        //{
        //    base.OnAppearing();
        //    this.Animate("", (s) => Layout(new Rectangle(((1 - s) * Width), Y, Width, Height)), 0, 600, Easing.SpringIn, null, null);

        //}
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

        //private void goBackButton_Clicked(object sender, EventArgs e)
        //{
        //    Navigation.PopAsync();
        //}
    }
}