using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DynamicForm3.Controls
{
    public class FieldDateTime : Field<DateTime>
    {
        public FieldDateTime(String caption, String id) : base(caption, id, FIELD_TYPES.DATETIME)
        {
            picker = new DatePicker
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Format = "dd/MM/yyyy"
            };
            Children.Add(picker);
            picker.DateSelected += (sender, e) =>
            {
                FieldChanging();
            };
        }

        public override DateTime getValue()
        {
            return picker.Date;
        }
        public override void setValue(DateTime value)
        {
            picker.Date = value;
            FieldChanging();
        }
        private DatePicker picker;
    }
}
