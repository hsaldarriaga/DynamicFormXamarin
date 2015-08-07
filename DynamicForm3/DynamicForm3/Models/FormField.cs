using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

using NCalc;

namespace DynamicForm3.Models
{
    public class FormField
    {
        public FormField(ref StackLayout field, object criteria, object calculo)
        {
            Field = field;
            HasCriteria = criteria != null;
            HasCalculo = calculo != null;
            if (HasCriteria)
                this.criteria = criteria.ToString();
            if (HasCalculo)
                this.calculo = calculo.ToString();
        }

        public void EvaluateCriteria(ref Dictionary<string, FormField> fields, string bo_type)
        {
            if (HasCriteria)
            {
                var tools = new Tools(ref fields);
                bo_type = bo_type.Replace("BO", "bo");
                bo_type += ".";
                string n_criteria = criteria.Substring(0, criteria.Length);
                n_criteria = n_criteria.Replace(bo_type, "[");
                n_criteria = n_criteria.Replace("()", "]");
                n_criteria = n_criteria.Replace("===", "==");
                Expression e = new Expression(n_criteria);
                e.EvaluateParameter += tools.Variables_ResolveVariableValue;
                e.EvaluateFunction += (s, a) =>
                {
                    string val = s;
                };
                try
                {
                    var vl = (bool)e.Evaluate();
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Field.IsVisible = vl;
                    });
                }
                catch (Exception)
                {

                }
            }
        }

        public void EvaluateCalculo(ref Dictionary<string, FormField> fields)
        {
            if (HasCalculo)
            {
                var tools = new Tools(ref fields);
                var n_calculo = calculo;
                var exp = new Expression(n_calculo);
                exp.EvaluateFunction += (s, o) =>
                {
                    if (s == "Now")
                    {
                        o.Result = DateTime.Now;
                    }
                    if (s == "DateDiffYear")
                    {
                        object[] objs = o.EvaluateParameters();
                        o.Result = Tools.GetDifferenceInYears((DateTime)objs[0], (DateTime)objs[1]);
                    } 
                    else if (s == "DateDiffDay")
                    {
                        object[] objs = o.EvaluateParameters();
                        var t1 = (DateTime)objs[0];
                        var t2 = (DateTime)objs[1];
                        TimeSpan resu = t1 - t2;
                        o.Result = resu.TotalDays;
                    } 
                    else if (s == "DateDiffMonth")
                    {
                        object[] objs = o.EvaluateParameters();
                        var t1 = (DateTime)objs[0];
                        var t2 = (DateTime)objs[1];
                        o.Result = Tools.MonthDiff(t1, t2);
                    } 
                    else if (s == "Iif")
                    {
                        object[] objs = o.EvaluateParameters();
                        bool p1 = (bool)objs[0];
                        if (p1)
                            o.Result = objs[1];
                        else
                            o.Result = objs[2];
                    }
                    else
                    {
                        System.Diagnostics.Debug.Assert(true, "Si esto explota es porque en el autogenerado de campos, dentro de el string que se evalua para calcular su valor, existe una funcion no reconocida y hay que implementar");
                    }
                };
                exp.EvaluateParameter += tools.Variables_ResolveVariableValue;
                try
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        object resu = exp.Evaluate();
                        if (Field is Controls.FieldBoolean)
                        {
                            var campo = Field as Controls.FieldBoolean;
                            campo.setValue(resu as bool?);
                        }
                        else if (Field is Controls.FieldDecimal)
                        {
                            var campo = Field as Controls.FieldDecimal;
                            campo.setValue((decimal?)resu);
                        }
                        else if (Field is Controls.FieldDouble)
                        {
                            var campo = Field as Controls.FieldDouble;
                            campo.setValue(resu as double?);
                        }
                        else if (Field is Controls.FieldInteger)
                        {
                            var campo = Field as Controls.FieldInteger;
                            campo.setValue((int)resu);
                        }
                    });
                }
                catch (System.ArgumentException)
                {

                }
            }
        }



        public StackLayout Field { get; private set;}
        private string criteria, calculo;
        public bool HasCriteria { get; private set; }
        public bool HasCalculo { get; private set; }
        public string ID { get; set; }
    }
}
