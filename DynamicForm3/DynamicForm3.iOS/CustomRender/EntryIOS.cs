using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

using Xamarin.Forms.Platform.iOS;
[assembly: Xamarin.Forms.ExportRenderer(typeof(DynamicForm3.CustomEntry), typeof(DynamicForm3.iOS.CustomRender.EntryIOS))]
namespace DynamicForm3.iOS.CustomRender
{
    public class EntryIOS : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Entry> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                DynamicForm3.CustomEntry entry = (DynamicForm3.CustomEntry)e.NewElement;
                var tv = Control as UITextField;
                if (entry.KeyBoardType == "Numeric")
                {
                    tv.KeyboardType = UIKeyboardType.NumberPad;
                }
                else if (entry.KeyBoardType == "Decimal")
                {
                    tv.KeyboardType = UIKeyboardType.DecimalPad;
                }
                else if (entry.KeyBoardType == "Text")
                {
                    tv.KeyboardType = UIKeyboardType.Default;
                }
            }
        }
    }
}