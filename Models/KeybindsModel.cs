using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KeypadSoftware.Models
{
    class KeybindsModel
    {
        private KeypadSerial keypad = null;
        public bool Initialized = false;
        byte LeftButtonKeyCode;
        byte RightButtonKeyCode;
        byte SideButtonKeyCode;

        public KeybindsModel(KeypadSerial _keypad)
        {
            keypad = _keypad;
            PullAllValues();
#if NO_KEYPAD
            Console.WriteLine("No keypad; Initializing with default values");
            LeftButtonKeyCode = KeyCodeConverter.ToScanCode(Key.Z);
            RightButtonKeyCode = KeyCodeConverter.ToScanCode(Key.X);
            SideButtonKeyCode = KeyCodeConverter.ToScanCode(Key.Escape);
#endif
        }

        // Reads current values from keypad
        public void PullAllValues()
        {
#if NO_KEYPAD
            Console.WriteLine("No keypad; assume values in keypad already match");
            return;
#endif
            // Request keybinds from keypad
        }

        // Writes values from this object into keypad
        public void PushValues()
        {

        }
    }
}
