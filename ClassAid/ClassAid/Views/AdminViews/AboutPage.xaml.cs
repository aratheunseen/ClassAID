using ClassAid.Models.Users;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ClassAid.Models;

namespace ClassAid.Views.AdminViews
{

    public partial class AboutPage : ContentPage
    {
        private bool canExit = false;
        public Admin admin;
        public string RealName { get { return admin.Name; } }
        public AboutPage(Admin admin)
        {
            InitializeComponent();
            this.admin = admin;
            userMail.Text = admin.Email;
            userPhone.Text = admin.Phone.ToString();
            userID.Text = admin.ID;
        }
        protected override bool OnBackButtonPressed()
        {            
            if (!canExit)
            {
                DependencyService.Get<Toast>().Show("Press again to Exit.");
                canExit = true;
            }
            else
                System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
            return true;
        }
    }
}