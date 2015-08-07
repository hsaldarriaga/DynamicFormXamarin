using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
namespace DynamicForm3.Controls
{
    public class FieldLookUpBO : Field<string>, Pages.IFormPageItemSelected
    {
        public FieldLookUpBO(String id, String caption, String bo_id): base(caption, id, FIELD_TYPES.LOOKUP_BO)
        {
            var BOObjects = DependencyService.Get<AllPlatformMethods.DatabaseUtils>().getBOList(bo_id, false);
            AllValues = new Dictionary<string, string>();
            foreach (var item in BOObjects)
            {
                AllValues.Add(item["DocumentID"].ToString(), (item["Document_Values"] as Dictionary<string, object>)["date"].ToString() + " " + item["DocumentID"].ToString());
            }
            bt = new Button
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Text = "Nuevo"
            };
            bt.Clicked += bt_Clicked;
            Children.Add(bt);
        }

        void bt_Clicked(object sender, EventArgs e)
        {
            var pg = new Pages.PageLookUpBO(AllValues);
            pg.ItemSelected = this;
            Navigation.PushAsync(pg);
        }

        public void SelectedItem(object key, object value)
        {
            this.key = key.ToString();
            this.value = value.ToString();
            bt.Text = this.key;
            FieldChanging();
        }

        public override string getValue()
        {
            return key;
        }

        public override void setValue(string value)
        {
            key = value;
            bt.Text = "Modificar";
        }

        private Dictionary<string, string> AllValues;
        private Button bt;
        private string key = null, value = null;
    }
}
