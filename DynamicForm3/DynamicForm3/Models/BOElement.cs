using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicForm3.Models
{
    public class BOElement
    {

        public BOElement(String doc_id) { Id = doc_id; }

        public void SetLabels(string MainLabel, string label1, string label2)
        {
            LabelMainContent = MainLabel; LabelContent1 = label1; LabelContent2 = label2;
        }

        public void SetValues(string MainValue, string value1, string value2)
        {
            this.MainValue = MainValue; Value1 = value1; Value2 = value2;
        }

        public string LabelMainContent { get; set; }
        public string LabelContent1 { get; set; }
        public string LabelContent2 { get; set; }

        public string MainValue { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }

        public string Id { get; private set; }
    }
}
