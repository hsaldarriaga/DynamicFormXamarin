using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DynamicForm3.Controls
{
    public class FieldLookUp : Field<int>, Pages.IFormPageItemSelected
    {
        public FieldLookUp(String id, String caption, Dictionary<string, int> values): base(caption, id, FIELD_TYPES.LOOKUP_FIXED)
        {
            AllValues = values;
            bt = new Button
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Text = "Seleccionar"
            };
            bt.Clicked += bt_Clicked;
            Children.Add(bt);
        }

        void bt_Clicked(object sender, EventArgs e)
        {
            var pg = new Pages.PageLookUp(AllValues);
            pg.ItemSelected = this;
            Navigation.PushAsync(pg);
        }

        public void SelectedItem(object key, object value)
        {
            this.key = key.ToString();
            this.value = Int32.Parse(value.ToString());
            bt.Text = this.key;
            FieldChanging();
        }

        public override int getValue()
        {
            return value;
        }

        public override void setValue(int value)
        {
            bt.Text = AllValues.First(e => e.Value == value).Key;
            this.value = value;
        }

        private Dictionary<string, int> AllValues;
        private Button bt;
        private string key;
        private int value = -1;
    }
}
