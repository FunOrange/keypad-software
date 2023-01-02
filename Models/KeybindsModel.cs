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

        public KeybindsModel(KeypadSerial _keypad)
        {
            keypad = _keypad;
        }

        // Reads current values from keypad
        public void PullAllValues()
        {
            // Request keybinds from keypad
            var keybindsRawData = keypad.ReadKeybinds();
            LeftButtonScanCode = keybindsRawData[0];
            RightButtonScanCode = keybindsRawData[1];
            SideButtonScanCode = keybindsRawData[2];
        }

        // Writes values from this object into keypad
        [DllImport("msvcrt.dll", CallingConvention=CallingConvention.Cdecl)]
        static extern int memcmp(byte[] b1, byte[] b2, long count);
        public bool PushAllValues()
        {
            var scanCodes = new byte[] {
                LeftButtonScanCode,
                RightButtonScanCode,
                SideButtonScanCode
            };
            Console.WriteLine($"KeybindsModel.PushAllValues: Write: {string.Join(" ", scanCodes.Select(b => $"0x{b:x}"))}");
            keypad.WriteKeybinds(scanCodes);
            // Readback
            var readback = keypad.ReadKeybinds();
            var readbackSuccess = readback.Length == scanCodes.Length && memcmp(readback, scanCodes, readback.Length) == 0;
            if (readbackSuccess)
            {
                Console.WriteLine($"KeybindsModel.PushAllValues: Readback success!");
                keypad.SaveConfigToEeprom();
            }
            return readbackSuccess;
        }
    }
}
