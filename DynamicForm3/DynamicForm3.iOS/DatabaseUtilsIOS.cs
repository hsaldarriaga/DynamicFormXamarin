using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation;
using UIKit;
[assembly: Xamarin.Forms.Dependency(typeof(DynamicForm3.iOS.DatabaseUtilsIOS))]
namespace DynamicForm3.iOS
{
    public class DatabaseUtilsIOS : DynamicForm3.Dependence.DatabaseUtils
    {
        public DatabaseUtilsIOS() { }

        public async Task<bool> LoadDatabase()
        {
            return false;
        }

        public async Task<Dictionary<string, string>> getForms()
        {
            return null;
        }

        public Newtonsoft.Json.Linq.JToken getForm(string id)
        {
            return null;
        }

        public Dictionary<string, int> getEnumeration(string id)
        {
            return null;
        }

        public Dictionary<string, object> getDataBO(string id)
        {
            return null;
        }

        public List<Dictionary<string, object>> getBOList(string bo_id, bool IncludeLink)
        {
            return null;
        }
    }
}