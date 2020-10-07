using ClassAid.DataContex;
using ClassAid.Models.Users;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClassAid.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StudentProfile : ContentPage
    {
        public Shared User { get; }
        public StudentProfile(Shared user)
        {
            User = user;
            InitializeComponent();
            logoutBtn.Command = new Command(() => Logout());
            AllocateRequestList(user.AdminKey);
        }
        private async void AllocateRequestList(string key)
        {
            //var data = await FirebaseHandler.GetPendingStudents(key);
            //if (data!=null)
            //    RequestCollectionView.ItemsSource = data.ToList();
            List<Shared> shareds = new List<Shared>()
            {
                new Shared() { Name = "Shaikh Hasina", ID = "192311048", Phone = "0123567890" },
                new Shared() { Name = "Mahmud", ID = "192311002", Phone = "0123567891" },
                new Shared() { Name = "Alif", ID = "192311018", Phone = "0123567892" },
                new Shared() { Name = "Khaleda Jia", ID = "19221148", Phone = "0123567893" },
            };
            RequestCollectionView.ItemsSource = shareds;
        }
        private void Logout()
        {
            Preferences.Set(PrefKeys.isLoggedIn, false);
            LocalStorageEngine.ClearData(FileType.Shared);
            Application.Current.MainPage =
                new NavigationPage(new StartPage());
        }

        private void Name_Scrolled(object sender, ItemsViewScrolledEventArgs e)
        {

        }

        private void AcceptBtn_Clicked(object sender, System.EventArgs e)
        {

        }

        private void RejectBtn_Clicked(object sender, System.EventArgs e)
        {

        }
    }
}