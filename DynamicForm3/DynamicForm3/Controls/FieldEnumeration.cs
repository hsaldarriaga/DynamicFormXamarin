using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DynamicForm3.Controls
{
    public class FieldEnumeration : Field<int>
    {
        public FieldEnumeration(String caption, String id, Dictionary<string, int> values) : base(caption, id, FIELD_TYPES.FIX_VALUE)
        {
            this.values = values;
            picker = new Picker
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Title = "Seleccionar Valor"
            };
            foreach (var item in values)
            {
                picker.Items.Add(item.Key);
            }
            picker.SelectedIndexChanged += (sender, e) =>
            {
                FieldChanging();
            };
            Children.Add(picker);
        }

        public override int getValue()
        {
            if (picker.SelectedIndex != -1)
                return values[picker.Items[picker.SelectedIndex]];
            return -1;
        }

        public override void setValue(int value)
        {
            picker.SelectedIndex = value;
        }
        private Picker picker;
        private Dictionary<string, int> values;
    }
}
