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

        public enum Page
        {
            Keybinds,
            Lighting,
            Counters,
            Debounce
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

        #region Keypad Connection Properties
        public string ConnectionStatusString
        {
            get {
                return Keypad.IsConnected ? "Keypad found!" : "Keypad disconnected";
            }
        }
        public bool IsConnected
        {
            get { return Keypad.IsConnected; }
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
            CurrentPage = Page.Keybinds;
        }

        private void LoadPage(Page page)
        {
            CurrentPage = page;
            switch (page)
            {
                case Page.Keybinds:
                    ActivateItem(KeybindsViewModel.GetInstance(Keypad));
                    break;
                case Page.Lighting:
                    ActivateItem(new LightingViewModel());
                    break;
                case Page.Counters:
                    ActivateItem(new CountersViewModel());
                    break;
                case Page.Debounce:
                    ActivateItem(new DebounceViewModel());
                    break;
                default:
                    break;
            }
        }

        public void KeybindsTabButton (object sender, RoutedEventArgs e) {
            LoadPage(Page.Keybinds);
        }
        public void LightingTabButton (object sender, RoutedEventArgs e) {
            LoadPage(Page.Lighting);
        }
        public void CountersTabButton (object sender, RoutedEventArgs e) {
            LoadPage(Page.Counters);
        }
        public void DebounceTabButton (object sender, RoutedEventArgs e) {
            LoadPage(Page.Debounce);
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
                    Keypad.Heartbeat();
                    NotifyOfPropertyChange(() => ConnectionStatusString);
                    NotifyOfPropertyChange(() => IsConnected);
                    PortListHighPriority = Keypad.GetPresentablePrioritylist(1);
                    PortListLowPriority = Keypad.GetPresentablePrioritylist(0);
                    Thread.Sleep(1000);
                }
            }
        }
    }
}
