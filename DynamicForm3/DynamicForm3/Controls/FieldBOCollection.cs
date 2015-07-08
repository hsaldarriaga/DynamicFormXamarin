using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DynamicForm3.Controls
{
    public class FieldBOCollection : Field<List<string>>, Pages.IFormPageEvents
    {
        public FieldBOCollection(string id, string caption, string bo_id): base(caption, id, FIELD_TYPES.BO_COLLECTION)
        {
            _bo_id = bo_id;
            AllValues = new Dictionary<string, string>();
            
            list = new CustomListView
            {
                HeightRequest = 80
            };
            list.ItemSelected += list_ItemSelected;
            list.ItemTemplate = new DataTemplate(typeof(TextCell));
            list.ItemTemplate.SetBinding(TextCell.TextProperty, "Value");
            var bt = new Button
            {
                Text = "Agregar"
            };
            bt.Clicked += bt_Clicked;
            Children.Add(bt);
            Children.Add(list);
        }

        void bt_Clicked(object sender, EventArgs e)
        {
            var pg = new Pages.FormPage(_bo_id, true);
            pg.FormPageEvents = this;
            Navigation.PushAsync(pg);
        }

        void list_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                var value = (KeyValuePair<string, string>)e.SelectedItem;
                var item = DependencyService.Get<Dependence.DatabaseUtils>().getDataBO(value.Key);
                var pg = new Pages.FormPage(_bo_id, item, true);
                pg.FormPageEvents = this;
                Navigation.PushAsync(pg);
            }
            list.SelectedItem = null;
        }

        public override void setValue(List<string> value)
        {
            AllValues = new Dictionary<string, string>();
            foreach (var it in value)
            {
                var item = DependencyService.Get<Dependence.DatabaseUtils>().getDataBO(it);
                AllValues.Add(item["DocumentID"].ToString(), (item["Document_Values"] as Dictionary<string, object>)["date"].ToString() + " " + item["DocumentID"].ToString());
            }
            list.ItemsSource = AllValues;
            FieldChanging();
        }

        public override List<string> getValue()
        {
            if (AllValues.Count == 0)
                return null;
            else
                return new List<string>(AllValues.Keys);
        }

        public void FormDeleted(string doc_id)
        {
            list.ItemsSource = null;
            AllValues.Remove(doc_id);
            list.ItemsSource = AllValues;
            FieldChanging();
        }
        public void FormUpdated(string doc_id)
        {

        }
        public void FormCreated(string doc_id)
        {
            list.ItemsSource = null;
            var item = DependencyService.Get<Dependence.DatabaseUtils>().getDataBO(doc_id);
            AllValues.Add(item["DocumentID"].ToString(), (item["Document_Values"] as Dictionary<string, object>)["date"].ToString() + " " + item["DocumentID"].ToString());
            list.ItemsSource = AllValues;
            FieldChanging();
        }

        private Dictionary<string, string> AllValues;
        private ListView list;
        private string _bo_id;
    }
}
