using ClassAid.DataContex;
using ClassAid.Models.Users;
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
        readonly Student student;
        public Student Student { get; }
        public StudentNotActivatedPage(Student student)
        {
            Student = student;
            this.student = LocalDbContex.GetAdminInfo();
            Debug.WriteLine(this.student.Key);
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
            Launcher.OpenAsync(CreateNSUri(student.Phone));
        }
        private Uri CreateNSUri(string phoneNumber)
        {
            return new Uri($"tel:{phoneNumber}");
        }

        private void SendRequest_Clicked(object sender, EventArgs e)
        {
            student.IsRejected = false;
            FirebaseHandler.UpdateStudent(Student);
        }
    }
}