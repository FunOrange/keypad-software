using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
        public byte LeftMacroScanCode;
        public byte RightMacroScanCode;

        public KeybindsModel(KeypadSerial _keypad)
        {
            keypad = _keypad;
            Console.WriteLine("KeybindsModel constructor: initializing KeybindsModel with default values");
            LeftButtonScanCode = KeyCodeConverter.FromScanCode(Key.Z).ScanCode;
            RightButtonScanCode = KeyCodeConverter.FromScanCode(Key.X).ScanCode;
            SideButtonScanCode = KeyCodeConverter.FromScanCode(Key.Escape).ScanCode;
            LeftMacroScanCode = KeyCodeConverter.FromScanCode(Key.Z).ScanCode;
            RightMacroScanCode = KeyCodeConverter.FromScanCode(Key.X).ScanCode;
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
            LeftMacroScanCode = keybindsRawData[3];
            RightMacroScanCode = keybindsRawData[4];
        }

        // Writes values from this object into keypad
        [DllImport("msvcrt.dll", CallingConvention=CallingConvention.Cdecl)]
        static extern int memcmp(byte[] b1, byte[] b2, long count);
        public bool PushAllValues()
        {
            var scanCodes = new byte[] { LeftButtonScanCode, RightButtonScanCode, SideButtonScanCode, LeftMacroScanCode, RightMacroScanCode, SideButtonScanCode };
            Console.WriteLine($"KeybindsModel.PushAllValues: Write: {string.Join(" ", scanCodes.Select(b => $"0x{b:x}"))}");
            keypad.WriteKeybinds(scanCodes);
            // Readback
            var readback = keypad.ReadKeybinds();
            Console.WriteLine($"KeybindsModel.PushAllValues: Readback: {string.Join(" ", readback.Select(b => $"0x{b:x}"))}");
            return readback.Length == scanCodes.Length && memcmp(readback, scanCodes, readback.Length) == 0;
        }
    }
}
