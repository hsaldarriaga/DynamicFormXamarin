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
            NavigationPage.SetHasNavigationBar(this, false);
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
            bool value = await DependencyService.Get<AllPlatformMethods.DatabaseUtils>().LoadDatabase();
            ActivityIndicator indicator = (ActivityIndicator)Content;
            indicator.IsRunning = false;
            if (value)
            {
                await Navigation.PushAsync(new LoginPage());
                Navigation.RemovePage(this);
            }
            else
            {
                await Navigation.PopAsync();
            }
        }
    }
}
