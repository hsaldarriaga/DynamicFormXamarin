using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

using Xamarin.Forms.Platform.iOS;


[assembly: Xamarin.Forms.ExportRenderer(typeof(DynamicForm3.CustomExpandableListView), typeof(DynamicForm3.iOS.CustomRender.ExpandableTableView))]
namespace DynamicForm3.iOS.CustomRender
{
    public class ExpandableTableView : ViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.View> e)
        {
            base.OnElementChanged(e);
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                var lv = e.NewElement as DynamicForm3.CustomExpandableListView;
                UITableView tb = new UITableView();
                tb.Source = new CustomRender.Helpers.TableSource(lv.Items, lv.OnItemClick);
                SetNativeControl(tb);
            }
        }
    }
}