using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//using Ciloci.Flee;
using NCalc;

using DynamicForm3.Controls;

using Xamarin.Forms;

namespace DynamicForm3.Models
{
    class Tools
    {
        public Tools(ref Dictionary<string, FormField> fields)
        {
            this.fields = fields;
        }

        public void Variables_ResolveVariableValue(string name, ParameterArgs e)
        {
            if (name.EndsWith("Now()"))
            {
                e.Result = DateTime.Now;
            }
            if (name == "?")
                e.Result = false;
            var control = fields[name];
            if (control.Field is FieldDateTime)
            {
                var field = control.Field as FieldDateTime;
                e.Result = field.getValue();
            }
            else if (control.Field is FieldBoolean)
            {
                var field = control.Field as FieldBoolean;
                bool? value = field.getValue();
                if (value.HasValue)
                    e.Result = value.Value;
            }
            else if (control.Field is FieldDecimal)
            {
                var field = control.Field as FieldDecimal;
                decimal? value = field.getValue();
                if (value.HasValue)
                    e.Result = (double)value.Value;
                else
                    e.Result = 0;
            }
            else if (control.Field is FieldDouble)
            {
                var field = control.Field as FieldDouble;
                double? value = field.getValue();
                if (value.HasValue)
                    e.Result = value.Value;
                else
                    e.Result = 0;
            }
            else if (control.Field is FieldEnumeration)
            {
                var field = control.Field as FieldEnumeration;
                e.Result = field.getValue();
            }
            else if (control.Field is FieldInteger)
            {
                var field = control.Field as FieldInteger;
                int? val = field.getValue();
                if (val.HasValue)
                    e.Result = val.Value;
                else
                    e.Result = 0;
            }
            else if (control.Field is FieldLookUp)
            {
                var field = control.Field as FieldLookUp;
                e.Result = field.getValue();
            }
            else if (control.Field is FieldLookUpBO)
            {
                var field = control.Field as FieldLookUpBO;
                e.Result = field.getValue();
            }
            else if (control.Field is FieldString)
            {
                var field = control.Field as FieldString;
                string val = field.getValue();
                if (val != null)
                    e.Result = val;
                else
                    e.Result = "";
            }
            else
            {
                System.Diagnostics.Debug.Assert(true, "si esto explota entonces, agregar mas conversiones a los tipos de fields que faltan");
            }
        }

        public static int MonthDiff(DateTime date1, DateTime date2)
        {
            if (date1.Month < date2.Month)
            {
                return (date2.Year - date1.Year) * 12 + date2.Month - date1.Month;
            }
            else
            {
                return (date2.Year - date1.Year - 1) * 12 + date2.Month - date1.Month + 12;
            }
        }

        public static int GetDifferenceInYears(DateTime startDate, DateTime endDate)
        {
            //Excel documentation says "COMPLETE calendar years in between dates"
            int years = endDate.Year - startDate.Year;

            if (startDate.Month == endDate.Month &&// if the start month and the end month are the same
                endDate.Day < startDate.Day)// BUT the end day is less than the start day
            {
                years--;
            }
            else if (endDate.Month < startDate.Month)// if the end month is less than the start month
            {
                years--;
            }

            return years;
        }

        public static Dictionary<string, object> SaveData(View v, int index)
        {
            Dictionary<string, object> elem = new Dictionary<string, object>();
            if (v is FieldBOCollection)
            {
                var val = v as FieldBOCollection;
                val.ExecutePendientSaving();
                elem.Add("prop_id", val.Field_id);
                if (val.getValue() != null)
                    elem.Add("value", val.getValue().ToString());
                else
                    elem.Add("value", "null");
                elem.Add("order", index);
            }
            else if (v is FieldBOLink)
            {
                var val = v as FieldBOLink;
                elem.Add("prop_id", val.Field_id);
                if (val.getValue() != null)
                    elem.Add("value", val.getValue().ToString());
                else
                    elem.Add("value", "null");
                elem.Add("order", index);
            }
            else if (v is FieldBoolean)
            {
                var val = v as FieldBoolean;
                elem.Add("prop_id", val.Field_id);
                if (val.getValue() != null)
                    elem.Add("value", val.getValue().ToString());
                else
                    elem.Add("value", "null");
                elem.Add("order", index);
            }
            else if (v is FieldDateTime)
            {
                var val = v as FieldDateTime;
                elem.Add("prop_id", val.Field_id);
                if (val.getValue() != null)
                    elem.Add("value", val.getValue().ToString());
                else
                    elem.Add("value", "null");
                elem.Add("order", index);
            }
            else if (v is FieldDecimal)
            {
                var val = v as FieldDecimal;
                elem.Add("prop_id", val.Field_id);
                if (val.getValue() != null)
                    elem.Add("value", val.getValue().ToString());
                else
                    elem.Add("value", "null");
                elem.Add("order", index);
            }
            else if (v is FieldEnumeration)
            {
                var val = v as FieldEnumeration;
                elem.Add("prop_id", val.Field_id);
                if (val.getValue() != -1)
                    elem.Add("value", val.getValue().ToString());
                else
                    elem.Add("value", "null");
                elem.Add("order", index);
            }
            else if (v is FieldInteger)
            {
                var val = v as FieldInteger;
                elem.Add("prop_id", val.Field_id);
                if (val.getValue() != null)
                    elem.Add("value", val.getValue().ToString());
                else
                    elem.Add("value", "null");
                elem.Add("order", index);
            }
            else if (v is FieldLookUp)
            {
                var val = v as FieldLookUp;
                elem.Add("prop_id", val.Field_id);
                if (val.getValue() != -1)
                    elem.Add("value", val.getValue().ToString());
                else
                    elem.Add("value", "null");
                elem.Add("order", index);
            }
            else if (v is FieldLookUpBO)
            {
                var val = v as FieldLookUpBO;
                elem.Add("prop_id", val.Field_id);
                if (val.getValue() != null)
                    elem.Add("value", val.getValue().ToString());
                else
                    elem.Add("value", "null");
                elem.Add("order", index);
            }
            else if (v is FieldString)
            {
                var val = v as FieldString;
                elem.Add("prop_id", val.Field_id);
                if (val.getValue() != null)
                    elem.Add("value", val.getValue().ToString());
                else
                    elem.Add("value", "null");
                elem.Add("order", index);
            }
            else if (v is FieldTime)
            {
                var val = v as FieldTime;
                elem.Add("prop_id", val.Field_id);
                if (val.getValue() != null)
                    elem.Add("value", val.getValue().ToString());
                else
                    elem.Add("value", "null");
                elem.Add("order", index);
            }
            else if (v is FieldBoolean)
            {
                var val = v as FieldBoolean;
                elem.Add("prop_id", val.Field_id);
                if (val.getValue() != null)
                    elem.Add("value", val.getValue().ToString());
                else
                    elem.Add("value", "null");
                elem.Add("order", index);
            }
            else if (v is FieldBOCollection)
            {
                var val = v as FieldBOCollection;
                elem.Add("prop_id", val.Field_id);
                if (val.getValue() != null)
                    elem.Add("value", val.getValue());
                else
                    elem.Add("value", "null");
                elem.Add("order", index);
            }
            return elem;
        }

        private Dictionary<string, FormField> fields;
    }
}
