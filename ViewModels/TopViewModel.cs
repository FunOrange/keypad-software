using Caliburn.Micro;
using KeypadSoftware.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace KeypadSoftware.Views
{
    public class TopViewModel : Conductor<object>
    {
        private KeypadSerial _keypad;
        public KeypadSerial Keypad
        {
            get { return _keypad; }
            set { _keypad = value; }
        }

        // Connecting Screen Properties
        public string ConnectionStatus
        {
            get {
                return Keypad.IsConnected ? "Keypad found!" : "Keypad disconnected";
            }
        }

        private BindableCollection<string> _portListColumn1;
        public BindableCollection<string> PortListColumn1
        {
            get { return _portListColumn1; }
            set {
                _portListColumn1 = value;
                NotifyOfPropertyChange(() => PortListColumn1);
            }
        }
        private BindableCollection<string> _portListColumn2;
        public BindableCollection<string> PortListColumn2
        {
            get { return _portListColumn2; }
            set {
                _portListColumn2 = value;
                NotifyOfPropertyChange(() => PortListColumn2);
            }
        }

        public TopViewModel()
        {
            Keypad = new KeypadSerial();
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

        public async void Window_Loaded(EventArgs e)
        {
            // Loop until keypad found
            while (!Keypad.IsConnected)
            {
                // Look for ports
                Keypad.UpdatePortList();

                // Try next port
                if (Keypad.UntestedPortsAvailable())
                {
                    string currentPort = Keypad.NextPort();
                    var pl1 =
                        Keypad.PortList
                        .Select(kvp => $"{kvp.Key}");
                    PortListColumn1 = new BindableCollection<string>(pl1);
                    var pl2 =
                        Keypad.PortList
                        .Select(kvp => $"{(kvp.Key == currentPort ? "＊" : KeypadSerial.StatusToString(kvp.Value.Item2))}");
                    PortListColumn2 = new BindableCollection<string>(pl2);

                    await Keypad.TryNextPortAsync();
                }
            }
            NotifyOfPropertyChange(() => ConnectionStatus);
            // Update ports display one more time
            var pl3 =
                Keypad.PortList
                .Select(kvp => $"{kvp.Key}");
            PortListColumn1 = new BindableCollection<string>(pl3);
            var pl4 =
                Keypad.PortList
                .Select(kvp => $"{KeypadSerial.StatusToString(kvp.Value.Item2)}");
            PortListColumn2 = new BindableCollection<string>(pl4);
        }
    }
}
