using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using ClassAid.CustomComponents;
using ClassAid.Droid.ComponentsRenderer;
using System.Drawing;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ClassAidButton),typeof(CustomButtonRenderer))]
namespace ClassAid.Droid.ComponentsRenderer
{
    public class CustomButtonRenderer : ButtonRenderer
    {
        public CustomButtonRenderer(Context context) :base(context)
        {

        }
        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                GradientDrawable drawable = new GradientDrawable();
                drawable.SetCornerRadius(5);
                drawable.SetColor(Android.Graphics.Color.Transparent);
                Pen blackPen = new Pen(System.Drawing.Color.FromArgb(255, 0, 0, 0), 5);
                //drawable.DrawRectangle(blackPen, 10, 10, 100, 50);
                drawable.SetBounds(1, 1, 1, 1);
            }
        }
    }
}