using ClassAid.Models.Users;
using ClassAid.Views;
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
        
        private void AdminBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AdminLoginPage());
        }

        private void StudentBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new StudentLoginPage());
        }

        private void GotoBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ViewScheduleTemplate());
        }
        private void BypassBtn_Clicked(object sender, EventArgs e)
        {
            //Admin admin = new Admin("jondoe", "yodawgssup");
            //admin.ID = "192311000";
            //admin.Name = "Jon Doe";
            //admin.Phone = "01911104587";
            //Application.Current.MainPage = new Dashboard(admin);
        }
    }
}