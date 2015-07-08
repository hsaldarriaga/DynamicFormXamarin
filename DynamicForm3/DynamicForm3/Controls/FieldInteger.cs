using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DynamicForm3.Controls
{
    public class FieldInteger : Field<int?>
    {
        public FieldInteger(String id, String caption) : base(caption, id, FIELD_TYPES.INTEGER)
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

        public override int? getValue()
        {
            int value;
            if(Int32.TryParse(customEntry.Text, out value))
                return value;
            return null;
        }

        public override void setValue(int? value)
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
