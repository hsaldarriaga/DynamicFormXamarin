using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DynamicForm3.Pages
{
    public class LoadingPage : ContentPage
    {
        public LoadingPage()
        {
            BackgroundColor = Color.White;
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
            bool value = await DependencyService.Get<Dependence.DatabaseUtils>().LoadDatabase();
            ActivityIndicator indicator = (ActivityIndicator)Content;
            indicator.IsRunning = false;
            if (value)
            {
                await Navigation.PushAsync(new BOList());
                Navigation.RemovePage(this);
            }
            else
            {
                await Navigation.PopAsync();
            }
        }
    }
}
