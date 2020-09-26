using ClassAid.Models.Users;
using ClassAid.Views.AdminViews;
using ClassAid.Views.StudentViews;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClassAid
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartPage : ContentPage
    {
        public StartPage()
        {
            InitializeComponent();
        }

        private void adminBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AdminRegestrationPage());
        }

        private void studentBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new StudentLoginPage());
        }

        private void bypassBtn_Clicked(object sender, EventArgs e)
        {
            Admin admin = new Admin("HolaSenorita","IamAwesome");
            admin.Email = "mail@iloveyou.com";
            admin.ID = "192311000";
            admin.Name = "Jon Doe";
            admin.Phone = 01911104587;
            //Application.Current.MainPage = new AboutPage(admin);
            Application.Current.MainPage = new NavigationPage(new AboutPage(admin));

        }
    }
}