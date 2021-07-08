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
        public List<bool> LeftRawInput = new bool[5000].ToList();
        public List<bool> RightRawInput = new bool[5000].ToList();

        KeypadSerial keypad;

        public DebounceModel(KeypadSerial _keypad)
        {
            keypad = _keypad;
            LeftButtonPressDebounceTime = 5;
            LeftButtonReleaseDebounceTime = 20;
            RightButtonPressDebounceTime = 5;
            RightButtonReleaseDebounceTime = 20;
            SideButtonDebounceTime = 50;
        }

        public void PullAllValues()
        {
#if NO_KEYPAD
            Console.WriteLine("DebounceModel.PullAllValues: no keypad; keeping values in model the same");
#endif
            byte[] debounceValues = keypad.ReadDebounce();
            LeftButtonPressDebounceTime = debounceValues[0];
            LeftButtonReleaseDebounceTime = debounceValues[1];
            RightButtonPressDebounceTime = debounceValues[2];
            RightButtonReleaseDebounceTime = debounceValues[3];
            SideButtonDebounceTime = debounceValues[4];
        }

        public void PushAllValues()
        {
#if NO_KEYPAD
            Console.WriteLine("DebounceModel.PushAllValues: no keypad; doing nothing");
#endif
            byte[] debounceValues = new byte[5];
            debounceValues[0] = (byte)LeftButtonPressDebounceTime;
            debounceValues[1] = (byte)LeftButtonReleaseDebounceTime;
            debounceValues[2] = (byte)RightButtonPressDebounceTime;
            debounceValues[3] = (byte)RightButtonReleaseDebounceTime;
            debounceValues[4] = (byte)SideButtonDebounceTime;
        }

        public (List<bool>, List<bool>) ReadRawButtonStateBuffer()
        {
            (LeftRawInput, RightRawInput) = keypad.ReadRawButtonStateBuffer();
            return (LeftRawInput, RightRawInput);
        }
    }
}
