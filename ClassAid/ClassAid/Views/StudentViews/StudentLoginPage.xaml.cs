using ClassAid.DataContex;
using ClassAid.Models.Users;
using Com.OneSignal;
using System;
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
                var tempStudent =
                    await FirebaseHandler.GetStudentAsync(student.Key);
                if (tempStudent == null)
                {
                    activityIndicator.IsRunning = false;
                    await Navigation.PushAsync(
                        new AdditionalDetails(student));
                    FirebaseHandler.InsertStudent(student);
                }
                else
                {
                    var tempAdmin = await FirebaseHandler.GetAdminAsync(student.AdminKey);
                    OneSignal.Current.SendTag("AdminKey", tempStudent.AdminKey);
                    activityIndicator.IsRunning = false;
                    if (student.IsActive)
                    {
                        Preferences.Set(PrefKeys.IsLoggedIn, true);
                        Preferences.Set(PrefKeys.AdminKey, tempStudent.TeamCode);
                        Preferences.Set(PrefKeys.IsAdmin, false);
                        Preferences.Set(PrefKeys.Key, student.Key);
                        Application.Current.MainPage =
                            new NavigationPage(new Dashboard(tempStudent));
                        LocalDbContex.CreateTables();
                        LocalDbContex.SaveUser(student);                        
                        LocalDbContex.SaveUser(tempAdmin);
                        LocalDbContex.SaveBatchDetails(tempAdmin.BatchDetails);
                    }
                    else
                    {
                        Application.Current.MainPage = new StudentNotActivatedPage(student);
                        LocalDbContex.CreateTables();
                        LocalDbContex.SaveUser(student);
                        LocalDbContex.SaveUser(tempAdmin);
                    }
                }
            }
            catch (Exception e)
            {
                resultText.Text = "Sorry something bad happened.";
            }

        }
    }
}