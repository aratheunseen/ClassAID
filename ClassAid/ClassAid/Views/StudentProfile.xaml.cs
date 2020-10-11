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

namespace ClassAid.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StudentProfile : ContentPage
    {
        public Admin Admin { get; set; }
        public Student Student { get; set; }
        private static BatchDetails BatchDetails;
        public StudentProfile(Admin admin)
        {
            Admin = admin;
            InitializeComponent();
            RetakeStudentArea.IsVisible = false;
            logoutBtn.Command = new Command(() => App.LogOut());
            AllocateRequestList(admin.AdminKey);
            addAnotherAdmin.IsVisible = false;
            anotherTeamCode.IsVisible = false;
        }
        public StudentProfile(Student student)
        {
            BatchDetails = LocalDbContex.GetBatchDetails();
            Student = student;
            InitializeComponent();
            logoutBtn.Command = new Command(() => App.LogOut());
            BindData();
            //if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            //{
            //    Task.Run(async () => Admin = await FirebaseHandler.GetAdminAsync(student.AdminKey));
            //}
            //else
            //{
            //    //Admin.BatchDetails = LocalDbContex.GetBatchDetails();
            //    Admin.StudentList =
            //        new ObservableCollection<Student>(LocalDbContex.GetStudents());
            //}
            
        }
        private async void AllocateRequestList(string key)
        {
            var data = await FirebaseHandler.GetPendingStudents(key);
            if (data != null)
                RequestCollectionView.ItemsSource = data;
            else
            {
                List<Admin> sample = new List<Admin>()
                {
                    new Admin() { Name = "Hasina", ID = "192311001", Phone = "0123567890" },
                    new Admin() { Name = "Mahmud", ID = "192311002", Phone = "0123567891" },
                    new Admin() { Name = "Alif", ID = "192311003", Phone = "0123567892" },
                    new Admin() { Name = "Khaleda", ID = "19231104", Phone = "0123567893" }
                };
                RequestCollectionView.ItemsSource = sample;
            }
        }
        private void BindData()
        {
            userName.Text = Student.Name;
            userPhone.Text = Student.Phone;
            userID.Text = Student.ID;
            userDepartment.Text = BatchDetails.Department;
            userSection.Text = BatchDetails.Section;
            userSemester.Text = BatchDetails.Semester;
            ObservableCollection<Student> students = new ObservableCollection<Student>();
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                FirebaseHandler.GetStudentList(students, Student.AdminKey);
                students.CollectionChanged += Students_CollectionChanged;
                RequestClassmateCollectionView.ItemsSource = students;
            }
            else
                RequestClassmateCollectionView.ItemsSource = LocalDbContex.GetStudents();
        }

        private void Students_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            LocalDbContex.SaveStudent(((ObservableCollection<Student>)sender)[e.NewStartingIndex]);
        }

        private void Name_Scrolled(object sender, ItemsViewScrolledEventArgs e)
        {

        }

        private void AcceptBtn_Clicked(object sender, EventArgs e)
        {

        }

        private void RejectBtn_Clicked(object sender, EventArgs e)
        {

        }

        private async void addAnotherAdmin_Clicked(object sender, EventArgs e)
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
    }
}