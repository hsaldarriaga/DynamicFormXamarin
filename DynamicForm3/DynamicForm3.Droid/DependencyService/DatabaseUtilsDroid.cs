using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

[assembly: Xamarin.Forms.Dependency(typeof(DynamicForm3.Droid.DependencyService.DatabaseUtilsDroid))]
namespace DynamicForm3.Droid.DependencyService
{
    public class DatabaseUtilsDroid : Java.Lang.Object, DynamicForm3.AllPlatformMethods.DatabaseUtils
    {
        public DatabaseUtilsDroid() { }

        public async Task<bool> LoadDatabase()
        {
            DroidDatabase db = DroidDatabase.Instance;
            return await db.CreateDataBase();
        }

        public async Task<Dictionary<string, string>> getForms()
        {
            return await DroidDatabase.Instance.getAllForms();
        }

        public Newtonsoft.Json.Linq.JToken getForm(string id)
        {
            return DroidDatabase.Instance.getForm(id);
        }

        public Dictionary<string, int> getEnumeration(string id)
        {
            return DroidDatabase.Instance.getEnumeration(id);
        }

        public Dictionary<string, object> getDataBO(string id)
        {
            return Droid.DroidDatabase.Instance.getDataBO(id);
        }

        public List<Dictionary<string, object>> getBOList(string bo_id, bool IncludeLink)
        {
            return Droid.DroidDatabase.Instance.getDataBOList(bo_id, IncludeLink);
        }
    }
}