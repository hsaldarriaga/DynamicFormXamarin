using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

using Newtonsoft.Json.Linq;

using DynamicForm3.Controls;

namespace DynamicForm3.Pages
{
    public class FormPage : ContentPage
    {
        public FormPage(string form_id, bool IsLink)
        {
            IsUpdate = false;
            Initialize(form_id);
            this.IsLinked = IsLink;
        }

        public FormPage(string form_id, Dictionary<string, object> values, bool Islink)
        {
            IsUpdate = true;
            this.IsLinked = Islink;
            ID = values["DocumentID"].ToString();
            var jarr = (values["Document_Values"] as Dictionary<string, object>)["field_values"] as JArray;
            if (jarr != null)
            {
                this.values = jarr.ToObject<List<Dictionary<string, object>>>();
            }
            else
            {
                this.values = (values["Document_Values"] as Dictionary<string, object>)["field_values"] as List<Dictionary<string, object>>;
            }
            Initialize(form_id);
        }

        private void Initialize(string form_id)
        {
            title = new Label
            {
                FontSize = 30
            };
            var Update = new Button
            {
                HorizontalOptions = LayoutOptions.Center
            };
            if (IsUpdate)
                Update.Text = "Actualizar";
            else
                Update.Text = "Crear";
            var Delete = new Button
            {
                HorizontalOptions = LayoutOptions.Center,
                Text = "Eliminar"
            };
            if (!IsUpdate)
                Delete.IsEnabled = false;
            var Cancel = new Button
            {
                HorizontalOptions = LayoutOptions.Center,
                Text = "Cancelar"
            };
            content = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Spacing = 5,
                Children = { title }
            };
            var maincontent = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children = {
                    content,
                    Update, Cancel, Delete
                }
            };
            Update.Clicked += Update_Clicked;
            Delete.Clicked += Delete_Clicked;
            Cancel.Clicked += Cancel_Clicked;
            Content = new ActivityIndicator
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                IsRunning = true,
                IsEnabled = true
            };
            AddContent(form_id, maincontent);
            
        }

        private void AddEverything(StackLayout maincontent)
        {
            Content = new ScrollView
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Padding = 5,
                Content = maincontent
            };
            Finished = true;
            UpdateControls(null, null);
        }

        private void Cancel_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void Delete_Clicked(object sender, EventArgs e)
        {
            DependencyService.Get<AllPlatformMethods.DatabaseCRUD>().DeleteBO(ID);
            if (FormPageEvents != null)
                FormPageEvents.FormDeleted(ID);
            Navigation.PopAsync();
        }

        private void Update_Clicked(object sender, EventArgs e)
        {
            if (IsUpdate)
            {
                var values = new Dictionary<string, object>();
                values.Add("bo_id", BO_ID);
                var list_values = new List<Dictionary<string, object>>();
                for (int i = 1; i < content.Children.Count; i++)
                {
                    var val = content.Children.ElementAt(i);
                    if (!(val is FieldLabel))
                    {
                        var elem = DynamicForm3.Models.Tools.SaveData(val, i - 1);
                        list_values.Add(elem);
                    }
                }
                values.Add("field_values", list_values);
                if(DependencyService.Get<AllPlatformMethods.DatabaseCRUD>().UpdateBO(values, ID, IsLinked))
                    Navigation.PopAsync();
                if (FormPageEvents != null)
                    FormPageEvents.FormUpdated(ID);
            }
            else
            {
                var values = new List<Dictionary<string, object>>();
                for (int i = 1; i < content.Children.Count; i++)
                {
                    var val = content.Children.ElementAt(i);
                    if (!(val is FieldLabel))
                    {
                        var elem = DynamicForm3.Models.Tools.SaveData(val, i - 1);
                        values.Add(elem);
                    }
                }
                ID = DependencyService.Get<AllPlatformMethods.DatabaseCRUD>().CreateBO(values, BO_ID, IsLinked);
                if (ID != null)
                {
                    Navigation.PopAsync();
                    if (FormPageEvents != null)
                        FormPageEvents.FormCreated(ID);
                }
            }
        }

        private async void AddContent(string id, StackLayout maincontent)
        {
            await Task.Run(() =>
            {
                BO_ID = id;
                allfields = new List<Models.FormField>();
                var data = DependencyService.Get<AllPlatformMethods.DatabaseUtils>().getForm(id);
                content.Children.Add(new Entry
                {
                    Text = data.ToString()
                });
                title.Text = data["description"].ToString();
                var fields = data["fields"] as JArray;
                fields.OrderBy((e) => Int32.Parse(e["prop_view_order"].ToString()));
                foreach (var item in fields)
                {
                    string caption = item["caption"].ToString();
                    string prop_id = item["prop_id"].ToString();
                    var type = (FIELD_TYPES)Int32.Parse(item["field_type"].ToString());
                    Dictionary<string, object> update_value = null;
                    if (IsUpdate)
                        foreach (var item1 in values)
                        {
                            if (item1["prop_id"].ToString() == prop_id)
                            {
                                update_value = item1; break;
                            }
                        }
                    Models.FormField formfield = null;
                    StackLayout campo;
                    switch (type)
                    {
                        case FIELD_TYPES.TIME:
                            FieldTime ttime = new FieldTime(prop_id, caption);
                            ttime.FieldValueChanged = UpdateControls;
                            if (IsUpdate && update_value["value"].ToString() != "null")
                                ttime.setValue(TimeSpan.Parse(update_value["value"].ToString()));
                            content.Children.Add(ttime);
                            campo = ttime as StackLayout;
                            formfield = new Models.FormField(ref campo, item["criteria"], item["calculo"]);
                            break;
                        case FIELD_TYPES.STRING:
                            FieldString str = new FieldString(prop_id, caption);
                            str.FieldValueChanged = UpdateControls;
                            if (IsUpdate && update_value["value"].ToString() != "null")
                                str.setValue(update_value["value"].ToString());
                            content.Children.Add(str);
                            campo = str as StackLayout;
                            formfield = new Models.FormField(ref campo, item["criteria"], item["calculo"]);
                            break;
                        case FIELD_TYPES.LOOKUP_FIXED:
                            string datasource = item["datasource"].ToString();
                            string prop_title = item["title"].ToString();
                            var data1 = DependencyService.Get<AllPlatformMethods.DatabaseUtils>().getEnumeration(datasource);
                            FieldLookUp look = new FieldLookUp(prop_id, prop_title, data1);
                            look.FieldValueChanged = UpdateControls;
                            if (IsUpdate && update_value["value"].ToString() != "null")
                                look.setValue(Int32.Parse(update_value["value"].ToString()));
                            content.Children.Add(look);
                            campo = look as StackLayout;
                            formfield = new Models.FormField(ref campo, item["criteria"], item["calculo"]);
                            break;
                        case FIELD_TYPES.LOOKUP_BO:
                            string bo_type = item["link_to"].ToString();
                            FieldLookUpBO lookupbo = new FieldLookUpBO(prop_id, caption, bo_type);
                            lookupbo.FieldValueChanged = UpdateControls;
                            if (IsUpdate && update_value["value"].ToString() != "null")
                                lookupbo.setValue(update_value["value"].ToString());
                            content.Children.Add(lookupbo);
                            campo = lookupbo as StackLayout;
                            formfield = new Models.FormField(ref campo, item["criteria"], item["calculo"]);
                            break;
                        case FIELD_TYPES.LABEL:
                            FieldLabel label = new FieldLabel(caption, prop_id);
                            content.Children.Add(label);
                            break;
                        case FIELD_TYPES.INTEGER:
                            FieldInteger view = new FieldInteger(prop_id, caption);
                            if (IsUpdate && update_value["value"].ToString() != "null")
                                view.setValue(Int32.Parse(update_value["value"].ToString()));
                            view.FieldValueChanged = UpdateControls;
                            content.Children.Add(view);
                            campo = view as StackLayout;
                            formfield = new Models.FormField(ref campo, item["criteria"], item["calculo"]);
                            break;
                        case FIELD_TYPES.FIX_VALUE:
                            break;
                        case FIELD_TYPES.DECIMAL:
                            FieldDecimal decim = new FieldDecimal(prop_id, caption);
                            decim.FieldValueChanged = UpdateControls;
                            if (IsUpdate && update_value["value"].ToString() != "null")
                            {
                                decimal? value_dec = null;
                                decimal real_val;
                                if (decimal.TryParse(update_value["value"].ToString(), out real_val))
                                {
                                    value_dec = real_val;
                                }
                                decim.setValue(value_dec);

                            }
                            content.Children.Add(decim);
                            campo = decim as StackLayout;
                            formfield = new Models.FormField(ref campo, item["criteria"], item["calculo"]);
                            break;
                        case FIELD_TYPES.DATETIME:
                            FieldDateTime datet = new FieldDateTime(caption, prop_id);
                            datet.FieldValueChanged = UpdateControls;
                            if (IsUpdate && update_value["value"].ToString() != "null")
                                datet.setValue(DateTime.Parse(update_value["value"].ToString()));
                            content.Children.Add(datet);
                            campo = datet as StackLayout;
                            formfield = new Models.FormField(ref campo, item["criteria"], item["calculo"]);
                            break;
                        case FIELD_TYPES.BOOLEAN:
                            FieldBoolean boole = new FieldBoolean(caption, prop_id);
                            boole.FieldValueChanged = UpdateControls;
                            if (IsUpdate && update_value["value"].ToString() != "null")
                            {
                                string vall = update_value["value"].ToString();
                                if (vall != "null")
                                    boole.setValue(bool.Parse(vall));
                            }
                            content.Children.Add(boole);
                            campo = boole as StackLayout;
                            formfield = new Models.FormField(ref campo, item["criteria"], item["calculo"]);
                            break;
                        case FIELD_TYPES.BO_REF:
                            break;
                        case FIELD_TYPES.BO_LINK:
                            var lb1 = new FieldBOLink(prop_id, caption, item["link_to"].ToString(), item["Association"].ToString());
                            lb1.FieldValueChanged = UpdateControls;
                            if (IsUpdate && update_value["value"].ToString() != "null")
                                lb1.setValue(update_value["value"].ToString());
                            content.Children.Add(lb1);
                            campo = lb1 as StackLayout;
                            formfield = new Models.FormField(ref campo, item["criteria"], item["calculo"]);
                            break;
                        case FIELD_TYPES.BO_COLLECTION:
                            FieldBOCollection bocoll = new FieldBOCollection(prop_id, caption, item["link_to"].ToString(), item["Association"].ToString());
                            bocoll.FieldValueChanged = UpdateControls;
                            if (IsUpdate && update_value["value"].ToString() != "null")
                                bocoll.setValue(update_value["value"] as List<string>);
                            content.Children.Add(bocoll);
                            campo = bocoll as StackLayout;
                            formfield = new Models.FormField(ref campo, item["criteria"], item["calculo"]);
                            break;
                        case FIELD_TYPES.DOUBLE:
                            FieldDouble view2 = new FieldDouble(prop_id, caption);
                            view2.FieldValueChanged = UpdateControls;
                            if (IsUpdate && update_value["value"].ToString() != "null")
                                view2.setValue(Double.Parse(update_value["value"].ToString()));
                            content.Children.Add(view2);
                            campo = view2 as StackLayout;
                            formfield = new Models.FormField(ref campo, item["criteria"], item["calculo"]);
                            break;
                        default:
                            break;
                    }
                    if (formfield != null)
                    {
                        formfield.ID = prop_id;
                        allfields.Add(formfield);
                    }
                }
                dicfields = allfields.ToDictionary(x => x.ID, x => x);
            });
            AddEverything(maincontent);
        }

        private async void UpdateControls(object sender, PropertyChangingEventArgs e)
        {
            if (Finished)
            {
                await Task.Run(() =>
                {
                    foreach (var item in allfields)
                    {
                        item.EvaluateCriteria(ref dicfields, BO_ID);
                        item.EvaluateCalculo(ref dicfields);
                    }
                });
            }
        }

        private string ID, BO_ID;
        private Label title;
        private StackLayout content;
        
        private bool Finished = false;
        private List<Dictionary<string, object>> values;
        
        private List<Models.FormField> allfields;
        private Dictionary<string, Models.FormField> dicfields;

        public bool IsUpdate { get; private set; }
        public bool IsLinked { get; private set; }

        public IFormPageEvents FormPageEvents { get; set; }
    }
}
