using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DynamicForm3.Controls
{
    public class FieldString : Field<String>
    {
        public FieldString(String id, String caption) : base(caption, id, FIELD_TYPES.STRING)
        {
            customEntry = new CustomEntry
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                KeyBoardType = "Text"
            };
            customEntry.Unfocused += (sender, e) =>
            {
                FieldChanging();
            };
            Children.Add(customEntry);
        }

        public override string getValue()
        {
            return customEntry.Text;
        }

        public override void setValue(string value)
        {
            customEntry.Text = value;
        }

        private CustomEntry customEntry;
    }
}
