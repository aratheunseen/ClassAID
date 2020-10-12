using ClassAid.DataContex;
using ClassAid.Models.Users;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClassAid.Views.StudentViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StudentNotActivatedPage : ContentPage
    {
        public Student Student { get; }
        public StudentNotActivatedPage(Student student)
        {
            Student = student;
            InitializeComponent();
            LogOut.Command = new Command(() => App.LogOut());
            if (student.IsRejected == true)
            {
                ErrorText.Text = "Opps " + student.Name.Split(" ")[0] + "! Your Account hasn't been rejected by the authority. " +
                    "Please re-log in if you think this is an mistake or call your corresponding class representative." +
                    "or send request again.";
                SendRequest.IsVisible = true;
            }
            else
                ErrorText.Text = "Opps " + student.Name.Split(" ")[0] + "! Your Account hasn't been activated yet. " +
                    "Please re-log in if you think this is an mistake or call your corresponding class representative.";
        }

        private void Call_Clicked(object sender, EventArgs e)
        {
            Launcher.OpenAsync(CreateNSUri(Student.Phone));
        }
        private Uri CreateNSUri(string phoneNumber)
        {
            return new Uri($"tel:{phoneNumber}");
        }

        private async void SendRequest_Clicked(object sender, EventArgs e)
        {
            Student.IsRejected = false;
            FirebaseHandler.UpdateStudent(Student);
            var admin = await FirebaseHandler.GetAdminAsync(Student.AdminKey);
            var obj = admin.StudentList.FirstOrDefault(x => x.Key == Student.Key);
            obj.IsRejected = false;
            obj.IsActive = false;
            FirebaseHandler.UpdateAdmin(admin);
        }
    }
}