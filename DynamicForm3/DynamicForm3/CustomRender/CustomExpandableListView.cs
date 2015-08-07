using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DynamicForm3
{
    public class CustomExpandableListView : View
    {
        public CustomExpandableListView(EventHandler e)
        {
            OnItemClick = e;
        }
        
        public List<Models.BOHeaders> Items { 
            get 
            { 
                return _Items; 
            } 
            set 
            { 
                _Items = value; 
            }
        }

        public EventHandler OnItemClick
        {
            get
            {
                return _OnItemClick;
            }
            private set
            {
                _OnItemClick = value;
            }
        }

        private List<Models.BOHeaders> _Items;
        private EventHandler _OnItemClick;
    }
}
