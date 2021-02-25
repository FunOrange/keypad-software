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
        }
        #endregion

        private KeypadSerial keypad;
        public KeybindsViewModel(KeypadSerial _keypad)
        {
            keypad = _keypad;
            Keybinds = new KeybindsModel(_keypad);
            NotifyAllProperties();
        }

        public void EditLeftKeybind() {
            Console.WriteLine("edit left button");
        }
        public void EditRightKeybind()
        {
            Console.WriteLine("edit right button");
        }
        public void EditSideKeybind()
        {
            Console.WriteLine("edit right button");
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
