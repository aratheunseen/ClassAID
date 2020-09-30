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
        Shared user;
        FirebaseClient client;
        public AddTeacherPage(Shared user)
        {
            InitializeComponent();
            this.user = user;
            client = App.fireSharpClient.GetClient();
        }
        private async void AddTeacherBtn_Clicked()
        {
            Teacher t = new Teacher() 
            { 
                Name = teacherName.Text, 
                Designation = teacherDesegnation.Text 
            };
            user.TeacherList.Add(t);
            await Navigation.PopAsync();
            await AdminDbHandler.UpdateAdmin(client, user);            
        }
        
        // TODO: Can not build after change the button to frame gesture

        private void Form_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(teacherName.Text) ||
                string.IsNullOrWhiteSpace(teacherDesegnation.Text))
                addTeacherBtn.Command= null;
            else
                addTeacherBtn.Command = new Command(()=> AddTeacherBtn_Clicked());
        }
    }
}