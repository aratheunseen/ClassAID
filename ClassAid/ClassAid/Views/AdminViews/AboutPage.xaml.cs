using ClassAid.Models.Users;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ClassAid.Models;
using System.Collections.ObjectModel;
using ClassAid.Models.Schedule;
using System.Collections.Generic;

namespace ClassAid.Views.AdminViews
{

    public partial class AboutPage : ContentPage
    {
        public static ObservableCollection<ScheduleModel> scheduleCores;
        private bool canExit = false;
        public Admin admin;
        public string RealName { get { return admin.Name; } }
        public AboutPage(Admin admin)
        {
            scheduleCores = new ObservableCollection<ScheduleModel>();
            InitializeComponent();
            
            List<Teacher> tList = new List<Teacher>()
            {
                new Teacher(){Name="Nahid Hasan", Designation="Lect"},
                new Teacher(){Name="Shahrirar Saif", Designation="Lect"},
                new Teacher(){Name="Sarwar Parvej", Designation="Ast. Proff"}
            };
            teacherPeaker.ItemsSource = tList;
            
            //scheduleCores = new ObservableCollection<ScheduleModel>()
            //{
            //    new ScheduleModel(){ CourseCode="211", Subject="DS",Teacher=tList[0]}
            //};


            this.admin = admin;
            userName.Text = admin.Name;
            userMail.Text = admin.Email;
            userPhone.Text = admin.Phone.ToString();
            userID.Text = admin.ID;
            scheduleView.ItemsSource = scheduleCores;
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

        private void addScheduleBtn_Clicked(object sender, EventArgs e)
        {
            scheduleCores.Add(new ScheduleModel()
            {
                Teacher = (Teacher)teacherPeaker.SelectedItem,
                StartTime = startDate.Time,
                EndTime = endDate.Time,
                Subject = subjectName.Text,
                CourseCode = courseCode.Text
            });
        }

        private void scheduleRefreshView_Refreshing(object sender, EventArgs e)
        {
            scheduleRefreshView.IsRefreshing = false;
        }
    }
}