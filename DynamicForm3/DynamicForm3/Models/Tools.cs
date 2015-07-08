using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ciloci.Flee;
using DynamicForm3.Controls;

namespace DynamicForm3.Models
{
    class Tools
    {
        public Tools(ref Dictionary<string, FormField> fields)
        {
            this.fields = fields;
        }
        public void Variables_ResolveVariableType(object sender, ResolveVariableTypeEventArgs e)
        {
            var control = fields[e.VariableName];
            if (control.Field is FieldBoolean)
            {
                e.VariableType = typeof(bool);
            }
            else if (control.Field is FieldDecimal)
            {
                e.VariableType = typeof(Decimal);
            }
            else if (control.Field is FieldDouble)
            {
                e.VariableType = typeof(Double);
            }
            else if (control.Field is FieldEnumeration)
            {
                e.VariableType = typeof(Int32);
            }
            else if (control.Field is FieldInteger)
            {
                e.VariableType = typeof(Int32);
            }
            else if (control.Field is FieldLookUp)
            {
                e.VariableType = typeof(Int32);
            }
            else if (control.Field is FieldLookUpBO)
            {
                e.VariableType = typeof(string);
            }
            else if (control.Field is FieldString)
            {
                e.VariableType = typeof(string);
            }
        }

        public void Variables_ResolveVariableValue(object sender, ResolveVariableValueEventArgs e)
        {
            var control = fields[e.VariableName];
            if (control.Field is FieldBoolean)
            {
                var field = control.Field as FieldBoolean;
                bool? value = field.getValue();
                if (value.HasValue)
                    e.VariableValue = value.Value;
            }
            else if (control.Field is FieldDecimal)
            {
                var field = control.Field as FieldDecimal;
                decimal? value = field.getValue();
                if (value.HasValue)
                    e.VariableValue = value.Value;
            }
            else if (control.Field is FieldDouble)
            {
                var field = control.Field as FieldDouble;
                double? value = field.getValue();
                if (value.HasValue)
                    e.VariableValue = value.Value;
            }
            else if (control.Field is FieldEnumeration)
            {
                var field = control.Field as FieldEnumeration;
                e.VariableValue = field.getValue();
            }
            else if (control.Field is FieldInteger)
            {
                var field = control.Field as FieldInteger;
                e.VariableValue = field.getValue();
            }
            else if (control.Field is FieldLookUp)
            {
                var field = control.Field as FieldLookUp;
                e.VariableValue = field.getValue();
            }
            else if (control.Field is FieldLookUpBO)
            {
                var field = control.Field as FieldLookUpBO;
                e.VariableValue = field.getValue();
            }
            else if (control.Field is FieldString)
            {
                var field = control.Field as FieldString;
                e.VariableValue = field.getValue();
            }
        }

        private Dictionary<string, FormField> fields;
    }
}
