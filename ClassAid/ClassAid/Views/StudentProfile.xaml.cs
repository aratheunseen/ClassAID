using ClassAid.DataContex;
using ClassAid.Models.Users;
using System.Collections.Generic;
using System;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;
using ClassAid.Models;
using ClassAid.Models.Schedule;
using Firebase.Database.Query;
using ClassAid.Views.AdminViews.Settings;

namespace ClassAid.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StudentProfile : ContentPage
    {
        private ObservableCollection<Student> requestList;
        public StudentProfile()
        {
            InitializeComponent();
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            logoutBtn.Command = new Command(() => App.LogOut());
            userDepartment.Text = App.BatchDetails.Department;
            userUniversity.Text = App.BatchDetails.University;
            userSection.Text = App.BatchDetails.Section;
            userSemester.Text = App.BatchDetails.Semester;
            if (Preferences.Get(PrefKeys.IsAdmin, false))
            {
                userName.Text = App.Admin.Name;
                userPhone.Text = App.Admin.Phone;
                userID.Text = App.Admin.ID;
                mainGrid.Children.Remove(RetakeStudentArea);
                editBatchDetails.Command = new Command(() =>
                    Navigation.PushAsync(new EditBatchDetails()));
                if (Connectivity.NetworkAccess == Connectivity.NetworkAccess)
                    AllocateRequestList(App.Admin.Key);
            }
            else
            {
                mainGrid.Children.Remove(RequestCollectionView);
                mainGrid.Children.Remove(RequestListTitle);
                mainGrid.Children.Remove(editBatchDetails);
                userName.Text = App.Student.Name;
                userPhone.Text = App.Student.Phone;
                userID.Text = App.Student.ID;
            }
            ClassmateCollectionView.ItemsSource = App.StudentList;
            TeacherCollectionView.ItemsSource = App.TeacherList;
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            AllocateRequestList(App.Admin.Key);
        }

        private async void AllocateRequestList(string key)
        {
            requestList = await FirebaseHandler.GetPendingStudents(key);
            if (requestList != null)
            {
                if (requestList.Count == 0)
                {
                    mainGrid.Children.Remove(RequestCollectionView);
                    mainGrid.Children.Remove(RequestListTitle);
                }
                else
                    RequestCollectionView.ItemsSource = requestList;
            }
        }

        #region Accept And Reject
        private void AcceptBtn_Clicked(object sender, EventArgs e)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                if (sender is ImageButton b && b.CommandParameter is Student student)
                {
                    FirebaseHandler.GetClient().Child(student.Key).Child("IsActive").PutAsync(true);
                    App.Admin.StudentList.FirstOrDefault(p => p.Key == student.Key).IsActive = true;
                    App.UpdateAdminOrSync();
                    requestList.Remove(student);
                }
            }
            else
                DependencyService.Get<Toast>().Show("No INTERNET access. Try again later.");
        }
        private void RejectBtn_Clicked(object sender, EventArgs e)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                if (sender is ImageButton b && b.CommandParameter is Student student)
                {
                    FirebaseHandler.GetClient().Child(student.Key).Child("IsRejected").PutAsync(true);
                    App.Admin.StudentList.FirstOrDefault(p => p.Key == student.Key).IsRejected = true;
                    App.UpdateAdminOrSync();
                    requestList.Remove(student);
                }
            }
            else
                DependencyService.Get<Toast>().Show("No INTERNET access. Try again later.");
        }
        #endregion
        private async void AddAnotherAdmin_Clicked(object sender, EventArgs e)
        {
            // TODO: Not verified portion
            KeyVault key = await FirebaseHandler.ValidateTeamCode(anotherTeamCode.Text);
            if (key != null)
            {
                var retake = new RetakeStudentModel()
                {
                    TeamCode = anotherTeamCode.Text,
                    AdminKey = key.AdminKey,
                    IsActive = false
                };
                if (App.Student.RetakeModels == null)
                {
                    App.Student.RetakeModels = new List<RetakeStudentModel>();
                }
                App.Student.RetakeModels.Add(retake);
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    FirebaseHandler.UpdateStudent(App.Student);
                    FirebaseHandler.AddRetake(retake);
                }
            }
        }
        #region Call Functions
        private void ClassMateCallBtn_Clicked(object sender, EventArgs e)
        {
            ImageButton button = (ImageButton)sender;
            var d = (Student)button.BindingContext;
            Launcher.OpenAsync(new Uri($"tel:{d.Phone}"));
        }
        private void TeacherCallBtn_Clicked(object sender, EventArgs e)
        {
            ImageButton button = (ImageButton)sender;
            var d = (Teacher)button.BindingContext;
            Launcher.OpenAsync(new Uri($"tel:{d.Phone}"));
        }
        #endregion
    }
}