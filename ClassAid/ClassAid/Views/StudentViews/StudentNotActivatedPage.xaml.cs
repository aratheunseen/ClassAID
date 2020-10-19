using ClassAid.DataContex;
using ClassAid.Models.Users;
using System;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClassAid.Views.StudentViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StudentNotActivatedPage : ContentPage
    {
        Student Student;
        Admin Admin;
        public StudentNotActivatedPage(Student student, Admin admin)
        {
            Student = student;
            Admin = admin;
            InitializeComponent();
            LogOut.Command = new Command(() => App.LogOut());
            if (Student.IsRejected == true)
            {
                ErrorText.Text = "Opps " + Student.Name.Split(" ")[0] + "! Your " +
                    "Account has been rejected by the authority. " +
                    "If you think this is an mistake, call your " +
                    "corresponding class representative," +
                    "or send request again.";
                SendRequest.IsVisible = true;
            }
            else
                ErrorText.Text = "Opps " + Student.Name.Split(" ")[0] + "! " +
                    "Your Account hasn't been activated yet. " +
                    "Please re-log in if you think this is an mistake or call your " +
                    "corresponding class representative.";
        }

        private void Call_Clicked(object sender, EventArgs e)
        {
            Launcher.OpenAsync(new Uri($"tel:{Admin.Phone}"));
        }

        private void SendRequest_Clicked(object sender, EventArgs e)
        {
            Student.IsRejected = false; 
            FirebaseHandler.UpdateStudent(Student);
            var obj = Admin.StudentList.FirstOrDefault(x => x.Key == Student.Key);
            obj.IsRejected = false;
            obj.IsActive = false;
            App.UpdateAdminOrSync(Admin);
        }
    }
}