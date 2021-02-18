using Caliburn.Micro;
using KeypadSoftware.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KeypadSoftware.Views
{
    public class TopViewModel : Conductor<object>
    {
        public TopViewModel()
        {
            //KeypadSerial keypad = new KeypadSerial();
            //keypad.ConnectKeypad();
        }

        public void KeybindsTabButton (object sender, RoutedEventArgs e) {
            ActivateItem(new KeybindsViewModel());
        }
        public void LightingTabButton (object sender, RoutedEventArgs e) {
            ActivateItem(new LightingViewModel());
        }
        public void CountersTabButton (object sender, RoutedEventArgs e) {
            ActivateItem(new CountersViewModel());
        }
        public void DebounceTabButton (object sender, RoutedEventArgs e) {
            ActivateItem(new DebounceViewModel());
        }
    }
}
