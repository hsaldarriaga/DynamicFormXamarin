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

namespace DynamicForm3.Droid.CustomRender
{
    class ListViewOnTouchListener : Java.Lang.Object, View.IOnTouchListener
    {
        public bool OnTouch(View v, MotionEvent e)
        {
            var lv = v as ListView;

            if (lv.Adapter.Count > 0)
            {
                switch (e.Action)
                {
                    case MotionEventActions.Down:
                        v.Parent.RequestDisallowInterceptTouchEvent(true);
                        break;
                    case MotionEventActions.Up:
                        v.Parent.RequestDisallowInterceptTouchEvent(false);
                        break;
                }
            }
            v.OnTouchEvent(e);
            return true;
        }
    }
}