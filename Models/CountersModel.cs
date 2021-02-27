using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeypadSoftware.Models
{
    public class CountersModel
    {
        private KeypadSerial keypad = null;
        public uint LeftButtonClickCount;
        public uint RightButtonClickCount;
        public uint SideButtonClickCount;

        public CountersModel(KeypadSerial _keypad)
        {
            keypad = _keypad;
            Console.WriteLine("CountersModel constructor: no keypad; setting random counter values");
            var r = new Random();
            LeftButtonClickCount = (uint)r.Next(0, 10000);
            RightButtonClickCount = (uint)r.Next(0, 10000);
            SideButtonClickCount = (uint)r.Next(0, 10000);
        }

        public void PullAllValues()
        {
#if NO_KEYPAD
            return;
#endif
            // Request keybinds from keypad
            var counters = keypad.ReadCounters();
            LeftButtonClickCount = counters[0];
            RightButtonClickCount = counters[1];
            SideButtonClickCount = counters[2];
        }
    }
}
