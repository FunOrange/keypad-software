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
        private KeybindsModel _keybinds;

        public KeybindsModel Keybinds
        {
            get { return _keybinds; }
            set {
                _keybinds = value;
                NotifyOfPropertyChange(() => Keybinds);
            }
        }


        private KeypadSerial keypad;
        private static KeybindsViewModel singleInstance;
        private KeybindsViewModel(KeypadSerial _keypad)
        {
            keypad = _keypad;
            Keybinds = new KeybindsModel(_keypad);
        }

        public void PullAllValues()
        {
            Keybinds.PullAllValues();
            NotifyOfPropertyChange(() => Keybinds);
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
