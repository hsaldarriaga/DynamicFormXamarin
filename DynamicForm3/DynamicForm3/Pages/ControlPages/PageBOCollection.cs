using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace DynamicForm3.Pages
{
    public class PageBOCollection : ContentPage
    {
        public PageBOCollection(String BO_TYPE, List<string> selectedvalues)
        {
            AllElements = new ListView();
            AllElements.ItemTemplate = new DataTemplate(typeof(SwitchCell));
            AllElements.ItemTemplate.SetBinding(SwitchCell.TextProperty, "Key");
            AllElements.ItemTemplate.SetBinding(SwitchCell.OnProperty, new Binding("Value", BindingMode.TwoWay));
            var all = DependencyService.Get<AllPlatformMethods.DatabaseUtils>().getBOList(BO_TYPE, false);
            values = new List<Models.BOElementWithSwitch>();
            foreach (var item in all)
            {
                var element = new Models.BOElementWithSwitch(item["DocumentID"].ToString());

            }
            AllElements.ItemsSource = values;
            var Accept = new Button {
                Text = "Aceptar"
            };
            Accept.Clicked +=Accept_Clicked;
            var Cancel = new Button {
                Text = "Cancelar"
            };
            Cancel.Clicked +=Cancel_Clicked;
            Content = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                Children = {
                    AllElements,
                    new StackLayout {
                        Orientation = StackOrientation.Horizontal,
                        HorizontalOptions = LayoutOptions.Fill,
                        VerticalOptions = LayoutOptions.End,
                        Children = {
                            Accept, Cancel
                        }
                    }
                }
            };
        }

        void Cancel_Clicked(object sender, EventArgs e)
        {
            if (EventSelected != null)
                EventSelected.SelectedItem(false, AllElements);
            Navigation.PopAsync();
        }

        void Accept_Clicked(object sender, EventArgs e)
        {
            if (EventSelected != null)
                EventSelected.SelectedItem(true, AllElements.ItemsSource);
            Navigation.PopAsync();
        }

        private ListView AllElements;
        private List<Models.BOElementWithSwitch> values;

        public IFormPageItemSelected EventSelected { get; set;}
    }
}
