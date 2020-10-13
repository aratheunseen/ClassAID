using ClassAid.DataContex;
using ClassAid.Models.Users;
using Com.OneSignal;
using System;
using System.Diagnostics;
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
                    //Debug.WriteLine(student.AdminKey);
                    Admin tempAdmin = await FirebaseHandler.GetAdminAsync(tempStudent.AdminKey);
                    OneSignal.Current.SendTag("AdminKey", tempStudent.AdminKey);
                    activityIndicator.IsRunning = false;
                    LocalDbContex.CreateTables();
                    if (tempStudent.IsActive)
                    {
                        Preferences.Set(PrefKeys.IsLoggedIn, true);
                        Preferences.Set(PrefKeys.AdminKey, tempStudent.TeamCode);
                        Preferences.Set(PrefKeys.IsAdmin, false);
                        Preferences.Set(PrefKeys.Key, student.Key);
                        Application.Current.MainPage =
                            new NavigationPage(new Dashboard(tempStudent));                        
                        LocalDbContex.SaveUser(tempStudent);                        
                        LocalDbContex.SaveUser(tempAdmin);
                        LocalDbContex.SaveBatchDetails(tempAdmin.BatchDetails);
                    }
                    else
                    {
                        Debug.WriteLine(tempStudent.Name);
                        Debug.WriteLine(tempAdmin.Name);
                        LocalDbContex.SaveUser(tempStudent);
                        LocalDbContex.SaveUser(tempAdmin);
                        Application.Current.MainPage = new StudentNotActivatedPage(tempStudent);
                    }
                }
            }
            catch (Exception e)
            {
                resultText.Text = "Sorry something bad happened. " + e.Message;
            }

        }
    }
}