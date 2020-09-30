
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
        public ViewEventPage(Shared admin)
        {
            InitializeComponent();
            if (admin.EventList != null)
            {
                eventListView.ItemsSource = admin.EventList;
            }
        }
    }
}