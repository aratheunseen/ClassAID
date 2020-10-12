using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ClassAid.DataContex;
using System.Collections.ObjectModel;
using ClassAid.Models;
using System.Collections.Specialized;
using Xamarin.Essentials;
using ClassAid.Models.Users;

namespace ClassAid.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatHub : ContentPage
    {
        private readonly string key;
        private readonly string Name;
        private readonly ObservableCollection<ChatModel> chats;
        public ChatHub(string key, string Name)
        {
            InitializeComponent();
            this.key = key;
            this.Name = Name;
            chats = new ObservableCollection<ChatModel>();
            FirebaseHandler.RealTimeChat(chats);
            ChatViewBox.ItemsSource = chats;
            messageBox.Completed += SendButton_Clicked;
            chats.CollectionChanged += Chats_CollectionChanged;
        }

        private void Chats_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (chats[e.NewStartingIndex].SenderKey == key)
            {
                // Do something.
            }
        }

        private void SendButton_Clicked(object sender, EventArgs e)
        {
            ChatModel chat = new ChatModel()
            {
                Message = messageBox.Text,
                SenderKey = key,
                Sender = Name,
                Time = DateTime.Now.ToString("MMM dd, hh:mm tt"),
                AdminKey = Preferences.Get(PrefKeys.AdminKey, "")
            };
            messageBox.Text = string.Empty;
            FirebaseHandler.SendMessage(chat);
        }
    }
}
