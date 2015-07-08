using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
namespace DynamicForm3.Pages
{
    public class BODataList : ContentPage, IFormPageEvents
    {
        public BODataList(string bo_id)
        {
            _bo_id = bo_id;
            values = DependencyService.Get<Dependence.DatabaseUtils>().getBOList(bo_id, false);
            search = new SearchBar
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Placeholder = "Cuadro de Búsqueda"
            };
            search.TextChanged += search_TextChanged;
            var button = new Button
            {
                VerticalOptions = LayoutOptions.EndAndExpand,
                Text = "Agregar"
            };
            button.Clicked += button_Clicked;
            list = new ListView
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            list.ItemTemplate = new DataTemplate(typeof(TextCell));
            list.ItemTemplate.SetBinding(TextCell.TextProperty, "Key");
            LoadData();
            list.ItemSelected += list_ItemSelected;
            Content = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Spacing = 3,
                Padding = 3,
                Children = {
                    search, list, button
                }
            };
        }

        void LoadData()
        {
            AllValues = new Dictionary<string, Dictionary<string, object>>();
            foreach (var item in values)
            {
                var val = item["Document_Values"] as Dictionary<string, object>;
                AllValues.Add(val["date"].ToString() + " " + item["DocumentID"].ToString(), item);
            }
            list.ItemsSource = AllValues;
        }

        void list_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                var value = (KeyValuePair<string, Dictionary<string, object>>)e.SelectedItem;
                var pg = new Pages.FormPage(_bo_id, value.Value, false);
                pg.FormPageEvents = this;
                Navigation.PushAsync(pg);
                list.SelectedItem = null;
            }
        }

        void button_Clicked(object sender, EventArgs e)
        {
            var pg = new Pages.FormPage(_bo_id, false);
            pg.FormPageEvents = this;
            Navigation.PushAsync(pg);
        }

        void search_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue == "")
                list.ItemsSource = AllValues;
            else
            {
                var newValues = new Dictionary<string, Dictionary<string, object>>();
                newValues = AllValues.Where( (ee) => ee.Key.Contains(e.NewTextValue)).ToDictionary( f => f.Key, f => f.Value);
                list.ItemsSource = newValues;
            }
        }

        public void FormDeleted(string doc_id)
        {
            search.Text = "";
            values = DependencyService.Get<Dependence.DatabaseUtils>().getBOList(_bo_id, false);
            LoadData();
        }
        public void FormUpdated(string doc_id)
        {
            search.Text = "";
            values = DependencyService.Get<Dependence.DatabaseUtils>().getBOList(_bo_id, false);
            LoadData();
        }
        public void FormCreated(string doc_id)
        {
            search.Text = "";
            values = DependencyService.Get<Dependence.DatabaseUtils>().getBOList(_bo_id, false);
            LoadData();
        }

        private string _bo_id;
        private List<Dictionary<string, object>> values;
        private Dictionary<string, Dictionary<string, object>> AllValues;
        private ListView list;
        private SearchBar search;
    }
}
