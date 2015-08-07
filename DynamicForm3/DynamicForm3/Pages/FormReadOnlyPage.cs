using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

using DynamicForm3.Controls;

namespace DynamicForm3.Pages
{
    public class FormReadOnlyPage : ContentPage
    {
        public FormReadOnlyPage(string form_id, Dictionary<string, object> values)
        {
            var jarr = (values["Document_Values"] as Dictionary<string, object>)["field_values"] as Newtonsoft.Json.Linq.JArray;
            List<Dictionary<string, object>> formvalues;
            if (jarr != null)
            {
                formvalues = jarr.ToObject<List<Dictionary<string, object>>>();
            }
            else
            {
                formvalues = (values["Document_Values"] as Dictionary<string, object>)["field_values"] as List<Dictionary<string, object>>;
            }
            Initialize(formvalues, form_id);
        }

        private void Initialize(List<Dictionary<string, object>> formvalues, string id)
        {
            var Stack = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Fill,
                Spacing = 10
            };
            Content = new ScrollView
            {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                Content = Stack
            };
            var data = DependencyService.Get<AllPlatformMethods.DatabaseUtils>().getForm(id);
            String title = data["description"].ToString();
            Stack.Children.Add(new CaptionLabel
            {
                Text = title,
                FontSize = 26
            });
            var fields = data["fields"] as Newtonsoft.Json.Linq.JArray;
            fields.OrderBy((e) => Int32.Parse(e["prop_view_order"].ToString()));
            foreach (var item in fields)
            {
                string caption = item["caption"].ToString();
                string prop_id = item["prop_id"].ToString();
                var type = (FIELD_TYPES)Int32.Parse(item["field_type"].ToString());
                Dictionary<string, object> update_value = null;
                foreach (var item1 in formvalues)
                {
                    if (item1["prop_id"].ToString() == prop_id)
                    {
                        update_value = item1; break;
                    }
                }
                var StackField = new StackLayout
                {
                    HorizontalOptions = LayoutOptions.Fill,
                    Spacing = 5
                };
                StackField.Children.Add(new CaptionLabel
                {
                    Text = caption
                });
                StackField.Children.Add(new CaptionLabel
                {
                    Text = update_value["value"].ToString(),
                    FontSize = 9
                });

                Stack.Children.Add(StackField);
            }
        }
    }
}
