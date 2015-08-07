using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DynamicForm3.Controls
{
    public class FieldDecimal : Field<decimal?>
    {
        public FieldDecimal(String id, String caption) : base(caption, id, FIELD_TYPES.DECIMAL)
        {
            customEntry = new CustomEntry
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                KeyBoardType = "Decimal"
            };
            customEntry.Unfocused += (sender, e) =>
            {
                FieldChanging();
            };
            Children.Add(customEntry);
        }

        public override decimal? getValue()
        {
            decimal result;
            if (decimal.TryParse(customEntry.Text, out result))
            {
                return result;
            }
            return null;
        }

        public override void setValue(decimal? value)
        {
            if (value.HasValue)
            {
                customEntry.Text = value.Value + "";
            }
        }

        private CustomEntry customEntry;
    }
}
