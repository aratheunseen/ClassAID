using ClassAid.DataContex;
using ClassAid.Models.Schedule;
using ClassAid.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClassAid.Views.StudentViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Profile : ContentPage
    {
        public Student Student { get; set; }
        public Profile()
        {
            Student = LocalDbContex.GetStudentAsUser();
            BatchDetails batch = LocalDbContex.GetBatchDetails();
            //List<Student> students = LocalDbContex.GetStudents();
            //List<Teacher> teachers = LocalDbContex.GetTeachers();
            InitializeComponent();
            logoutBtn.Command = new Command(() => App.LogOut());
            userName.Text = Student.Name;
            userID.Text = Student.ID;
            userPhone.Text = Student.Phone;
            userSection.Text = batch.Section;
            userSemester.Text = batch.Semester;
            userUniversity.Text = batch.University;
            userDepartment.Text = batch.Department;
        }
    }
}