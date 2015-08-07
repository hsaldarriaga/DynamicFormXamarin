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
using DynamicForms3.Droid;

namespace DynamicForm3.Droid.CustomRender.Helpers
{
    public class ExpandableAdapter : BaseExpandableListAdapter
    {
        private LayoutInflater inflater;
        private List<DynamicForm3.Models.BOHeaders> items;
        public ExpandableAdapter(Context c, List<DynamicForm3.Models.BOHeaders> items)
        {
            inflater = LayoutInflater.FromContext(c);
            this.items = items;
        }

        public override View GetGroupView(int groupPosition, bool isExpanded, View convertView, ViewGroup parent)
        {
            var header = convertView as View;
            if (header == null)
            {
                header = inflater.Inflate(Resource.Layout.GroupExpandable, null) as View;
            }
            header.FindViewById<TextView>(Resource.Id.DataHeader).Text = items[groupPosition].Name;
            return header;
        }

        public override View GetChildView(int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent)
        {
            var row = convertView as View;
            if (row == null)
            {
                row = inflater.Inflate(Resource.Layout.ChildExpandable, null) as View;
            }
            row.FindViewById<TextView>(Resource.Id.DataRow).Text = items[groupPosition][childPosition];
            return row;
        }
        #region "Another Implementations"
        
        public override Java.Lang.Object GetChild(int groupPosition, int childPosition)
        {
            return items[groupPosition][childPosition];
        }

        public override long GetChildId(int groupPosition, int childPosition)
        {
            return 1 + groupPosition + childPosition;
        }

        public override int GetChildrenCount(int groupPosition)
        {
            return items[groupPosition].Count;
        }

        public override Java.Lang.Object GetGroup(int groupPosition)
        {
            return items[groupPosition].ToArray<string>();
        }

        public override long GetGroupId(int groupPosition)
        {
            return groupPosition;
        }

        public override int GroupCount
        {
            get { return items.Count; }
        }

        public override bool HasStableIds
        {
            get { return true; }
        }

        public override bool IsChildSelectable(int groupPosition, int childPosition)
        {
            return true;
        }
        #endregion
    }
}