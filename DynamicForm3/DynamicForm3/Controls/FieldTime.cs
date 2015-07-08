using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DynamicForm3.Controls
{
    public class FieldTime : Field<TimeSpan>
    {
        public FieldTime(String id, String caption) : base(caption, id, FIELD_TYPES.TIME)
        {
            timePicker = new TimePicker
            {
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            timePicker.PropertyChanged += timePicker_PropertyChanged;
            Children.Add(timePicker);
        }

        void timePicker_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Time")
                FieldChanging();
        }

        public override TimeSpan getValue()
        {
            return timePicker.Time;
        }

        public override void setValue(TimeSpan value)
        {
            timePicker.Time = value;
        }

        private TimePicker timePicker;
    }
}
