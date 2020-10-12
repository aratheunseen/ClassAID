using ClassAid.Models.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Essentials;

namespace ClassAid.Models
{
    public class ChatModel
    {
        public string Message { get; set; }
        public string Sender { get; set; }
        public string SenderKey { get; set; }
        public string AdminKey { get; set; }
        public string Time { get; set; }
    }
}
