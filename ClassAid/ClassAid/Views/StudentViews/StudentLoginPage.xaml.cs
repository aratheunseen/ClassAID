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
                Student tempStudent = new Student(userName.Text + "student", userPass.Text)
                {
                    IsAdmin = false
                };

                activityIndicator.IsRunning = true;
                Student Student = await FirebaseHandler.GetStudentAsync(tempStudent.Key);

                if (Student == null || Student.Name == null)
                {
                    activityIndicator.IsRunning = false;
                    Student = tempStudent;
                    await Navigation.PushAsync(
                        new AdditionalDetails(Student));
                    FirebaseHandler.InsertStudent(tempStudent);
                }
                else
                {
                    Admin Admin = await FirebaseHandler.GetAdminAsync(Student.AdminKey);

                    activityIndicator.IsRunning = false;
                    LocalDbContex.CreateTables();
                    Preferences.Set(PrefKeys.IsLoggedIn, true);
                    Preferences.Set(PrefKeys.AdminKey, Student.AdminKey);
                    Preferences.Set(PrefKeys.IsAdmin, false);
                    Preferences.Set(PrefKeys.Key, tempStudent.Key);

                    if (Student.IsActive)
                    {
                        LocalDbContex.SaveUser(Student);
                        LocalDbContex.SaveUser(Admin);
                        LocalDbContex.SaveSchedules(Admin.ScheduleList);
                        LocalDbContex.SaveEvents(Admin.EventList);

                        Application.Current.MainPage =
                            new NavigationPage(new Dashboard());

                        LocalDbContex.SaveBatchDetails(Admin.BatchDetails);
                        LocalDbContex.SaveTeachers(Admin.TeacherList);
                        LocalDbContex.SaveStudents(Admin.StudentList
                                .Where(item => item.IsActive == true).ToList());

                        OneSignal.Current.SendTag("AdminKey", tempStudent.AdminKey);
                        OneSignal.Current.RegisterForPushNotifications();
                    }
                    else
                    {
                        LocalDbContex.SaveUser(Student);
                        LocalDbContex.SaveUser(Admin);
                        Application.Current.MainPage = new StudentNotActivatedPage(Student, Admin);
                    }
                }
            }
            catch (Exception)
            {
                resultText.Text = "Sorry something bad happened. ERROR code PCAiDx02";
            }
        }
    }
}