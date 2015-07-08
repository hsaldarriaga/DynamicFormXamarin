using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DynamicForm3.Controls
{
    public class FieldDouble : Field<Double?>
    {
        public FieldDouble(String id, String caption) : base(caption, id, FIELD_TYPES.DOUBLE)
        {
            customEntry = new CustomEntry
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                KeyBoardType = "Numeric",
                Keyboard =Keyboard.Numeric
            };
            customEntry.TextChanged += (sender, e) =>
            {
                FieldChanging();
            };
            Children.Add(customEntry);
        }

        public override Double? getValue()
        {
            Double value;
            if(Double.TryParse(customEntry.Text, out value))
                return value;
            return null;
        }

        public override void setValue(Double? value)
        {
            if (value.HasValue)
            {
                customEntry.Text = value.Value + "";
                FieldChanging();
            }
        }

        private CustomEntry customEntry;
    }
}
