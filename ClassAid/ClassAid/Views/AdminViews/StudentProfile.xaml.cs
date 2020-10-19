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
            userDepartment.Text = App.Admin.BatchDetails.Department;
            userUniversity.Text = App.Admin.BatchDetails.University;
            userSection.Text = App.Admin.BatchDetails.Section;
            userSemester.Text = App.Admin.BatchDetails.Semester;
            if (Preferences.Get(PrefKeys.IsAdmin, false))
            {
                userName.Text = App.Admin.Name;
                userPhone.Text = App.Admin.Phone;
                userID.Text = App.Admin.ID;
                editBatchDetails.Command = new Command(() =>
                    Navigation.PushAsync(new EditBatchDetails()));
                if (Connectivity.NetworkAccess == Connectivity.NetworkAccess)
                    AllocateRequestList(App.Admin.Key);
            }
            else
            {
                //mainGrid.Children.Remove(RequestCollectionView);
                //mainGrid.Children.Remove(RequestListTitle);
                //mainGrid.Children.Remove(editBatchDetails);
                userName.Text = App.Student.Name;
                userPhone.Text = App.Student.Phone;
                userID.Text = App.Student.ID;
            }
            ClassmateCollectionView.ItemsSource = App.Admin.StudentList.Where(p => p.IsActive == true);
            TeacherCollectionView.ItemsSource = App.Admin.TeacherList;
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
                RequestCollectionView.ItemsSource = requestList;
                if (requestList.Count == 0)
                {
                    ////mainGrid.Children.Remove(RequestCollectionView);
                    ////mainGrid.Children.Remove(RequestListTitle);
                }
            }
            requestList.CollectionChanged += RequestList_CollectionChanged;
        }

        private void RequestList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            //mainGrid.Children.Add(RequestListTitle);
            //mainGrid.Children.Add(RequestCollectionView);
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
                    App.UpdateAdminOrSync(App.Admin);
                    requestList.Remove(student);
                    LocalDbContex.SaveStudent(student);
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
                    App.UpdateAdminOrSync(App.Admin);
                    requestList.Remove(student);
                }
            }
            else
                DependencyService.Get<Toast>().Show("No INTERNET access. Try again later.");
        }
        #endregion

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