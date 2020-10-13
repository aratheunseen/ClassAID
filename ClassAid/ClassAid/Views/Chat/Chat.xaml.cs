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

namespace ClassAid.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatHub : ContentPage
    {
        private readonly string key;
        private readonly string Name;
        private readonly string ID;
        private readonly ObservableCollection<ChatModel> chats;
        public ChatHub(string key, string Name, string ID)
        {
            InitializeComponent();
            Debug.WriteLine(Preferences.Get(PrefKeys.AdminKey, ""));
            this.key = key;
            this.Name = Name;
            this.ID = ID;
            chats = new ObservableCollection<ChatModel>();
            FirebaseHandler.RealTimeChat(chats);
            ChatViewBox.ItemsSource = chats;
            messageBox.Completed += SendButton_Clicked;
            chats.CollectionChanged += Chats_CollectionChanged;
            ChatViewBox.ChildAdded += ChatViewBox_ChildAdded;
        }

        private void ChatViewBox_ChildAdded(object sender, ElementEventArgs e)
        {
            //chatBoxGrid.Bindings.
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
                FirebaseHandler.SendMessage(chat);
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
