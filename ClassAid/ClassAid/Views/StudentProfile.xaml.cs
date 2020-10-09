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

namespace ClassAid.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StudentProfile : ContentPage
    {
        public Admin Admin { get; set; }
        public StudentProfile(Admin admin)
        {
            Admin = admin;
            InitializeComponent();
            logoutBtn.Command = new Command(() => App.LogOut());
            AllocateRequestList(admin.AdminKey);
        }
        public StudentProfile(Student student)
        {
            InitializeComponent();
            logoutBtn.Command = new Command(() => App.LogOut());
            Admin.StudentList = new ObservableCollection<Student>(LocalDbContex.GetStudents());
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
               Task.Run(async ()=> Admin = await FirebaseHandler.GetAdminAsync(student.AdminKey));
            }
            else
            {
                Admin.BatchDetails = LocalDbContex.GetBatchDetails();
                Admin.StudentList = 
                    new ObservableCollection<Student>(LocalDbContex.GetStudents());
            }
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
        private void Name_Scrolled(object sender, ItemsViewScrolledEventArgs e)
        {

        }

        private void AcceptBtn_Clicked(object sender, EventArgs e)
        {

        }

        private void RejectBtn_Clicked(object sender, EventArgs e)
        {

        }
    }
}