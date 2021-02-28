using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeypadSoftware.Models
{
    public class DebounceModel : IKeypadModel
    {
        public bool Initialized = false;
        public int LeftButtonPressDebounceTime { get; set; }
        public int LeftButtonReleaseDebounceTime { get; set; }
        public int RightButtonPressDebounceTime { get; set; }
        public int RightButtonReleaseDebounceTime { get; set; }
        public int SideButtonDebounceTime { get; set; }

        KeypadSerial keypad;

        public DebounceModel(KeypadSerial _keypad)
        {
            keypad = _keypad;
            LeftButtonPressDebounceTime = 5;
            LeftButtonReleaseDebounceTime = 20;
            RightButtonPressDebounceTime = 5;
            RightButtonReleaseDebounceTime= 20;
            SideButtonDebounceTime = 50;
        }

        public void PullAllValues()
        {
            throw new NotImplementedException();
        }

        public void PushAllValues()
        {
            throw new NotImplementedException();
        }
    }
}
