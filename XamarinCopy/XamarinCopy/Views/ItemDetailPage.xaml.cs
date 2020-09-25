using System.ComponentModel;
using Xamarin.Forms;
using XamarinCopy.ViewModels;

namespace XamarinCopy.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}