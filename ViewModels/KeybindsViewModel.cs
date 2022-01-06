using Caliburn.Micro;
using KeypadSoftware.Models;
using KeypadSoftware.UserControls;
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
        public enum KeypadButton
        {
            None,
            Left,
            Right,
            Side
        }

        #region View Properties

        public string EmptyString
        {
            get { return ""; }
            set { return;  }
        }


        // MODEL ////////////////////////////////
        public KeybindsModel Keybinds { get; set; }
        /////////////////////////////////////////

        public string LeftKeybind => KeyCodeConverter.FromKeyCode(Keybinds.LeftButtonScanCode).DisplayName;
        public string RightKeybind => KeyCodeConverter.FromKeyCode(Keybinds.RightButtonScanCode).DisplayName;
        public string SideKeybind => KeyCodeConverter.FromKeyCode(Keybinds.SideButtonScanCode).DisplayName;

        public void NotifyAllProperties()
        {
            NotifyOfPropertyChange(() => LeftKeybind);
            NotifyOfPropertyChange(() => RightKeybind);
            NotifyOfPropertyChange(() => SideKeybind);
            NotifyOfPropertyChange(() => EditLeftKeybindCoverVisible);
            NotifyOfPropertyChange(() => EditRightKeybindCoverVisible);
            NotifyOfPropertyChange(() => EditSideKeybindCoverVisible);
            NotifyOfPropertyChange(() => MacroGroupBoxesVisible);
            NotifyOfPropertyChange(() => EmptyString);
        }
        private KeypadButton buttonBeingEdited = KeypadButton.None;
        public Visibility EditLeftKeybindCoverVisible => buttonBeingEdited == KeypadButton.Left ? Visibility.Visible : Visibility.Hidden;
        public Visibility EditRightKeybindCoverVisible => buttonBeingEdited == KeypadButton.Right ? Visibility.Visible : Visibility.Hidden;
        public Visibility EditSideKeybindCoverVisible => buttonBeingEdited == KeypadButton.Side ? Visibility.Visible : Visibility.Hidden;
        public Visibility MacroGroupBoxesVisible => Keybinds.SideButtonScanCode == 0xff ? Visibility.Visible : Visibility.Hidden;
        #endregion

        public KeybindsViewModel(KeypadSerial _keypad)
        {
            Keybinds = new KeybindsModel(_keypad);
        }
        
        public void KeyDownAnywhere(object sender, KeyEventArgs e) {
            if (buttonBeingEdited == KeypadButton.None)
                return;

            Key key = e.Key == Key.System ? e.SystemKey : e.Key;

            switch (buttonBeingEdited)
            {
                case KeypadButton.Left:
                    Keybinds.LeftButtonScanCode = KeyCodeConverter.FromScanCode(key).ScanCode;
                    break;
                case KeypadButton.Right:
                    Keybinds.RightButtonScanCode = KeyCodeConverter.FromScanCode(key).ScanCode;
                    break;
                case KeypadButton.Side:
                    Keybinds.SideButtonScanCode = KeyCodeConverter.FromScanCode(key).ScanCode;
                    break;
            }

            if (!Keybinds.PushAllValues())
                Console.WriteLine("Readback failed");

            NotifyAllProperties();
        }

        public void KeyUpAnywhere(object sender, KeyEventArgs e) {
            if (buttonBeingEdited == KeypadButton.None)
                return;

            Key key = e.Key == Key.System ? e.SystemKey : e.Key;
            NotifyAllProperties();
        }

        // common
        public void BeginEditKeybind(KeypadButton whichButton)
        {
            buttonBeingEdited = whichButton;
            NotifyAllProperties();
        }
        public void StopEditKeybind(KeypadButton whichButton)
        {
            if (buttonBeingEdited == whichButton)
                buttonBeingEdited = KeypadButton.None;
            NotifyAllProperties();
        }
        public void SetKeybind(KeypadButton whichButton, KeybindEventArgs e)
        {
            switch (whichButton)
            {
                case KeypadButton.Left:
                    Keybinds.LeftButtonScanCode = e.ScanCode;
                    break;
                case KeypadButton.Right:
                    Keybinds.RightButtonScanCode = e.ScanCode;
                    break;
                case KeypadButton.Side:
                    Keybinds.SideButtonScanCode = e.ScanCode;
                    break;
            }
            if (!Keybinds.PushAllValues())
                Console.WriteLine("Readback failed");
            NotifyAllProperties();
        }

        // Side button
        public void ToggleLed()
        {
            Keybinds.SideButtonScanCode = 0xff;
            if (!Keybinds.PushAllValues())
                Console.WriteLine("Readback failed");
            NotifyAllProperties();
        }
        
        public void PullAllValues()
        {
            Keybinds.PullAllValues();
            NotifyOfPropertyChange(() => Keybinds);
            NotifyAllProperties();
        }
    }
}
