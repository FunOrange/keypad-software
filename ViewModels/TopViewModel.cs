using Caliburn.Micro;
using KeypadSoftware.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

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

        #region ViewModels
        KeybindsViewModel keybindsVm;
        CountersViewModel countersVm;
        DebounceViewModel debounceVm;
        LightingViewModel lightingVm;
        DebugViewModel debugVm;
        #endregion

        #region Page
        public enum Page
        {
            Keybinds,
            Lighting,
            Counters,
            Debounce,
            Debug
        }
        private Page _currentPage;
        public Page CurrentPage
        {
            get { return _currentPage; }
            set
            {
                _currentPage = value;
                NotifyOfPropertyChange(() => CurrentPage);
            }
        }
        #endregion

        #region Keypad Connection Properties
        public string ConnectionStatusString
        {
            get {
                return Keypad.IsConnected ? "Keypad found!" : "Keypad disconnected";
            }
        }
        public bool IsConnected => Keypad.IsConnected;

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

        public void ClickAnywhere()
        {
            keybindsVm?.ClickAnywhere();
        }
        public void KeyDownAnywhere(object sender, KeyEventArgs e)
        {
            keybindsVm?.KeyDownAnywhere(sender, e);
        }

        public TopViewModel()
        {
            Keypad = new KeypadSerial();
            CurrentPage = Page.Keybinds;
            keybindsVm = new KeybindsViewModel(Keypad);
            countersVm = new CountersViewModel(Keypad);
            debounceVm = new DebounceViewModel(Keypad);
            lightingVm = new LightingViewModel(Keypad);
            debugVm = new DebugViewModel(Keypad);
            TopView.DeviceChanged += CheckConnection;
        }

        private void LoadPage(Page page)
        {
            CurrentPage = page;
            switch (page)
            {
                case Page.Keybinds:
                    ActivateItem(keybindsVm);
                    keybindsVm.PullAllValues();
                    break;
                case Page.Lighting:
                    ActivateItem(lightingVm);
                    lightingVm.PullAllValues();
                    break;
                case Page.Counters:
                    ActivateItem(countersVm);
                    countersVm.PullAllValues();
                    break;
                case Page.Debounce:
                    ActivateItem(debounceVm);
                    debounceVm.PullAllValues();
                    break;
                case Page.Debug:
                    ActivateItem(debugVm);
                    break;
                default:
                    break;
            }
        }

        public void SwitchToKeybindsTab (object sender, RoutedEventArgs e) => LoadPage(Page.Keybinds);
        public void SwitchToLightingTab (object sender, RoutedEventArgs e) => LoadPage(Page.Lighting);
        public void SwitchToCountersTab (object sender, RoutedEventArgs e) => LoadPage(Page.Counters);
        public void SwitchToDebounceTab (object sender, RoutedEventArgs e) => LoadPage(Page.Debounce);
        public void SwitchToDebugTab (object sender, RoutedEventArgs e) => LoadPage(Page.Debug);
        public void Window_Loaded(EventArgs e) {
            Task.Run(() => CheckConnection(this, e));
        }
        public void CheckConnection(object sender, EventArgs e)
        {
            if (!Keypad.IsConnected)
            {
                Console.WriteLine("Looking for keypad");
                // Look for ports
                Keypad.UpdatePortList();
                PortListHighPriority = Keypad.GetPresentablePrioritylist(1);
                PortListLowPriority = Keypad.GetPresentablePrioritylist(0);

                // Try next port
                Keypad.TryNextPort();
                NotifyOfPropertyChange(() => ConnectionStatusString);
                NotifyOfPropertyChange(() => IsConnected);
                PortListHighPriority = Keypad.GetPresentablePrioritylist(1);
                PortListLowPriority = Keypad.GetPresentablePrioritylist(0);

                if (Keypad.IsConnected)
                {
                    // Reload last viewed page
                    LoadPage(CurrentPage);
                }
            }
            else
            {
                Console.WriteLine("Checking heartbeat on existing keypad");
                Keypad.Heartbeat();
                NotifyOfPropertyChange(() => ConnectionStatusString);
                NotifyOfPropertyChange(() => IsConnected);
                PortListHighPriority = Keypad.GetPresentablePrioritylist(1);
                PortListLowPriority = Keypad.GetPresentablePrioritylist(0);
            }
        }
    }
}
