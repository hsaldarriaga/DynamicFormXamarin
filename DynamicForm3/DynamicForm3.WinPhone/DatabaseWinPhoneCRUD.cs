using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency(typeof(DynamicForm3.WinPhone.DatabaseWinPhoneCRUD))]
namespace DynamicForm3.WinPhone
{
    public class DatabaseWinPhoneCRUD : DynamicForm3.DependencyService.DatabaseCRUD
    {
        public string CreateBO(List<Dictionary<string, object>> obj, string bo_id, bool IsLink)
        {
            return null;
        }
        public Dictionary<string, object> ReadBO(string doc_id)
        {
            return null;
        }
        public bool UpdateBO(Dictionary<string, object> obj, string doc_id, bool IsLink)
        {
            return false;
        }
        public bool DeleteBO(string doc_id)
        {
            return false;
        }
    }
}
