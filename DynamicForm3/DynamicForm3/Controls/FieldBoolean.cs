using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DynamicForm3.Controls
{
    public class FieldBoolean : Field<bool?>
    {
        public FieldBoolean(String caption, String id) : base(caption, id, FIELD_TYPES.BOOLEAN)
        {
            Picker = new Picker
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                Title = "Seleccionar",
                Items = {
                    "Verdadero","Falso"
                }
            };
            Picker.SelectedIndexChanged += (sender, e) =>
            {
                FieldChanging();
            };
            Children.Add(Picker);
        }

        public override bool? getValue()
        {
            if (Picker.SelectedIndex == 0)
                return true;
            else if (Picker.SelectedIndex == 1)
                return false;
            return null;
        }

        public override void setValue(bool? value)
        {
            if (value.HasValue)
            {
                if (value.Value)
                    Picker.SelectedIndex = 0;
                else
                    Picker.SelectedIndex = 1;
                FieldChanging();
            }
        }

        private Picker Picker;
    }
}
