using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DynamicForm3.Pages
{
    public class BOList : ContentPage
    {
        public BOList()
        {
            Content = new ActivityIndicator
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                IsRunning = true,
                IsEnabled = true,
                Color = Color.Gray
            };
            Initialization();
        }

        public async void Initialization()
        {
            var value = await DependencyService.Get<Dependence.DatabaseUtils>().getForms();
            ActivityIndicator indicator = (ActivityIndicator)Content;
            indicator.IsRunning = false;
            if (value != null)
            {
                if (value.Count > 0)
                {
                    Padding = 5;
                    listView = new ListView
                    {
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                    };
                    value = value.OrderBy((val) => val.Value).ToDictionary(s => s.Key, s => s.Value);
                    listView.ItemsSource = value;
                    listView.ItemTemplate = new DataTemplate(typeof(TextCell));
                    listView.ItemTemplate.SetBinding(TextCell.TextProperty, "Value");
                    listView.ItemSelected += listView_ItemSelected;
                    Content = listView;
                }
            }
            else
            {
                await Navigation.PopAsync();
            }
        }

        void listView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

            if (e.SelectedItem != null)
            {
                KeyValuePair<string, string> value = (KeyValuePair<string, string>)e.SelectedItem;
                Navigation.PushAsync(new BODataList(value.Key));
                listView.SelectedItem = null;
            }

        }

        private ListView listView;
    }
}
