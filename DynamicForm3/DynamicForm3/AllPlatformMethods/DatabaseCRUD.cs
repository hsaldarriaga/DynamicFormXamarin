using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicForm3.AllPlatformMethods
{
    public interface DatabaseCRUD
    {
        string CreateBO(List<Dictionary<string, object>> obj, string bo_id, bool IsLink);
        Dictionary<string, object> ReadBO(string doc_id);
        bool UpdateBO(Dictionary<string, object> obj, string doc_id, bool IsLink);
        bool DeleteBO(string doc_id);
        bool init_usuarios();
        string iniciar_sesion(string user, string pass, out string mesagge_error);
    }
}
