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
            //RetakeStudentArea.IsVisible = false;
            logoutBtn.Command = new Command(() => App.LogOut());
            //addAnotherAdmin.IsVisible = false;
            //anotherTeamCode.IsVisible = false;
            BindData();
            AllocateRequestList(admin.Key);
        }
        public StudentProfile(Student student)
        {
            Student = student;
            InitializeComponent();
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
            //var data = await FirebaseHandler.GetPendingStudents(key);
            //if (data != null)
            //    RequestCollectionView.ItemsSource = data;
            //else
            //{
                List<Student> sample = new List<Student>()
                {
                    new Admin() { Name = "Hasina", ID = "192311001", Phone = "0123567890" },
                    new Admin() { Name = "Mahmud", ID = "192311002", Phone = "0123567891" },
                    new Admin() { Name = "Alif", ID = "192311003", Phone = "0123567892" },
                    new Admin() { Name = "Alif", ID = "192311003", Phone = "0123567892" },
                    new Admin() { Name = "Alif", ID = "192311003", Phone = "0123567892" },
                    new Admin() { Name = "Alif", ID = "192311003", Phone = "0123567892" },
                    new Admin() { Name = "Alif", ID = "192311003", Phone = "0123567892" },
                    new Admin() { Name = "Alif", ID = "192311003", Phone = "0123567892" },
                    new Admin() { Name = "Alif", ID = "192311003", Phone = "0123567892" },
                    new Admin() { Name = "Alif", ID = "192311003", Phone = "0123567892" },
                    new Admin() { Name = "Alif", ID = "192311003", Phone = "0123567892" },
                    new Admin() { Name = "Alif", ID = "192311003", Phone = "0123567892" },
                    new Admin() { Name = "Alif", ID = "192311003", Phone = "0123567892" },
                    new Admin() { Name = "Alif", ID = "192311003", Phone = "0123567892" },
                    new Admin() { Name = "Alif", ID = "192311003", Phone = "0123567892" },
                    new Admin() { Name = "Alif", ID = "192311003", Phone = "0123567892" },
                    new Admin() { Name = "Alif", ID = "192311003", Phone = "0123567892" },
                    new Admin() { Name = "Alif", ID = "192311003", Phone = "0123567892" },
                    new Admin() { Name = "Alif", ID = "192311003", Phone = "0123567892" },
                    new Admin() { Name = "Alif", ID = "192311003", Phone = "0123567892" },
                    new Admin() { Name = "Alif", ID = "192311003", Phone = "0123567892" },
                    new Admin() { Name = "Alif", ID = "192311003", Phone = "0123567892" },
                    new Admin() { Name = "Alif", ID = "192311003", Phone = "0123567892" },
                    new Admin() { Name = "Alif", ID = "192311003", Phone = "0123567892" },
                    new Admin() { Name = "Alif", ID = "192311003", Phone = "0123567892" },
                    new Admin() { Name = "Alif", ID = "192311003", Phone = "0123567892" },
                    new Admin() { Name = "Khaleda", ID = "19231104", Phone = "0123567893" }
                };
                RequestCollectionView.ItemsSource = sample;
            //}
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

            //ClassmateCollectionView.ItemsSource = LocalDbContex.GetStudents();
            //TeacherCollectionView.ItemsSource = LocalDbContex.GetTeachers();
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
            //requestTextSample.Text =
            //    ((Student)RequestCollectionView.SelectedItem).TeamCode;
        }

        private void RejectBtn_Clicked(object sender, EventArgs e)
        {
            //requestTextSample.Text =
            //    ((Student)RequestCollectionView.SelectedItem).TeamCode;
        }

        private async void AddAnotherAdmin_Clicked(object sender, EventArgs e)
        {
            //KeyVault key = await FirebaseHandler.ValidateTeamCode(anotherTeamCode.Text);
            //if (key != null)
            //{
            //    var retake = new RetakeStudentModel()
            //    {
            //        TeamCode = anotherTeamCode.Text,
            //        AdminKey = key.AdminKey,
            //        IsActive = false
            //    };
            //    if (Student.RetakeModels == null)
            //    {
            //        Student.RetakeModels = new List<RetakeStudentModel>();
            //    }
            //    Student.RetakeModels.Add(retake);
            //    FirebaseHandler.UpdateStudent(Student);
            //    FirebaseHandler.AddRetake(retake);
            //}
        }

        private void AcceptBtn_Clicked_1(object sender, EventArgs e)
        {

        }
    }
}