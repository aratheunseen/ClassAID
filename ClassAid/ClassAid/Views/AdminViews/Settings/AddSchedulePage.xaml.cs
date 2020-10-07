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
        private readonly Admin user;
        public AddSchedulePage(Admin user)
        {
            if (user.TeacherList == null)
            {
                user.TeacherList = new ObservableCollection<Teacher>();
            }
            if (user.ScheduleList == null)
            {
                user.ScheduleList =
                    new ObservableCollection<ScheduleModel>();
            }
            this.user = user;
            InitializeComponent();
            teacherPeaker.ItemsSource = user.TeacherList;
            dayPeaker.ItemsSource = new List<string>(Enum.GetNames(typeof(DayOfWeek)));
        }

        private async void AddScheduleBtn_Clicked()
        {
            user.ScheduleList.Insert(0, new ScheduleModel()
            {
                Teacher = (Teacher)teacherPeaker.SelectedItem,
                StartTime = startDate.Time,
                EndTime = endDate.Time,
                Subject = subjectName.Text,
                CourseCode = courseCode.Text,
                DayOfWeek = (DayOfWeek)dayPeaker.SelectedIndex
        });
            await Navigation.PopAsync();
            await FirebaseHandler.UpdateUser(user);
        }

        private void goBackBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        // TODO: Can not build after change the button to frame gesture
        private async void AddTeacher_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddTeacherPage(user));
        }

        private void Form_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(courseCode.Text) ||
                string.IsNullOrWhiteSpace(subjectName.Text))
                addSchedule.Command = null;
            else
                addSchedule.Command = new Command(()=> AddScheduleBtn_Clicked());
        }
    }
}