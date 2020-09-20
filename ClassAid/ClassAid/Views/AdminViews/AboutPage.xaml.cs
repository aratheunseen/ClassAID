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
            DependencyService.Get<Toast>().Show("This is a message. LOL.");
            return true;
        }
    }    
}