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

using Xamarin.Forms.Platform.Android;

[assembly: Xamarin.Forms.ExportRenderer(typeof(DynamicForm3.CustomListView), typeof(DynamicForm3.Droid.CustomRender.ListViewAndroid))]
namespace DynamicForm3.Droid.CustomRender
{
    public class ListViewAndroid : ListViewRenderer
    {
        public ListViewAndroid() { }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ListView> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                var lv = Control as ListView;
                lv.SetOnTouchListener(new ListViewOnTouchListener());
            }
        }
    }
}