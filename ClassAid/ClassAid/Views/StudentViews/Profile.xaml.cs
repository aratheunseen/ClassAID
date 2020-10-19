using ClassAid.DataContex;
using ClassAid.Models.Schedule;
using ClassAid.Models.Users;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClassAid.Views.StudentViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Profile : ContentPage
    {
        public Student Student { get; set; }
        public ICommand Logout { get { return new Command(() => App.LogOut()); } }
        public IEnumerable<Student> Students { get { return LocalDbContex.GetStudents().Where(p => p.ID != Student.ID); } }
        public IEnumerable<Teacher> Teachers { get { return LocalDbContex.GetTeachers(); } }
        public Profile()
        {
            Student = LocalDbContex.GetStudentAsUser();
            BatchDetails batch = LocalDbContex.GetBatchDetails();
            //List<Student> students = LocalDbContex.GetStudents();
            //List<Teacher> teachers = LocalDbContex.GetTeachers();
            InitializeComponent();
            TeacherCollectionView.ItemsSource = Teachers;
            userName.Text = Student.Name;
            userID.Text = Student.ID;
            userPhone.Text = Student.Phone;
            userSection.Text = batch.Section;
            userSemester.Text = batch.Semester;
            userUniversity.Text = batch.University;
            userDepartment.Text = batch.Department;
        }

        private void TeacherCallBtn_Clicked(object sender, System.EventArgs e)
        {

        }

        private void StudentCallBtn_Clicked(object sender, System.EventArgs e)
        {

        }
    }
}