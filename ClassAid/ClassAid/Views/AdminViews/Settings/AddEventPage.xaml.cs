using ClassAid.DataContex;
using ClassAid.Models.Schedule;
using ClassAid.Models.Users;
using Firebase.Database;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ClassAid.Views.AdminViews.Settings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddEventPage : ContentPage
    {
        private readonly Admin admin;
        private readonly FirebaseClient client;

        public AddEventPage()
        {
        }

        public AddEventPage(Admin admin)
        {
            InitializeComponent();
            this.admin = admin;
            this.client = App.fireSharpClient.GetClient();
        }

        private async void saveEvent_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
            if (admin.EventList == null)
            {
                admin.EventList =
                    new System.Collections.ObjectModel.ObservableCollection<EventModel>();
            }
            admin.EventList.Add(
                new EventModel()
                {
                    Title = eventTitle.Text,
                    Details = eventBody.Text
                });
            await AdminDbHandler.UpdateAdmin(client, admin);
        }
    }
}