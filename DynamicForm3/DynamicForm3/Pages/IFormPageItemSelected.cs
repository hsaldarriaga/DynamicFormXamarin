using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicForm3.Pages
{
    public interface IFormPageItemSelected
    {
        void SelectedItem(object key, object value);
    }
}
