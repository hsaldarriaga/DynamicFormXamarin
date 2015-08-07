using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace DynamicForm3.iOS.CustomRender.Helpers
{
    public class TableSource : UITableViewSource
    {
        private List<DynamicForm3.Models.BOHeaders> Items;
        private string CellIdentifier = "TableCell";
        private EventHandler Handler;
        private bool[] Expanded;
        public TableSource(List<DynamicForm3.Models.BOHeaders> items, EventHandler handler)
        {
            Items = items;
            Handler = handler;
            Expanded = new bool[Items.Count];
            for (int i = 0; i < Expanded.Length; i++)
            {
                Expanded[i] = true;
            }
        }

        public override void AccessoryButtonTapped(UITableView tableView, NSIndexPath indexPath)
        {
            int[] values = new int[2];
            values[0] = indexPath.Section; values[1] = indexPath.Row;
            Handler(values, null);
        }

        public override UIView GetViewForHeader(UITableView tableView, nint section)
        {
            UIButton bt = new UIButton(UIButtonType.DetailDisclosure);
            bt.SetTitle(Items[(int)section].Name, UIControlState.Normal);
            bt.TouchUpInside += (Sender, args) =>
            {
                Expanded[section] = !Expanded[section];
                tableView.ReloadSections(NSIndexSet.FromIndex(section), UITableViewRowAnimation.Fade);
            };
            return bt;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewCell cell = tableView.DequeueReusableCell(CellIdentifier);
            var item = Items[indexPath.Section][indexPath.Row];
            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Default, CellIdentifier);
                cell.Accessory = UITableViewCellAccessory.DetailDisclosureButton;
            }
            cell.TextLabel.Text = item;

            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            if (Expanded[section])
                return Items[(int)section].Count;
            else
                return 0;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return Items.Count;
        }

    }
}