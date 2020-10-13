using ClassAid.DataContex;
using ClassAid.Models.Users;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ClassAid.Models;
using ClassAid.Models.Schedule;
using Firebase.Database.Query;

namespace ClassAid.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StudentProfile : ContentPage
    {
        public Admin Admin { get; set; }
        private ObservableCollection<Student> requestList;
        private ObservableCollection<Teacher> teacherList;
        private ObservableCollection<Student> studentList;
        public Student Student { get; set; }
        private static BatchDetails BatchDetails;
        public StudentProfile(Admin admin)
        {
            Admin = admin;
            InitializeComponent();
            mainGrid.Children.Remove(RetakeStudentArea);
            logoutBtn.Command = new Command(() => App.LogOut());
            BindData();
            AllocateRequestList(admin.Key);
        }
        public StudentProfile(Student student)
        {
            // TODO: Fix collection binding and saving
            Student = student;
            InitializeComponent();
            mainGrid.Children.Remove(RequestCollectionView);
            logoutBtn.Command = new Command(() => App.LogOut());
            BindData();
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                ObservableCollection<Student> students = new ObservableCollection<Student>();
                FirebaseHandler.GetStudentList(students, Student.AdminKey);
                students.CollectionChanged += Students_CollectionChanged;
                LocalDbContex.ClearTable(TableList.students);
                LocalDbContex.SaveStudents(students);
                //ClassmateCollectionView.ItemsSource = LocalDbContex.GetStudents();
            }
        }
        private async void AllocateRequestList(string key)
        {
            requestList = await FirebaseHandler.GetPendingStudents(key);
            if (requestList != null)
                RequestCollectionView.ItemsSource = requestList;
        }
        private void BindData()
        {
            if (Student == null)
            {
                userName.Text = Admin.Name;
                userPhone.Text = Admin.Phone;
                userID.Text = Admin.ID;
            }
            else
            {
                userName.Text = Student.Name;
                userPhone.Text = Student.Phone;
                userID.Text = Student.ID;
            }
            BatchDetails = LocalDbContex.GetBatchDetails();
            userUniversity.Text = BatchDetails.University;
            userDepartment.Text = BatchDetails.Department;
            userSection.Text = BatchDetails.Section;
            userSemester.Text = BatchDetails.Semester;

            ClassmateCollectionView.ItemsSource = LocalDbContex.GetStudents()
                .Select(item=>item.IsActive == true);
            TeacherCollectionView.ItemsSource = LocalDbContex.GetTeachers();
        }

        private void Students_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            LocalDbContex.SaveStudent(((ObservableCollection<Student>)sender)[e.NewStartingIndex]);
        }
        private void AcceptBtn_Clicked(object sender, EventArgs e)
        {
            if (sender is ImageButton b && b.CommandParameter is Student student)
            {
                FirebaseHandler.GetClient().Child(student.Key).Child("IsActive").PutAsync(true);
                Admin.StudentList.FirstOrDefault(p => p.Key == student.Key).IsActive = true;
                FirebaseHandler.UpdateAdmin(Admin);
                requestList.Remove(student);
            }
        }
        private void RejectBtn_Clicked(object sender, EventArgs e)
        {
            if (sender is ImageButton b && b.CommandParameter is Student student)
            {
                FirebaseHandler.GetClient().Child(student.Key).Child("IsRejected").PutAsync(true);
                Admin.StudentList.FirstOrDefault(p => p.Key == student.Key).IsRejected = true;
                FirebaseHandler.UpdateAdmin(Admin);
                requestList.Remove(student);
            }
        }
        private async void AddAnotherAdmin_Clicked(object sender, EventArgs e)
        {
            KeyVault key = await FirebaseHandler.ValidateTeamCode(anotherTeamCode.Text);
            if (key != null)
            {
                var retake = new RetakeStudentModel()
                {
                    TeamCode = anotherTeamCode.Text,
                    AdminKey = key.AdminKey,
                    IsActive = false
                };
                if (Student.RetakeModels == null)
                {
                    Student.RetakeModels = new List<RetakeStudentModel>();
                }
                Student.RetakeModels.Add(retake);
                FirebaseHandler.UpdateStudent(Student);
                FirebaseHandler.AddRetake(retake);
            }
        }

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
    }
}