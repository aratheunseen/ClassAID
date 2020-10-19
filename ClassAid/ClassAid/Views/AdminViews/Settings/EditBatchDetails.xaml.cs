using ClassAid.DataContex;
using ClassAid.Models;
using ClassAid.Models.Users;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClassAid.Views.AdminViews.Settings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditBatchDetails : ContentPage
    {
        public EditBatchDetails()
        {
            InitializeComponent();
            departmentBox.Text = App.Admin.BatchDetails.Department;
            semesterBox.Text = App.Admin.BatchDetails.Semester;
            sectionBox.Text = App.Admin.BatchDetails.Section;
            universityBox.Text = App.Admin.BatchDetails.University;
            saveButton.Command = new Command(() =>
            {
                if (!string.IsNullOrWhiteSpace(departmentBox.Text)
                && !string.IsNullOrWhiteSpace(semesterBox.Text)
                && !string.IsNullOrWhiteSpace(sectionBox.Text)
                && !string.IsNullOrWhiteSpace(universityBox.Text))
                {
                    App.Admin.BatchDetails.Department = departmentBox.Text;
                    App.Admin.BatchDetails.University = universityBox.Text;
                    App.Admin.BatchDetails.Semester = semesterBox.Text;
                    App.Admin.BatchDetails.Section = sectionBox.Text;
                    App.UpdateAdminOrSync(App.Admin);
                    LocalDbContex.SaveBatchDetails(App.Admin.BatchDetails);
                }
            });
        }
    }
}