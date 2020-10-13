using ClassAid.DataContex;
using ClassAid.Models.Users;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClassAid.Views.AdminViews.Settings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditBatchDetails : ContentPage
    {
        public EditBatchDetails(Admin admin)
        {
            InitializeComponent();
            departmentBox.Text = admin.BatchDetails.Department;
            semesterBox.Text = admin.BatchDetails.Semester;
            sectionBox.Text = admin.BatchDetails.Section;
            universityBox.Text = admin.BatchDetails.University;
            saveButton.Command = new Command(() =>
            {
                if (!string.IsNullOrWhiteSpace(departmentBox.Text)
                && !string.IsNullOrWhiteSpace(semesterBox.Text)
                && !string.IsNullOrWhiteSpace(sectionBox.Text)
                && !string.IsNullOrWhiteSpace(universityBox.Text))
                {
                    admin.BatchDetails.Department = departmentBox.Text;
                    admin.BatchDetails.University = universityBox.Text;
                    admin.BatchDetails.Semester = semesterBox.Text;
                    admin.BatchDetails.Section = sectionBox.Text;
                    FirebaseHandler.UpdateAdmin(admin);
                }
            });
        }
    }
}