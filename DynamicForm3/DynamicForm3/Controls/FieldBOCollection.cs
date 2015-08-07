using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DynamicForm3.Controls
{
    public class FieldBOCollection : Field<List<string>>, Pages.IFormPageItemSelected
    {
        public FieldBOCollection(string id, string caption, string bo_id, string association): base(caption, id, FIELD_TYPES.BO_COLLECTION)
        {
            _association = association;
            _bo_id = bo_id;
            AllValues = new List<string>();
            var bt = new Button
            {
                HorizontalOptions = LayoutOptions.Fill,
                Text = "Seleccionar"
            };
            bt.Clicked += bt_Clicked;
            Children.Add(bt);
            DBUtils = DependencyService.Get<AllPlatformMethods.DatabaseUtils>();
            DBCRUD = DependencyService.Get<AllPlatformMethods.DatabaseCRUD>();
        }

        void bt_Clicked(object sender, EventArgs e)
        {
            if (pg == null)
            {
                pg = new DynamicForm3.Pages.PageBOCollection(_bo_id, AllValues);
                pg.EventSelected = this;
            }
            Navigation.PushAsync(pg);
        }

        public override void setValue(List<string> value)
        {
            AllValues = value;
        }

        public override List<string> getValue()
        {
            if (AllValues.Count == 0)
                return null;
            else
                return AllValues;
        }

        public void SelectedItem(object key, object value)
        {
            var change = (bool)key;
            if (change)
            {
                TempValues = (Dictionary<string, bool>)value;
                temp = new List<string>();
            }
        }

        public void SaveFieldLink(ref List<string> temp, String doc_id_to_add, bool Include)
        {
            temp.Add(doc_id_to_add);
            //Get the name of the field that contains the link field.
            var structure = DBUtils.getForm(_bo_id);
            var str_fields = structure["fields"].ToObject<List<Dictionary<string, object>>>();
            String field_id = getFieldIdByAssociation(str_fields);
            //Get Values to Update the Field which link field will be changed.
            List<Dictionary<string, object>> jarr_values;
            var doc = DBCRUD.ReadBO(doc_id_to_add);
            var jarr = doc["field_values"] as Newtonsoft.Json.Linq.JArray;
            if (jarr == null)
                jarr_values = doc["field_values"] as List<Dictionary<string, object>>;
            else
                jarr_values = jarr.ToObject<List<Dictionary<string, object>>>();
            foreach (var item1 in jarr_values)
            {
                if (item1["prop_id"].ToString() == field_id)
                {
                    if (Include)
                        item1["value"] = DOC_ID;
                    else
                        item1["value"] = "";
                    break;
                }
            }
            var values = new Dictionary<string, object>();
            values.Add("bo_id", doc_id_to_add);
            values.Add("field_values", jarr_values);
            DBCRUD.UpdateBO(values, doc_id_to_add, false);
            foreach (var item in TempValues)
            {
                if (item.Value)
                    AllValues.Add(item.Key);
            }
        }

        private String getFieldIdByAssociation(List<Dictionary<string, object>> str_fields) {
            foreach (var item in str_fields)
            {
                if (item.ContainsKey("Association"))
                {
                    String val = item["Association"].ToString();
                    if (val == _association)
                        return item["prop_id"].ToString();
                }
            }
            return null;
        }

        public void ExecutePendientSaving()
        {
            if (TempValues != null)
            {
                foreach (var item in TempValues)
                {
                    SaveFieldLink(ref temp, item.Key, item.Value);
                }
            }
        }

        private Dictionary<string, bool> TempValues;
        private List<string> temp;
        private List<string> AllValues;
        private DynamicForm3.Pages.PageBOCollection pg;
        private string _bo_id;
        private Task task;
        AllPlatformMethods.DatabaseUtils DBUtils;
        AllPlatformMethods.DatabaseCRUD DBCRUD;

        public string _association { get; private set; }
        public string DOC_ID { get; set; }
    }
}
