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
        string role;
        public BOList(string rol, string user)
        {
            role = rol;
            username = user;
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
            var value = await DependencyService.Get<AllPlatformMethods.DatabaseUtils>().getForms();
            _values = value;
            ActivityIndicator indicator = (ActivityIndicator)Content;
            indicator.IsRunning = false;
            if (value != null)
            {
                if (value.Count > 0)
                {
                    Padding = 5;
                    var resu = manual_creation(value);

                    var tx = new Label
                    {
                        Text = "Bienvenido usuario "+username,
                        FontSize = 25
                    };
                    listView = new CustomExpandableListView(ChildSelected)
                    {
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        Items = resu
                    };
                    Content = new StackLayout
                    {
                        HorizontalOptions = LayoutOptions.Fill,
                        VerticalOptions = LayoutOptions.Fill,
                        Spacing = 10,
                        Children = { tx, listView }
                    };
                }
            }
            else
            {
                await Navigation.PopAsync();
            }
        }

        private List<Models.BOHeaders> manual_creation(Dictionary<string, string> values)
        {
            List<Models.BOHeaders> resu = new List<Models.BOHeaders>();
            if (role == "medico")
            {

                var t1 = new Models.BOHeaders("Crear citas");
                var t2 = new Models.BOHeaders("Citas pendientes por agendar");
                var t3 = new Models.BOHeaders("Agenda");
                var t4 = new Models.BOHeaders("Pacientes");
                var t6 = new Models.BOHeaders("Facturacion Asistentes");
                var t7 = new Models.BOHeaders("Reportes");
                t1.Add(values["BO00024"]);
                t2.Add(values["BO00075"]);
                t3.Add(values["BO00085"]);
                t4.Add(values["BO00052"]);
                t6.Add(values["BO00064"]);
                t7.AddRange(new string[] { values["BO00032"], values["BO00030"], values["BO00066"] });
                //t8.AddRange(values.Select( e => e.Value).OrderBy( e => e).ToList<string>());
                resu.AddRange(new Models.BOHeaders[] { t1, t2, t3, t4, t6, t7});
            }
            else if (role == "Asistente_Administrativo" || role == "recepcionista")
            { 
                 var t1 = new Models.BOHeaders("Crear citas");
                var t2 = new Models.BOHeaders("Citas pendientes por agendar");
                var t3 = new Models.BOHeaders("Agenda");
                var t4 = new Models.BOHeaders("Pacientes");
                var t5 = new Models.BOHeaders("Encuestas");                
                t1.Add(values["BO00024"]);
                t2.Add(values["BO00075"]);
                t3.Add(values["BO00085"]);
                t4.Add(values["BO00052"]);
                t5.AddRange(new string[] { values["BO00004"], values["BO00028"], values["BO00036"], values["BO00047"], values["BO00053"] });
                resu.AddRange(new Models.BOHeaders[] { t1, t2, t3, t4, t5 });
            }
            else if (role == "Equipo_Calidad")
            {
                var t3 = new Models.BOHeaders("Agenda");
                var t4 = new Models.BOHeaders("Pacientes");
                var t6 = new Models.BOHeaders("Facturacion Asistentes");
                var t7 = new Models.BOHeaders("Reportes");
                t3.Add(values["BO00085"]);
                t4.Add(values["BO00052"]);
                t6.Add(values["BO00064"]);
                t7.AddRange(new string[] { values["BO00032"], values["BO00030"], values["BO00066"] });
                resu.AddRange(new Models.BOHeaders[] { t3, t4, t6, t7 });
            }
            else if(role == "fisico") {
                var t4 = new Models.BOHeaders("Pacientes");
                t4.Add(values["BO00052"]);
                resu.AddRange(new Models.BOHeaders[] {t4});
            }
            if (role == "nomatters")
            {
                var t8 = new Models.BOHeaders("Todos");
                t8.AddRange(values.Select(k => k.Value).ToList<string>());
                resu.Add(t8);
            }          
            return resu;
        }

        void ChildSelected(object sender, EventArgs e)
        {
            int [] vals = (int[])sender;
            var str = listView.Items[vals[0]][vals[1]];
            KeyValuePair<string, string> selected = _values.First((ee) => ee.Value == str);
            Navigation.PushAsync(new BODataList(selected.Key));
        }

        private string username;
        private CustomExpandableListView listView;
        private Dictionary<string, string> _values;
    }
}
