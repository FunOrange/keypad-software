using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KeypadSoftware.Models
{
    public class KeybindsModel
    {
        private KeypadSerial keypad = null;
        public byte LeftButtonKeyCode;
        public byte RightButtonKeyCode;
        public byte SideButtonKeyCode;

        public KeybindsModel(KeypadSerial _keypad)
        {
            keypad = _keypad;
            Console.WriteLine("Initializing KeybindsModel with default values");
            LeftButtonKeyCode = KeyCodeConverter.ToScanCode(Key.Z);
            RightButtonKeyCode = KeyCodeConverter.ToScanCode(Key.X);
            SideButtonKeyCode = KeyCodeConverter.ToScanCode(Key.Escape);
        }

        // Reads current values from keypad
        public void PullAllValues()
        {
#if NO_KEYPAD
            Console.WriteLine("No keypad; assume values in keypad already match");
            return;
#endif
            // Request keybinds from keypad
            var keybindsRawData = keypad.ReadKeybinds();
            LeftButtonKeyCode = keybindsRawData[0];
            RightButtonKeyCode = keybindsRawData[1];
            SideButtonKeyCode = keybindsRawData[2];
        }

        // Writes values from this object into keypad
        public void PushValues()
        {

        }
    }
}
