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
        public int LeftButtonPressDebounceTime
        {
            get => leftButtonPressDebounceTime;
            set {
                leftButtonPressDebounceTime = value;
            }
        }
        public int LeftButtonReleaseDebounceTime
        {
            get => leftButtonReleaseDebounceTime;
            set {
                leftButtonReleaseDebounceTime = value;
            }
        }
        public int RightButtonPressDebounceTime
        {
            get => rightButtonPressDebounceTime;
            set {
                rightButtonPressDebounceTime = value;
            }
        }
        public int RightButtonReleaseDebounceTime
        {
            get => rightButtonReleaseDebounceTime;
            set {
                rightButtonReleaseDebounceTime = value;
            }
        }
        public int SideButtonDebounceTime { get; set; }
        public List<bool> LeftRawInput = new bool[5000].ToList();
        public List<bool> RightRawInput = new bool[5000].ToList();

        KeypadSerial keypad;
        public bool IsConnected { get => keypad.IsConnected; }
        private int leftButtonPressDebounceTime;
        private int leftButtonReleaseDebounceTime;
        private int rightButtonPressDebounceTime;
        private int rightButtonReleaseDebounceTime;

        public DebounceModel(KeypadSerial _keypad)
        {
            keypad = _keypad;
        }

        public void PullAllValues()
        {
            byte[] debounceValues = keypad.ReadDebounce();
            LeftButtonPressDebounceTime = debounceValues[0];
            LeftButtonReleaseDebounceTime = debounceValues[1];
            RightButtonPressDebounceTime = debounceValues[2];
            RightButtonReleaseDebounceTime = debounceValues[3];
            SideButtonDebounceTime = debounceValues[4];
        }

        public void PushAllValues()
        {
            if (!keypad.IsConnected)
                return;
            byte[] debounceValues = new byte[5];
            debounceValues[0] = (byte)LeftButtonPressDebounceTime;
            debounceValues[1] = (byte)LeftButtonReleaseDebounceTime;
            debounceValues[2] = (byte)RightButtonPressDebounceTime;
            debounceValues[3] = (byte)RightButtonReleaseDebounceTime;
            debounceValues[4] = (byte)SideButtonDebounceTime;
            Console.WriteLine($"Writing debounce values:");
            for (int i = 0; i < debounceValues.Length; i++)
                Console.WriteLine($"{i}: 0x{debounceValues[i]:x}");
            keypad.WriteDebounce(debounceValues);
            keypad.SaveConfigToEeprom();
        }

        public (List<bool>, List<bool>) ReadRawButtonStateBuffer()
        {
            (LeftRawInput, RightRawInput) = keypad.ReadRawButtonStateBuffer();
            return (LeftRawInput, RightRawInput);
        }
    }
}
