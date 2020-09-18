using ClassAid.Models.Users;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClassAid.Views
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
    }
}