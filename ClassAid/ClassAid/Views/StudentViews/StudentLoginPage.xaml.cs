using ClassAid.DataContex;
using ClassAid.Models.Users;
using Com.OneSignal;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClassAid.Views.StudentViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StudentLoginPage : ContentPage
    {
        public static Command TapCommand;
        public StudentLoginPage()
        {
            InitializeComponent();
            TapCommand = new Command(() => ProceedBtn());
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            userName.Focus();
        }
        private void Form_TextChanged(object sender, TextChangedEventArgs e)
        {
            userName.Text.Replace(" ", "");
            if (string.IsNullOrWhiteSpace(userName.Text) ||
                string.IsNullOrWhiteSpace(userPass.Text) ||
                userName.Text.Length < 6 ||
                userPass.Text.Length < 6)
            {
                studentSignIn.Command = null;
            }
            else
                studentSignIn.Command = TapCommand;
        }
        private async void ProceedBtn()
        {
            try
            {
                Student student = new Student(userName.Text + "student", userPass.Text)
                {
                    IsAdmin = false
                };

                activityIndicator.IsRunning = true;
                App.Student = await FirebaseHandler.GetStudentAsync(student.Key);

                if (App.Student == null || App.Student.Name == null)
                {
                    activityIndicator.IsRunning = false;
                    App.Student = student;
                    await Navigation.PushAsync(
                        new AdditionalDetails());
                    FirebaseHandler.InsertStudent(student);
                }
                else
                {
                    App.Admin = await FirebaseHandler.GetAdminAsync(App.Student.AdminKey);

                    activityIndicator.IsRunning = false;
                    LocalDbContex.CreateTables();
                    if (App.Student.IsActive)
                    {
                        Preferences.Set(PrefKeys.IsLoggedIn, true);
                        Preferences.Set(PrefKeys.AdminKey, App.Student.AdminKey);
                        Preferences.Set(PrefKeys.IsAdmin, false);
                        Preferences.Set(PrefKeys.Key, student.Key);

                        Application.Current.MainPage =
                            new NavigationPage(new Dashboard());

                        LocalDbContex.SaveUser(App.Student);
                        LocalDbContex.SaveUser(App.Admin);

                        LocalDbContex.SaveBatchDetails(App.Admin.BatchDetails);

                        LocalDbContex.SaveEvents(App.Admin.EventList);

                        LocalDbContex.SaveTeachers(App.Admin.TeacherList);

                        LocalDbContex.SaveSchedules(App.Admin.ScheduleList);

                        LocalDbContex.SaveStudents(App.Admin.StudentList
                                .Where(item => item.IsActive == true).ToList());

                        OneSignal.Current.SendTag("AdminKey", student.AdminKey);
                        OneSignal.Current.RegisterForPushNotifications();
                    }
                    else
                    {
                        LocalDbContex.SaveUser(App.Student);
                        LocalDbContex.SaveUser(App.Admin);
                        Application.Current.MainPage = new StudentNotActivatedPage();
                    }
                }
            }
            catch (Exception e)
            {
                resultText.Text = "Sorry something bad happened. ERROR code PCAiDx02";
            }
        }
    }
}