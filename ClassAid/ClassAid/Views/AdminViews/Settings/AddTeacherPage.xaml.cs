using ClassAid.Models.Schedule;
using System;
using System.Collections.ObjectModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClassAid.Views.AdminViews.Settings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddTeacherPage : ContentPage
    {
        ObservableCollection<Teacher> teachers;
        public AddTeacherPage(ObservableCollection<Teacher> teachers)
        {
            InitializeComponent();
            this.teachers = teachers;
        }
        private void addTeacherBtn_Clicked(object sender, EventArgs e)
        {
            Teacher t = new Teacher() { Name = teacherName.Text, Designation = teacherDesegnation.Text };
            teachers.Add(t);
            Navigation.PopAsync();
        }

        private void inputField_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(teacherName.Text) && !string.IsNullOrWhiteSpace(teacherDesegnation.Text))
                addTeacherBtn.IsEnabled = true;
            else
                addTeacherBtn.IsEnabled = false;
        }

        private void goBackBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}