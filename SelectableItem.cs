using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeypadSoftware
{
    public class SelectableItem
    {
        public string Name { get; set; } = "";
        public bool IsSelected { get; set; } = false;
        public SelectableItem(string name) {
            Name = name;
        }
    }
}
