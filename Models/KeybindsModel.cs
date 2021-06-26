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
        public Dictionary<Key, bool> LeftButtonModifiers = new Dictionary<Key, bool>();
        public Dictionary<Key, bool> RightButtonModifiers = new Dictionary<Key, bool>();
        public Dictionary<Key, bool> SideButtonModifiers = new Dictionary<Key, bool>();
        public Dictionary<Key, bool> LeftMacroModifiers = new Dictionary<Key, bool>();
        public Dictionary<Key, bool> RightMacroModifiers = new Dictionary<Key, bool>();

        public KeybindsModel(KeypadSerial _keypad)
        {
            keypad = _keypad;
            Dictionary<Key, bool>[] ModifierDictionaries =
            {
                LeftButtonModifiers,
                RightButtonModifiers,
                SideButtonModifiers,
                LeftMacroModifiers,
                RightMacroModifiers
            };
            foreach (var moddict in ModifierDictionaries) {
                moddict[Key.LeftCtrl] = false;
                moddict[Key.LeftShift] = false;
                moddict[Key.LeftAlt] = false;
                moddict[Key.RightCtrl] = false;
                moddict[Key.RightShift] = false;
                moddict[Key.RightAlt] = false;
            }
        }

        // Reads current values from keypad
        public void PullAllValues()
        {
            // Request keybinds from keypad
            var keybindsRawData = keypad.ReadKeybinds();
            LeftButtonScanCode = keybindsRawData[0];
            RightButtonScanCode = keybindsRawData[1];
            SideButtonScanCode = keybindsRawData[2];
            LeftMacroScanCode = keybindsRawData[3];
            RightMacroScanCode = keybindsRawData[4];
            // TODO: read modifiers
        }

        // Writes values from this object into keypad
        [DllImport("msvcrt.dll", CallingConvention=CallingConvention.Cdecl)]
        static extern int memcmp(byte[] b1, byte[] b2, long count);
        public bool PushAllValues()
        {
            var scanCodes = new byte[] {
                LeftButtonScanCode,
                RightButtonScanCode,
                SideButtonScanCode,
                LeftMacroScanCode,
                RightMacroScanCode,
                SideButtonScanCode
                // TODO: push modifiers
            };
            Console.WriteLine($"KeybindsModel.PushAllValues: Write: {string.Join(" ", scanCodes.Select(b => $"0x{b:x}"))}");
            keypad.WriteKeybinds(scanCodes);
            // Readback
            var readback = keypad.ReadKeybinds();
            var readbackSuccess = readback.Length == scanCodes.Length && memcmp(readback, scanCodes, readback.Length) == 0;
            if (readbackSuccess)
                Console.WriteLine($"KeybindsModel.PushAllValues: Readback success!");
            return readbackSuccess;
        }
    }
}
