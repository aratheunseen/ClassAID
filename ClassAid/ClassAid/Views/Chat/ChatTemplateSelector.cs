using ClassAid.Models;
using ClassAid.Models.Users;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ClassAid.Views.Chat
{
    class ChatTemplateSelector : DataTemplateSelector
    {
        DataTemplate incomingDataTemplate;
        DataTemplate outgoingDataTemplate;
        public ChatTemplateSelector()
        {
            incomingDataTemplate = new DataTemplate(typeof(IncomingViewCell));
            outgoingDataTemplate = new DataTemplate(typeof(OutgoingViewCell));
        }
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var message = item as ChatModel;
            if (message == null)
                return null;
            return message.SenderKey == Preferences.Get(PrefKeys.Key, "")
                ? outgoingDataTemplate
                : incomingDataTemplate;
        }
    }
}
