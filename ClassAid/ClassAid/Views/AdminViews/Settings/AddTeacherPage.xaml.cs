using ClassAid.DataContex;
using ClassAid.Models.Schedule;
using ClassAid.Models.Users;
using Firebase.Database;
using System;
using System.Collections.ObjectModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClassAid.Views.AdminViews.Settings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddTeacherPage : ContentPage
    {
        Admin admin;
        FirebaseClient client;
        public AddTeacherPage(Admin admin)
        {
            InitializeComponent();
            this.admin = admin;
            this.client = App.fireSharpClient.GetClient();
        }
        private async void addTeacherBtn_Clicked(object sender, EventArgs e)
        {
            Teacher t = new Teacher() { Name = teacherName.Text, Designation = teacherDesegnation.Text };
            admin.TeacherList.Add(t);
            await Navigation.PopAsync();
            await AdminDbHandler.UpdateAdmin(client, admin);            
        }

        private void inputField_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(teacherName.Text) && !string.IsNullOrWhiteSpace(teacherDesegnation.Text))
                addTeacherBtn.IsEnabled = true;
            else
                addTeacherBtn.IsEnabled = false;
        }

        private void goBackBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}