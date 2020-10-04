using Android.Content;
using Android.Content.Res;
using Android.Graphics.Drawables;
using ClassAid.CustomComponents;
using ClassAid.Droid.ComponentsRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ClassAidEntry), typeof(CustomEntryRenderer))]
namespace ClassAid.Droid.ComponentsRenderer
{
    public class CustomEntryRenderer : EntryRenderer
    {
        public CustomEntryRenderer(Context context) : base(context)
        {

        }

        [System.Obsolete]
#pragma warning disable CS0809 // Obsolete member overrides non-obsolete member
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
#pragma warning restore CS0809 // Obsolete member overrides non-obsolete member
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Android.Graphics.Color placeHolderColor = new Android.Graphics.Color(200,198,196);
                GradientDrawable gd = new GradientDrawable();                
                gd.SetColor(Android.Graphics.Color.Transparent);
                Control.SetBackgroundDrawable(gd);
                //Control.SetRawInputType(InputTypes.TextFlagNoSuggestions);
                Control.SetHintTextColor(ColorStateList.ValueOf(placeHolderColor));
            }
        }
    }
}