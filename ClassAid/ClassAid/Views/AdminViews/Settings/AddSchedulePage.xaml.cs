using ClassAid.DataContex;
using ClassAid.Models.Schedule;
using ClassAid.Models.Users;
using Firebase.Database;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClassAid.Views.AdminViews.Settings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddSchedulePage : ContentPage
    {
        private readonly FirebaseClient client;
        private readonly Admin admin;
        public AddSchedulePage(Admin admin, FirebaseClient client)
        {
            this.admin = admin;
            this.client = client;
            InitializeComponent();
            teacherPeaker.ItemsSource = admin.teacherList;
        }

        private async void addScheduleBtn_Clicked(object sender, EventArgs e)
        {
            admin.ScheduleList.Insert(0, new ScheduleModel()
            {
                Teacher = (Teacher)teacherPeaker.SelectedItem,
                StartTime = startDate.Time,
                EndTime = endDate.Time,
                Subject = subjectName.Text,
                CourseCode = courseCode.Text
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
    }
}