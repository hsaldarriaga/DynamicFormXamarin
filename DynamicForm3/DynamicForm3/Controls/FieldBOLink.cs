using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DynamicForm3.Controls
{
    public class FieldBOLink : Field<string>, Pages.IFormPageEvents
    {
        public FieldBOLink(string id, string caption, string bo_type) : base(caption, id, FIELD_TYPES.BO_LINK)
        {
            var bt = new Button
            {
                Text = "Más Detalles"
            };
            bt.Clicked += bt_Clicked;
            Children.Add(bt);
            this.bo_type = bo_type;
        }

        void bt_Clicked(object sender, EventArgs e)
        {
            if (Value != null)
            {
                var val = DependencyService.Get<Dependence.DatabaseUtils>().getDataBO(Value);
                var pg = new Pages.FormPage(bo_type, val, true);
                pg.FormPageEvents = this;
                Navigation.PushAsync(pg);
            }
            else
            {
                var pg = new Pages.FormPage(bo_type, true);
                pg.FormPageEvents = this;
                Navigation.PushAsync(pg);
            }
        }

        public override void setValue(string value)
        {
            Value = value;
            FieldChanging();
        }

        public override string getValue()
        {
            return Value;
        }

        public void FormDeleted(string doc_id)
        {
            Value = null;
            FieldChanging();
        }
        public void FormUpdated(string doc_id)
        {
            Value = doc_id;
            FieldChanging();
        }
        public void FormCreated(string doc_id)
        {
            Value = doc_id;
            FieldChanging();
        }

        private string Value = null;
        private string bo_type;
    }
}
