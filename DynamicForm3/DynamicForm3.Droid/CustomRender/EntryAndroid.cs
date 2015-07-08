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

[assembly: ExportRenderer(typeof(DynamicForm3.CustomEntry), typeof(DynamicForm3.Droid.CustomRender.EntryAndroid))]
namespace DynamicForm3.Droid.CustomRender
{
    public class EntryAndroid : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                DynamicForm3.CustomEntry entry = (DynamicForm3.CustomEntry)e.NewElement;
                var tv = Control as TextView;
                if (entry.KeyBoardType == "Numeric")
                {
                    tv.InputType = Android.Text.InputTypes.ClassNumber | Android.Text.InputTypes.NumberFlagSigned;
                }
                else if (entry.KeyBoardType == "Decimal")
                {
                    tv.InputType = Android.Text.InputTypes.ClassNumber | Android.Text.InputTypes.NumberFlagDecimal | Android.Text.InputTypes.NumberFlagSigned;
                }
                else if (entry.KeyBoardType == "Text")
                {
                    tv.InputType = Android.Text.InputTypes.ClassText | Android.Text.InputTypes.TextFlagCapSentences | Android.Text.InputTypes.TextFlagAutoCorrect;
                }
            }
        }
    }
}