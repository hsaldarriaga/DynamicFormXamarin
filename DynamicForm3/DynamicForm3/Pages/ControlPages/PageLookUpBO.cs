using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DynamicForm3.Pages
{
    public class PageLookUpBO : ContentPage
    {
        public PageLookUpBO(Dictionary<string, string> values)
        {
            AllValues = values;
            FilterValues = new Dictionary<string, string>(values);
            SearchBar bar = new SearchBar
            {
                HorizontalOptions = LayoutOptions.Fill
            };
            bar.TextChanged += bar_TextChanged;
            if (AllValues.Count == 0)
            {
                bar.Placeholder = "No hay elementos";
            }
            lv = new ListView
            {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            lv.ItemTapped += lv_ItemTapped;
            lv.ItemTemplate = new DataTemplate(typeof(TextCell));
            lv.ItemTemplate.SetBinding(TextCell.TextProperty, "Value");
            lv.ItemsSource = values;
            Content = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children = {
                    bar, lv
                }
            };
        }

        void bar_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateList(e.NewTextValue);
        }

        void lv_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var selected = ((KeyValuePair<string, int>)(e.Item));
            if (ItemSelected != null)
                ItemSelected.SelectedItem(selected.Key, selected.Value);
            Navigation.PopAsync();
        }

        private void UpdateList(String text)
        {
            if (text != "")
                FilterValues = AllValues.Where((v) => v.Value.StartsWith(text, StringComparison.CurrentCultureIgnoreCase)).ToDictionary(x => x.Key, x => x.Value);
            else
                FilterValues = AllValues;
            lv.ItemsSource = FilterValues;
        }

        private ListView lv;
        private Dictionary<string, string> AllValues;
        private Dictionary<string, string> FilterValues;
        public IFormPageItemSelected ItemSelected { get; set; }
    }
}
