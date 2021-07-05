using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeypadSoftware.UserControls
{
    public class KeybindEventArgs : EventArgs
    {
        public bool LeftCtrl { get; set; }
        public bool LeftShift { get; set; }
        public bool LeftAlt { get; set; }
        public bool LeftWin { get; set; }
        public bool RightCtrl { get; set; }
        public bool RightShift { get; set; }
        public bool RightAlt { get; set; }
        public bool RightWin { get; set; }
        public byte ScanCode { get; set; }
        public KeybindEventArgs(byte scanCode,
            bool leftCtrl = false,
            bool leftShift = false,
            bool leftAlt = false,
            bool leftWin = false,
            bool rightCtrl = false,
            bool rightShift = false,
            bool rightAlt = false,
            bool rightWin = false)
        {
            ScanCode = scanCode;
            LeftCtrl = leftCtrl;
            LeftShift = leftShift;
            LeftAlt = leftAlt;
            LeftWin = leftWin;
            RightCtrl = rightCtrl;
            RightShift = rightShift;
            RightAlt = rightAlt;
            RightWin = rightWin;
        }
    }
}
