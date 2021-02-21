using Caliburn.Micro;
using KeypadSoftware.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KeypadSoftware.ViewModels
{
    public class KeybindsViewModel : Screen
    {
        private KeypadSerial keypad;
        private KeybindsModel model;
        private static KeybindsViewModel singleInstance;
        private KeybindsViewModel(KeypadSerial _keypad)
        {
            keypad = _keypad;
            model = new KeybindsModel(_keypad);
        }

        public void PullAllValues()
        {
        }


        public static KeybindsViewModel GetInstance(KeypadSerial _keypad)
        {
            if (singleInstance == null)
                singleInstance = new KeybindsViewModel(_keypad);
            return singleInstance;
        }
        public static KeybindsViewModel GetNewInstance(KeypadSerial _keypad)
        {
            singleInstance = new KeybindsViewModel(_keypad);
            return singleInstance;
        }
    }
}
