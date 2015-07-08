using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DynamicForm3.Controls
{
    class FieldLabel : Field<Object>
    {
        public FieldLabel(String caption, String id) : base(caption, id, FIELD_TYPES.LABEL)
        {
            ((Label)Children[0]).FontSize = 18;
        }

        public override object getValue()
        {
            return null;
        }

        public override void setValue(object value)
        {

        }
    }
}
