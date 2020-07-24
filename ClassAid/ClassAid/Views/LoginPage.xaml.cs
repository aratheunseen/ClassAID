using ClassAid.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClassAid.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void loginBtn_Clicked(object sender, EventArgs e)
        {
            User user = new User(usernameText.Text, passwordText.Text);
            if (user.CheckInformation())
            {
                DisplayAlert("Login", "Login success.", "Ok");
            }
            else
            {
                DisplayAlert("Login", "Unsuccessful login attempt.", "Try Again");
            }
        }
    }
}