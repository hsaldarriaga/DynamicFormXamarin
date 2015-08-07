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

[assembly: Xamarin.Forms.ExportRenderer(typeof(DynamicForm3.CustomExpandableListView), typeof(DynamicForm3.Droid.CustomRender.ExpandableListViewDroid))]
namespace DynamicForm3.Droid.CustomRender
{
    public class ExpandableListViewDroid : ViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.View> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                var lv = e.NewElement as DynamicForm3.CustomExpandableListView;
                handler = lv.OnItemClick;
                var newlv = new ExpandableListView(Xamarin.Forms.Forms.Context);
                var adapter = new CustomRender.Helpers.ExpandableAdapter(Xamarin.Forms.Forms.Context, lv.Items);
                newlv.SetAdapter(adapter);
                newlv.ChildClick += (object sender, ExpandableListView.ChildClickEventArgs ev) =>
                {
                    int[] values = new int[2];
                    values[0] = ev.GroupPosition; values[1] = ev.ChildPosition;
                    handler(values, null);
                };
                SetNativeControl(newlv);
            }
        }

        private EventHandler handler;
    }
}