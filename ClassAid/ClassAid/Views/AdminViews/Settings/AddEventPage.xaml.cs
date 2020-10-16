using ClassAid.DataContex;
using ClassAid.Models;
using ClassAid.Models.Engines;
using ClassAid.Models.Schedule;
using ClassAid.Models.Users;
using System;
using System.Collections.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClassAid.Views.AdminViews.Settings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddEventPage : ContentPage
    {
        public AddEventPage()
        {
            InitializeComponent();
        }
        private async void SaveEvent_Clicked()
        {
            await Navigation.PopAsync();
            var e = new EventModel()
            {
                Title = eventTitle.Text,
                Details = eventBody.Text,
                Time = DateTime.Now.ToString(@"dd\:hh\:mm\ t")
            };
            App.Admin.EventList.Insert(0, e);
            if (App.Admin.EventList.Count > 10)
            {
                LocalDbContex.DeleteEvent(App.Admin.EventList[10]);
                App.Admin.EventList.RemoveAt(10);
            }
            LocalDbContex.SaveEvent(e);
            App.UpdateAdminOrSync();
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                PushNotification.Send(e.Title, e.Details, App.Admin.Key);
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
