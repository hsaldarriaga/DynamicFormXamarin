using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

using Ciloci.Flee;

namespace DynamicForm3.Models
{
    public class FormField
    {
        public FormField(ref StackLayout field, string criteria, string calculo)
        {
            Field = field;
            HasCriteria = criteria != null;
            HasCalculo = calculo != null;
            if (HasCriteria)
                this.criteria = criteria;
            if (HasCalculo)
                this.calculo = calculo;
        }

        public void EvaluateCriteria(ref Dictionary<string, FormField> fields, string bo_type)
        {
            if (HasCriteria)
            {
                var tools = new Tools(ref fields);
                var context = new ExpressionContext();
                context.Variables.ResolveVariableType += tools.Variables_ResolveVariableType;
                context.Variables.ResolveVariableValue += tools.Variables_ResolveVariableValue;
                bo_type.Replace("BO", "bo");
                bo_type += ".";
                string n_criteria = criteria.Substring(0, criteria.Length);
                n_criteria = n_criteria.Replace(bo_type, "");
                n_criteria = n_criteria.Replace("()", "");
                n_criteria = n_criteria.Replace("===", "==");
                try
                {
                    var exp = context.CompileGeneric<bool>(n_criteria);
                    Field.IsVisible = exp.Evaluate();
                }
                catch (ExpressionCompileException) { }
            }
        }

        public void EvaluateCalculo(ref Dictionary<string, FormField> fields)
        {
            if (HasCalculo)
            {
                var tools = new Tools(ref fields);
                var context = new ExpressionContext();
                context.Variables.ResolveVariableType += tools.Variables_ResolveVariableType;
                context.Variables.ResolveVariableValue += tools.Variables_ResolveVariableValue;
                string n_calculo = criteria.Substring(0, criteria.Length);
                n_calculo = n_calculo.Replace("[", "");
                n_calculo = n_calculo.Replace("]", "");
                var exp = context.CompileDynamic(n_calculo);
                object resu = exp.Evaluate();
                if (Field is Controls.FieldBoolean)
                {
                    var campo = Field as Controls.FieldBoolean;
                    campo.setValue(resu as bool?);
                }
                else if (Field is Controls.FieldDecimal)
                {
                    var campo = Field as Controls.FieldDecimal;
                    campo.setValue(resu as decimal?);
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
            }
        }

        public StackLayout Field { get; private set;}
        private string criteria, calculo;
        public bool HasCriteria { get; private set; }
        public bool HasCalculo { get; private set; }
        public string ID { get; set; }
    }
}
