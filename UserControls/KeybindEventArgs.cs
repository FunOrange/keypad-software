using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeypadSoftware.UserControls
{
    public class KeybindEventArgs : EventArgs
    {
        public byte ScanCode { get; set; }
        public KeybindEventArgs(byte scanCode)
        {
            ScanCode = scanCode;
        }
    }
}
