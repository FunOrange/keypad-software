using Caliburn.Micro;
using KeypadSoftware.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace KeypadSoftware.ViewModels
{
    public class KeybindsViewModel : Screen, IKeypadViewModel
    {
        enum KeypadButton
        {
            None,
            Left,
            Right,
            Side
        }


        #region View Properties

        // MODEL ////////////////////////////////
        public KeybindsModel Keybinds { get; set; }
        /////////////////////////////////////////

        public string LeftKeybind => KeyCodeConverter.ToKeyCode(Keybinds.LeftButtonScanCode).ToString();
        public string RightKeybind => KeyCodeConverter.ToKeyCode(Keybinds.RightButtonScanCode).ToString();
        public string SideKeybind => KeyCodeConverter.ToKeyCode(Keybinds.SideButtonScanCode).ToString();
        public void NotifyAllProperties()
        {
            NotifyOfPropertyChange(() => LeftKeybind);
            NotifyOfPropertyChange(() => RightKeybind);
            NotifyOfPropertyChange(() => SideKeybind);
            NotifyOfPropertyChange(() => EditLeftKeybindCoverVisible);
            NotifyOfPropertyChange(() => EditRightKeybindCoverVisible);
            NotifyOfPropertyChange(() => EditSideKeybindCoverVisible);
        }
        private KeypadButton buttonBeingEdited = KeypadButton.None;
        public Visibility EditLeftKeybindCoverVisible => buttonBeingEdited == KeypadButton.Left ? Visibility.Visible : Visibility.Hidden;
        public Visibility EditRightKeybindCoverVisible => buttonBeingEdited == KeypadButton.Right ? Visibility.Visible : Visibility.Hidden;
        public Visibility EditSideKeybindCoverVisible => buttonBeingEdited == KeypadButton.Side ? Visibility.Visible : Visibility.Hidden;
        #endregion

        public KeybindsViewModel(KeypadSerial _keypad)
        {
            Keybinds = new KeybindsModel(_keypad);
        }
        public void KeyDownAnywhere(object sender, KeyEventArgs e) {
            switch (buttonBeingEdited)
            {
                case KeypadButton.None:
                    return;
                case KeypadButton.Left:  Keybinds.LeftButtonScanCode  = KeyCodeConverter.ToScanCode(e.Key); break;
                case KeypadButton.Right: Keybinds.RightButtonScanCode = KeyCodeConverter.ToScanCode(e.Key); break;
                case KeypadButton.Side:  Keybinds.SideButtonScanCode  = KeyCodeConverter.ToScanCode(e.Key); break;
            }
            Keybinds.PushAllValues();
            buttonBeingEdited = KeypadButton.None;
            NotifyAllProperties();
        }

        public void ClickAnywhere()
        {
            buttonBeingEdited = KeypadButton.None;
            NotifyAllProperties();
        }

        public void BeginEditLeftKeybind() {
            buttonBeingEdited = KeypadButton.Left;
            NotifyAllProperties();
        }
        public void BeginEditRightKeybind()
        {
            buttonBeingEdited = KeypadButton.Right;
            NotifyAllProperties();
        }
        public void BeginEditSideKeybind()
        {
            buttonBeingEdited = KeypadButton.Side;
            NotifyAllProperties();
        }

        public void PullAllValues()
        {
            Console.WriteLine("Pulling all keybind values from keypad...");
            Keybinds.PullAllValues();
            NotifyOfPropertyChange(() => Keybinds);
            NotifyAllProperties();
        }
    }
}
