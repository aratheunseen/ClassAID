using ClassAid.DataContex;
using ClassAid.Models;
using ClassAid.Models.Engines;
using ClassAid.Models.Schedule;
using ClassAid.Models.Users;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClassAid.Views.AdminViews.Settings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddEventPage : ContentPage
    {
        private readonly Admin admin;

        public AddEventPage()
        {
        }

        public AddEventPage(Admin admin)
        {
            InitializeComponent();
            this.admin = admin;
        }

        private async void SaveEvent_Clicked()
        {
            await Navigation.PopAsync();
            if (admin.EventList == null)
            {
                admin.EventList =
                    new System.Collections.ObjectModel.ObservableCollection<EventModel>();
            }
            var e = new EventModel()
            {
                Title = eventTitle.Text,
                Details = eventBody.Text,
                Time = DateTime.Now.ToString(@"dd\:hh\:mm\t")
            };
            admin.EventList.Add(e);

            if (admin.EventList.Count > 10)
                admin.EventList.RemoveAt(10);

            LocalDbContex.SaveEvent(e);
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                FirebaseHandler.UpdateAdmin(admin);
                PushNotification.Send(e.Title, e.Details, admin.Key);
            }
            else
            {
                Preferences.Set(PrefKeys.IsSyncPending, true);
                LocalStorageEngine.SaveDataAsync(admin, FileType.Admin);
                DependencyService.Get<Toast>().Show("No internet access. Sync pending.");
            }
        }

        private void Form_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(eventTitle.Text) ||
                string.IsNullOrWhiteSpace(eventBody.Text))
                saveEvent.Command = null;
            else
                saveEvent.Command = new Command(() => SaveEvent_Clicked());
        }
    }
}
