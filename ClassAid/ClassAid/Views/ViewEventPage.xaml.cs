using ClassAid.DataContex;
using ClassAid.Models;
using ClassAid.Models.Schedule;
using ClassAid.Models.Users;
using ClassAid.Views.AdminViews.Settings;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Xamarin.Essentials;
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
            if (Preferences.Get(PrefKeys.IsAdmin, false))
                addEventBtn.Clicked += AddEventBtn_Clicked;
            else
                mainGrid.Children.Remove(addEventBtn);
        }

        private async void AddEventBtn_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new AddEventPage());
        }

        private void OnCollectionViewScrolled(object sender, ItemsViewScrolledEventArgs e)
        {

        }
    }
}