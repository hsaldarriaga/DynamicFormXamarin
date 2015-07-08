using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicForm3.Dependence
{
    public interface DatabaseUtils
    {
        Task<bool> LoadDatabase();
        Task<Dictionary<string, string>> getForms();
        Newtonsoft.Json.Linq.JToken getForm(string id);
        Dictionary<string, int> getEnumeration(string id);
        Dictionary<string, object> getDataBO(string id);
        List<Dictionary<string, object>> getBOList(string bo_id, bool IncludeLink);
    }
}
