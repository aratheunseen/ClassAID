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
        private readonly FirebaseClient client;
        private readonly Admin admin;
        public AddSchedulePage(Admin admin)
        {
            if (admin.TeacherList == null)
            {
                admin.TeacherList = new ObservableCollection<Teacher>();
            }
            if (admin.ScheduleList == null)
            {
                admin.ScheduleList =
                    new ObservableCollection<ScheduleModel>();
            }
            this.admin = admin;
            client = App.fireSharpClient.GetClient();
            InitializeComponent();
            teacherPeaker.ItemsSource = admin.TeacherList;
            dayPeaker.ItemsSource = new List<string>(Enum.GetNames(typeof(DayOfWeek)));
        }

        private async void addScheduleBtn_Clicked(object sender, EventArgs e)
        {
            admin.ScheduleList.Insert(0, new ScheduleModel()
            {
                Teacher = (Teacher)teacherPeaker.SelectedItem,
                StartTime = startDate.Time,
                EndTime = endDate.Time,
                Subject = subjectName.Text,
                CourseCode = courseCode.Text,
                DayOfWeek = (DayOfWeek)dayPeaker.SelectedIndex
        });
            await Navigation.PopAsync();
            await AdminDbHandler.UpdateAdmin(client, admin);
        }

        private void goBackBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void inputTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(courseCode.Text) && !string.IsNullOrWhiteSpace(subjectName.Text))
                addTeacherBtn.IsEnabled = true;
            else
                addTeacherBtn.IsEnabled = false;
        }

        private async void addTeacher_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddTeacherPage(admin));
        }
    }
}