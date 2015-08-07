using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(DynamicForm3.iOS.DatabaseIOSCRUD))]
namespace DynamicForm3.iOS
{
    public class DatabaseIOSCRUD : DynamicForm3.AllPlatformMethods.DatabaseCRUD
    {
        public string CreateBO(List<Dictionary<string, object>> obj, string bo_id, bool IsLink)
        {
            return IOSDatabase.Instance.CreateBOData(obj, bo_id, IsLink);
        }
        public Dictionary<string, object> ReadBO(string doc_id)
        {
            return IOSDatabase.Instance.ReadBOData(doc_id);
        }
        public bool UpdateBO(Dictionary<string, object> obj, string doc_id, bool IsLink)
        {
            return IOSDatabase.Instance.UpdateBOData(obj, doc_id, IsLink);
        }
        public bool DeleteBO(string doc_id)
        {
            return IOSDatabase.Instance.DeleteBOData(doc_id);
        }
        public bool init_usuarios()
        {
            return IOSDatabase.Instance.init_usuarios();
        }
        public string iniciar_sesion(string user, string pass, out string message_error)
        {
            return IOSDatabase.Instance.iniciar_sesion(user, pass, out message_error);
        }

    }
}