using ClassAid.Models.Users;
using ClassAid.Views.AdminViews;
using ClassAid.Views.StudentViews;
using System;
using Xamarin.Essentials;
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
            //Preferences.Set("test", "Hello World");
            testText.Text = Preferences.Get("test","Not Found").ToString();
            //Application.Current.Properties["id"] = "LOL";
            //secondTestText.Text = Application.Current.Properties["id"] as string;
        }

        private void adminBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AdminLoginPage());
        }

        private void studentBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new StudentLoginPage());
        }

        private void gotoBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ViewScheduleTemplate());
        }

        private void bypassBtn_Clicked(object sender, EventArgs e)
        {
            Admin admin = new Admin("HolaSenorita","IamAwesome");
            admin.ID = "192311000";
            admin.Name = "Jon Doe";
            admin.Phone = "01911104587";
            Application.Current.MainPage = new DashBoardPage(admin);
            Application.Current.MainPage = new NavigationPage(new DashBoardPage(admin));
        }
    }
}