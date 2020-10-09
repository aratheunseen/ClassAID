using ClassAid.DataContex;
using ClassAid.Models.Users;
using System;
using System.Collections.Generic;
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
        Student admin = LocalDbContex.GetAdmin();
        public Student student{ get; }
        public StudentNotActivatedPage(Student student)
        {
            this.student = student;
            InitializeComponent();
            LogOut.Command = new Command(() => App.LogOut());
            ErrorText.Text = "Opps " + student.Name + "! Your Account hasn't been activated yet. " +
                "Please re-log in if you think this is an mistake or call your corresponding class representative.";
        }

        private void Call_Clicked(object sender, EventArgs e)
        {
            Launcher.OpenAsync(CreateNSUri(admin.Phone));
        }
        private Uri CreateNSUri(string phoneNumber)
        {
            return new Uri($"tel:{phoneNumber}");
        }
    }
}