using ClassAid.DataContex;
using ClassAid.Models.Schedule;
using ClassAid.Models.Users;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using ClassAid.Models;

namespace ClassAid.Views.AdminViews.Settings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddTeacherPage : ContentPage
    {
        public AddTeacherPage()
        {
            InitializeComponent();
        }
        private async void AddTeacherBtn_Clicked()
        {
            var t = (new Teacher()
            {
                Name = teacherName.Text,
                Designation = teacherDesegnation.Text,
                Phone = teacherPhoneNumber.Text
            });
            await Navigation.PopAsync();
            LocalDbContex.SaveTeacher(t);
            App.Admin.TeacherList.Add(t);
            App.UpdateAdminOrSync(App.Admin);
        }
        
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