using Caliburn.Micro;
using KeypadSoftware.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace KeypadSoftware.ViewModels
{
    public class KeybindsViewModel : Screen, IKeypadViewModel
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

        #region View Properties
        public string LeftKeybind => KeyCodeConverter.ToKeyCode(Keybinds.LeftButtonKeyCode).ToString();
        public string RightKeybind => KeyCodeConverter.ToKeyCode(Keybinds.RightButtonKeyCode).ToString();
        public string SideKeybind => KeyCodeConverter.ToKeyCode(Keybinds.SideButtonKeyCode).ToString();
        public void NotifyAllProperties()
        {
            NotifyOfPropertyChange(() => LeftKeybind);
            NotifyOfPropertyChange(() => RightKeybind);
            NotifyOfPropertyChange(() => SideKeybind);
            NotifyOfPropertyChange(() => EditLeftKeybindCoverVisible);
            NotifyOfPropertyChange(() => EditRightKeybindCoverVisible);
            NotifyOfPropertyChange(() => EditSideKeybindCoverVisible);
        }
        private bool editingLeftKeybind = false;
        private bool editingRightKeybind = false;
        private bool editingSideKeybind = false;
        public Visibility EditLeftKeybindCoverVisible => editingLeftKeybind ? Visibility.Visible : Visibility.Hidden;
        public Visibility EditRightKeybindCoverVisible => editingRightKeybind ? Visibility.Visible : Visibility.Hidden;
        public Visibility EditSideKeybindCoverVisible => editingSideKeybind ? Visibility.Visible : Visibility.Hidden;
        #endregion

        private KeypadSerial keypad;
        public KeybindsViewModel(KeypadSerial _keypad)
        {
            keypad = _keypad;
            Keybinds = new KeybindsModel(_keypad);
            NotifyAllProperties();
        }
        public void GridClick()
        {
            Console.WriteLine("grid click");
        }

        public void ClickAnywhere()
        {
            Console.WriteLine("click anywhere (keybinds vm)");
            editingLeftKeybind = false;
            editingRightKeybind = false;
            editingSideKeybind = false;
            NotifyAllProperties();
        }

        public void EditLeftKeybind() {
            editingLeftKeybind = true;
            editingRightKeybind = false;
            editingSideKeybind = false;
            NotifyAllProperties();
        }
        public void EditRightKeybind()
        {
            editingLeftKeybind = false;
            editingRightKeybind = true;
            editingSideKeybind = false;
            NotifyAllProperties();
        }
        public void EditSideKeybind()
        {
            editingLeftKeybind = false;
            editingRightKeybind = false;
            editingSideKeybind = true;
            NotifyAllProperties();
        }

        public void PullAllValues()
        {
            Console.WriteLine("Pulling all keybind values from keypad...");
            Keybinds.PullAllValues();
            NotifyOfPropertyChange(() => Keybinds);
            NotifyAllProperties();
        }

        public void PushAllValues()
        {
        }
    }
}
