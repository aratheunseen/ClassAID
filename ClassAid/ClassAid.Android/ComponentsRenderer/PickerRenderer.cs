using Android.Content;
using Android.Content.Res;
using Android.Graphics.Drawables;
using ClassAid.CustomComponents;
using ClassAid.Droid.ComponentsRenderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ClassAidPicker), typeof(CustomPickerRenderer))]
namespace ClassAid.Droid.ComponentsRenderer
{
    class CustomPickerRenderer : PickerRenderer
    {
        [System.Obsolete]
        public CustomPickerRenderer(Context context) : base(context)
        {

        }

        [System.Obsolete]
#pragma warning disable CS0809 
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
#pragma warning restore CS0809 
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Android.Graphics.Color placeHolderColor = new Android.Graphics.Color(200, 198, 196);
                GradientDrawable gd = new GradientDrawable();
                gd.SetColor(Android.Graphics.Color.Transparent);
                Control.SetBackgroundDrawable(gd);
                Control.SetHintTextColor(ColorStateList.ValueOf(placeHolderColor));
            }
        }
    }
}