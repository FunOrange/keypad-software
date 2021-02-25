using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeypadSoftware.ViewModels
{
    public class DebounceViewModel : Screen, IKeypadViewModel
    {
        private KeypadSerial keypad;
        public DebounceViewModel(KeypadSerial _keypad)
        {
            keypad = _keypad;
        }

        public void PullAllValues()
        {
        }

        public void PushAllValues()
        {
        }
    }
}
