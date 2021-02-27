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
        public byte LeftButtonScanCode;
        public byte RightButtonScanCode;
        public byte SideButtonScanCode;

        public KeybindsModel(KeypadSerial _keypad)
        {
            keypad = _keypad;
            Console.WriteLine("KeybindsModel constructor: initializing KeybindsModel with default values");
            LeftButtonScanCode = KeyCodeConverter.ToScanCode(Key.Z);
            RightButtonScanCode = KeyCodeConverter.ToScanCode(Key.X);
            SideButtonScanCode = KeyCodeConverter.ToScanCode(Key.Escape);
        }

        // Reads current values from keypad
        public void PullAllValues()
        {
#if NO_KEYPAD
            Console.WriteLine("KeybindsModel.PullAllValues: no keypad; assume values in keypad already match");
            return;
#endif
            // Request keybinds from keypad
            var keybindsRawData = keypad.ReadKeybinds();
            LeftButtonScanCode = keybindsRawData[0];
            RightButtonScanCode = keybindsRawData[1];
            SideButtonScanCode = keybindsRawData[2];
        }

        // Writes values from this object into keypad
        public bool PushAllValues()
        {
#if NO_KEYPAD
            Console.WriteLine("KeybindsModel.PushAllValues: no keypad; do nothing");
            return true;
#endif
            // Write keybinds to keypad
            var scanCodes = new byte[] { LeftButtonScanCode, RightButtonScanCode, SideButtonScanCode };
            keypad.WriteKeybinds(scanCodes);
            // Readback
            var readback = keypad.ReadKeybinds();
            return readback == scanCodes;
        }
    }
}
