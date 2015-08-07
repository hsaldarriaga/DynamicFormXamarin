using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicForm3.Models
{
    public class BOHeaders : List<string>
    {
        public BOHeaders(string name) : base()
        {
            Name = name;
        }

        public string Name { get { return _name; } private set { _name = value; } }

        private string _name;
    }
}
