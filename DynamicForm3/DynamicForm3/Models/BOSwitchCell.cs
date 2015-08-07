using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DynamicForm3.Models
{
    public class BOSwitchCell : ViewCell
    {
        public BOSwitchCell()
        {
            Label mainlb = new Label { FontSize = 20 };
            Label lb1 = new Label { FontSize = 16 };
            Label lb2 = new Label { FontSize = 16 };
            mainlb.SetBinding(Label.TextProperty, "LabelMainContent");
            lb1.SetBinding(Label.TextProperty, "LabelContent1");
            lb2.SetBinding(Label.TextProperty, "LabelContent2");
            Label MainContent = new Label { FontSize = 20 };
            Label con1 = new Label { FontSize = 16 };
            Label con2 = new Label { FontSize = 16 };
            MainContent.SetBinding(Label.TextProperty, "MainValue");
            con1.SetBinding(Label.TextProperty, "Value1");
            con2.SetBinding(Label.TextProperty, "Value2");

            var boswitch = new Switch
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            boswitch.SetBinding(Switch.IsToggledProperty, new Binding("Toggled", BindingMode.TwoWay));
            View = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    new StackLayout {
                        Children = {
                            new StackLayout {
                                Orientation = StackOrientation.Horizontal,
                                Children = { mainlb, MainContent } 
                            },
                            new StackLayout {
                                Orientation = StackOrientation.Horizontal,
                                Children = { lb1, con1 }
                            },
                            new StackLayout {
                                Orientation = StackOrientation.Horizontal,
                                Children = { lb2, con2 }
                            }
                        }
                    },
                    boswitch
                }
            };
        }
    }
}
