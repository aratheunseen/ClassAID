using ClassAid.DataContex;
using ClassAid.Models.Schedule;
using ClassAid.Models.Users;
using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClassAid.Views.AdminViews.Settings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddSchedulePage : ContentPage
    {
        private readonly Admin admin;
        public AddSchedulePage(Admin admin)
        {
            this.admin = admin;

            InitializeComponent();
            teacherPeaker.ItemsSource = admin.TeacherList;
            dayPeaker.ItemsSource = new List<string>(Enum.GetNames(typeof(DayOfWeek)));
        }

        private async void AddScheduleBtn_Clicked()
        {
            if (admin.ScheduleList == null)
                admin.ScheduleList = new ObservableCollection<ScheduleModel>();
            var sc = new ScheduleModel()
            {
                Teacher = ((Teacher)teacherPeaker.SelectedItem).Name,
                StartTime = startDate.Time,
                EndTime = endDate.Time,
                Subject = subjectName.Text,
                CourseCode = courseCode.Text,
                DayOfWeek = (DayOfWeek)dayPeaker.SelectedIndex
            };
            admin.ScheduleList.Insert(0, sc);
            await Navigation.PopAsync();
            LocalDbContex.SaveSchedules(sc);
            FirebaseHandler.UpdateAdmin(admin);
        }

        // TODO: Can not build after change the button to frame gesture
        private async void AddTeacher_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddTeacherPage(admin));
        }

        private void Form_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(courseCode.Text) ||
                string.IsNullOrWhiteSpace(subjectName.Text))
                addSchedule.Command = null;
            else
                addSchedule.Command = new Command(() => AddScheduleBtn_Clicked());
        }
    }
}