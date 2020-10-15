
using ClassAid.DataContex;
using ClassAid.Models.Schedule;
using ClassAid.Models.Users;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClassAid.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewEventPage : ContentPage
    {
        public ViewEventPage()
        {
            InitializeComponent();
            eventListView.ItemsSource = App.Admin.EventList;
            
        }

        private void OnCollectionViewScrolled(object sender, ItemsViewScrolledEventArgs e)
        {

        }
    }
}