using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

[assembly: Xamarin.Forms.Dependency(typeof(DynamicForm3.Droid.DependenceService.DatabaseDroidCRUD))]
namespace DynamicForm3.Droid.DependenceService
{
    public class DatabaseDroidCRUD : Java.Lang.Object, DynamicForm3.Dependence.DatabaseCRUD
    {
        public string CreateBO(List<Dictionary<string, object>> obj, string bo_id, bool IsLink)
        {
            return DroidDatabase.Instance.CreateBOData(obj, bo_id, IsLink);
        }
        public Dictionary<string, object> ReadBO(string doc_id)
        {
            return DroidDatabase.Instance.ReadBOData(doc_id);
        }
        public bool UpdateBO(Dictionary<string, object> obj, string doc_id, bool IsLink)
        {
            return DroidDatabase.Instance.UpdateBOData(obj, doc_id, IsLink);
        }
        public bool DeleteBO(string doc_id)
        {
            return DroidDatabase.Instance.DeleteBOData(doc_id);
        }

    }
}