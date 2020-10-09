using Android.Telecom;
using ClassAid.DataContex;
using ClassAid.Models.Schedule;
using ClassAid.Models.Users;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using ClassAid.Models;

namespace ClassAid.Views.AdminViews.Settings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddTeacherPage : ContentPage
    {
        private readonly Admin admin;
        public AddTeacherPage(Admin admin)
        {
            InitializeComponent();
            this.admin = admin;
        }
        private async void AddTeacherBtn_Clicked()
        {
            if (admin.TeacherList == null)
                admin.TeacherList = new ObservableCollection<Teacher>();
            var t = (new Teacher()
            {
                Name = teacherName.Text,
                Designation = teacherDesegnation.Text,
                Phone = teacherPhoneNumber.Text
            });
            await Navigation.PopAsync();
            LocalDbContex.SaveTeacher(t);
            admin.TeacherList.Add(t);
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                FirebaseHandler.UpdateAdmin(admin);
            else 
            {
                Preferences.Set(PrefKeys.IsSyncPending, true);
                LocalStorageEngine.SaveDataAsync(admin, FileType.Admin);
                DependencyService.Get<Toast>().Show("No internet access. Sync pending.");
            }
        }
        
        // TODO: Can not build after change the button to frame gesture

        private void Form_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(teacherName.Text) ||
                string.IsNullOrWhiteSpace(teacherDesegnation.Text))
                addTeacherBtn.Command= null;
            else
                addTeacherBtn.Command = new Command(()=> AddTeacherBtn_Clicked());
        }
    }
}