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
        public FieldBOLink(string id, string caption, string bo_type, string association) : base(caption, id, FIELD_TYPES.BO_LINK)
        {
            HasAssociation = association != "null";
            bt = new Button
            {
                Text = "No Asignado",
                IsEnabled = false
            };
            if (!HasAssociation)
            {
                bt.Text = "Mas detalles";
                bt.IsEnabled = true;
            }
            var lb = new CaptionLabel
            {
                Text = "*Este campo no es editable desde este Business Object",
                FontSize = 8
            };
            bt.Clicked += bt_Clicked;
            Children.Add(bt);
            if (HasAssociation)
                Children.Add(lb);
            this.bo_type = bo_type;
        }

        void bt_Clicked(object sender, EventArgs e)
        {
            if (Value != null)
            {
                if (HasAssociation)
                {
                    var val = DependencyService.Get<AllPlatformMethods.DatabaseUtils>().getDataBO(Value);
                    var pg = new Pages.FormReadOnlyPage(bo_type, val);
                    Navigation.PushAsync(pg);
                }
                else
                {
                    var val = DependencyService.Get<AllPlatformMethods.DatabaseUtils>().getDataBO(Value);
                    var pg = new Pages.FormPage(bo_type, val, false);
                    pg.FormPageEvents = this;
                    Navigation.PushAsync(pg);
                }
            }
            else
            {
                var pg = new Pages.FormPage(bo_type, false);
                pg.FormPageEvents = this;
                Navigation.PushAsync(pg);
            }
        }

        public override void setValue(string value)
        {
            Value = value;
            bt.IsEnabled = true;
            bt.Text = "Detalles";
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
        public Boolean HasAssociation { get; private set; }
        private string bo_type;
        private Button bt;
    }
}
