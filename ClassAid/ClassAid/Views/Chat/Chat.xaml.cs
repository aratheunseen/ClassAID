using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ClassAid.DataContex;
using System.Collections.ObjectModel;
using ClassAid.Models;
using System.Collections.Specialized;
using Xamarin.Essentials;
using ClassAid.Models.Users;
using System.Diagnostics;
using System.Timers;

namespace ClassAid.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatHub : ContentPage
    {
        private readonly string key;
        private readonly string Name;
        private readonly string ID;
        private static Timer SyncTimer;
        public ChatHub(string key, string Name, string ID)
        {
            InitializeComponent();
            this.key = key;
            this.Name = Name;
            this.ID = ID;

            FirebaseHandler.RealTimeChat(App.Chats);
            SyncTimer = new Timer(2000);
            SyncTimer.Start();
            SyncTimer.Elapsed += ATimer_Elapsed; ;

            ChatViewBox.ItemsSource = App.Chats;
            messageBox.Completed += SendButton_Clicked;
        }

        private void ATimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (App.Chats.Count < 1)
                FirebaseHandler.RealTimeChat(App.Chats);
            if(App.Chats.Count > 0)
                SyncTimer.Stop();
        }

        private void SendButton_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(messageBox.Text))
            {
                ChatModel chat = new ChatModel()
                {
                    Message = messageBox.Text,
                    SenderKey = key,
                    Sender = Name,
                    Time = DateTime.Now.ToString("MMM dd, hh:mm tt"),
                    AdminKey = Preferences.Get(PrefKeys.AdminKey, ""),
                    ID = ID
                };
                messageBox.Text = string.Empty;
                //App.Chats.Add(chat);
                //LocalDbContex.SaveChat(chat);
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                    FirebaseHandler.SendMessage(chat);
                else
                    DependencyService.Get<Toast>().Show("No INTERNET");
            }
        }
        private void MessageBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(messageBox.Text))
                sendButton.IsEnabled = false;
            else
                sendButton.IsEnabled = true;
        }
    }
}
