using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeypadSoftware.UserControls
{
    public class BoolArrayEventArgs : EventArgs
    {
        public bool[] Value { get; set; }
        public BoolArrayEventArgs(bool[] v)
        {
            Value = v;
        }
    }
}
