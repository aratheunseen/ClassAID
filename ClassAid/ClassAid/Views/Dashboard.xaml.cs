using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using ClassAid.Models.Users;
using ClassAid.Views.AdminViews.Settings;
using Xamarin.Essentials;

namespace ClassAid.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Dashboard : ContentPage
    {
        private static Shared shared;
        private static string timeFormat = @"dd\:hh\:mm";
        public Dashboard()
        {
            InitializeComponent();
        }
        public Dashboard(Admin admin)
        {
            InitializeComponent();
            shared = admin;
            InitializeData();
        }
        public Dashboard(Student student)
        {
            InitializeComponent();
            addNoticeImage.IsVisible = false;
            shared = student;
            InitializeData();
        }
        void logout()
        {
            Preferences.Set("isLoggedin", "false");
            Application.Current.MainPage = new StartPage();
        }
        private void InitializeData()
        {
           profileBtn.Command = new Command(()=> logout());
            
            try
            {
                var d = shared.ScheduleList[0];
                firstScheduleCourseCode.Text = d.CourseCode;
                firstScheduleCourseName.Text = d.Subject;
                firstScheduleStart.Text = d.StartTime.ToString(timeFormat);
                firstScheduleEnd.Text = d.EndTime.ToString(timeFormat);

                d = shared.ScheduleList[1];
                secondScheduleCourseCode.Text = d.CourseCode;
                secondScheduleCourseName.Text = d.Subject;
                secondScheduleStart.Text = d.StartTime.ToString(timeFormat);
                secondScheduleEnd.Text = d.EndTime.ToString(timeFormat);
            }
            catch (Exception)
            {

            }
        }

        //private void profileBtn_Clicked(object sender, EventArgs e)
        //{
        //    Preferences.Set("isLoggedin", "false");
        //    firstScheduleCourseCode.Text = "Success";
        //    Application.Current.MainPage = new StartPage();
        //}
    }
}