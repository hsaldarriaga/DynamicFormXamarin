using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DynamicForm3.Controls
{
    public abstract class Field<T> : StackLayout
    {
        public String Caption { get; private set; }
        public String Field_id { get; private set; }
        public readonly FIELD_TYPES type;
        protected CaptionLabel label;

        protected Field(String Caption, String Id, FIELD_TYPES tp) : base()
        {
            VerticalOptions = LayoutOptions.Start;
            this.type = tp;
            this.Caption = Caption;
            this.Field_id = Id;
            label = new CaptionLabel
            {
                HorizontalOptions = LayoutOptions.Start,
                Text = Caption
            };
            Spacing = 5;
            Padding = 5;
            Children.Add(label);
        }

        public abstract T getValue();

        public abstract void setValue(T value);

        public PropertyChangingEventHandler FieldValueChanged;

        protected void FieldChanging()
        {
            if (FieldValueChanged != null)
            {
                PropertyChangingEventArgs prop = new PropertyChangingEventArgs("Value");
                FieldValueChanged.Invoke(this, prop);
            }
        }

        public FIELD_TYPES getType()
        {
            return type;
        }
    }
}
