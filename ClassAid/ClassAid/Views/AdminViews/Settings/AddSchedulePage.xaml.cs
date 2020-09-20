using ClassAid.Models.Schedule;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClassAid.Views.AdminViews.Settings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddSchedulePage : ContentPage
    {
        private ObservableCollection<ScheduleModel> schedules;
        private ObservableCollection<Teacher> teachers;
        public AddSchedulePage(ObservableCollection<ScheduleModel> schedules,ObservableCollection<Teacher> teachers)
        {
            this.teachers = teachers;
            this.schedules = schedules;
            InitializeComponent();
            teacherPeaker.ItemsSource = this.teachers;
        }

        private void addScheduleBtn_Clicked(object sender, EventArgs e)
        {
            schedules.Insert(0, new ScheduleModel()
            {
                Teacher = (Teacher)teacherPeaker.SelectedItem,
                StartTime = startDate.Time,
                EndTime = endDate.Time,
                Subject = subjectName.Text,
                CourseCode = courseCode.Text
            });
            Navigation.PopAsync();
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