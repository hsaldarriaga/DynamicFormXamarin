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
    public class DatabaseUtilsIOS : DynamicForm3.AllPlatformMethods.DatabaseUtils
    {
        public DatabaseUtilsIOS() { }

        public async Task<bool> LoadDatabase()
        {
            IOSDatabase db = IOSDatabase.Instance;
            return await db.CreateDataBase();
        }

        public async Task<Dictionary<string, string>> getForms()
        {
            return await IOSDatabase.Instance.getAllForms();
        }

        public Newtonsoft.Json.Linq.JToken getForm(string id)
        {
            return IOSDatabase.Instance.getForm(id);
        }

        public Dictionary<string, int> getEnumeration(string id)
        {
            return IOSDatabase.Instance.getEnumeration(id);
        }

        public Dictionary<string, object> getDataBO(string id)
        {
            return IOSDatabase.Instance.getDataBO(id);
        }

        public List<Dictionary<string, object>> getBOList(string bo_id, bool IncludeLink)
        {
            return IOSDatabase.Instance.getDataBOList(bo_id, IncludeLink);
        }
    }
}