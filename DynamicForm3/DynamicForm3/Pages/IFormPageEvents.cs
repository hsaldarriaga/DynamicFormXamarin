using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicForm3.Pages
{
    public interface IFormPageEvents
    {
        void FormDeleted(string doc_id);
        void FormUpdated(string doc_id);
        void FormCreated(string doc_id);
    }
}
