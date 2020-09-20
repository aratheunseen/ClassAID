using Android.Widget;
using ClassAid.Droid.Models;
[assembly: Xamarin.Forms.Dependency(typeof(Toast_Android))]
namespace ClassAid.Droid.Models
{
    public class Toast_Android : ClassAid.Models.Toast
    {
        public void Show(string message)
        {
            Toast.MakeText(Android.App.Application.Context, message, ToastLength.Long).Show();
        }
    }
}