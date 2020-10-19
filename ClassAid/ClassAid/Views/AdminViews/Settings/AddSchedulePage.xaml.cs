using ClassAid.DataContex;
using ClassAid.Models;
using ClassAid.Models.Schedule;
using ClassAid.Models.Users;
using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClassAid.Views.AdminViews.Settings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddSchedulePage : ContentPage
    {
        public AddSchedulePage()
        {
            InitializeComponent();
            teacherPeaker.ItemsSource = App.Admin.TeacherList;
            dayPeaker.ItemsSource = new List<string>(Enum.GetNames(typeof(DayOfWeek)));
        }

        private async void AddScheduleBtn_Clicked()
        {
            var sc = new ScheduleModel()
            {
                Teacher = ((Teacher)teacherPeaker.SelectedItem).Name,
                StartTime = startDate.Time,
                EndTime = endDate.Time,
                Subject = subjectName.Text,
                CourseCode = courseCode.Text,
                DayOfWeek = (DayOfWeek)dayPeaker.SelectedIndex
            };
            App.Admin.ScheduleList.Add(sc);
            await Navigation.PopAsync();
            LocalDbContex.SaveSchedule(sc);
            App.UpdateAdminOrSync(App.Admin);
        }
        private async void AddTeacher_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddTeacherPage());
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