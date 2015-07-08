using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly:ExportRenderer(typeof(DynamicForm3.CaptionLabel), typeof(DynamicForm3.Droid.CustomRender.CaptionLabelAndroid))]
namespace DynamicForm3.Droid.CustomRender
{
    public class CaptionLabelAndroid : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Label> e)
        {
            base.OnElementChanged(e);
        }
    }
}