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

        #region Keypad Connection Properties
        public string ConnectionStatus
        {
            get {
                return Keypad.IsConnected ? "Keypad found!" : "Keypad disconnected";
            }
        }

        private BindableCollection<Tuple<string, string>> _portListHighPriority;
        public BindableCollection<Tuple<string, string>> PortListHighPriority
        {
            get { return _portListHighPriority; }
            set {
                _portListHighPriority = value;
                NotifyOfPropertyChange(() => PortListHighPriority);
            }
        }
        private BindableCollection<Tuple<string, string>> _portListLowPriority;
        public BindableCollection<Tuple<string, string>> PortListLowPriority
        {
            get { return _portListLowPriority; }
            set {
                _portListLowPriority = value;
                NotifyOfPropertyChange(() => PortListLowPriority);
            }
        }
        #endregion

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

        public void Window_Loaded(EventArgs e)
        {
            Task.Run(() => ConnectionLoop());
        }

        public void ConnectionLoop()
        {
            while (true)
            {
                if (!Keypad.IsConnected)
                {
                    // Look for ports
                    Keypad.UpdatePortList();
                    PortListHighPriority = Keypad.GetPresentablePrioritylist(1);
                    PortListLowPriority = Keypad.GetPresentablePrioritylist(0);

                    // Try next port
                    Keypad.TryNextPort();
                    NotifyOfPropertyChange(() => ConnectionStatus);
                    PortListHighPriority = Keypad.GetPresentablePrioritylist(1);
                    PortListLowPriority = Keypad.GetPresentablePrioritylist(0);
                }
                else
                {
                    Keypad.Heartbeat();
                    NotifyOfPropertyChange(() => ConnectionStatus);
                    PortListHighPriority = Keypad.GetPresentablePrioritylist(1);
                    PortListLowPriority = Keypad.GetPresentablePrioritylist(0);
                    Thread.Sleep(1000);
                }
            }
        }
    }
}
