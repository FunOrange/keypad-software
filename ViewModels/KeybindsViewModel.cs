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
            Side,
            LeftMacro,
            RightMacro
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

        public void onKeyboardFocus(EventArgs e)
        {
            Console.WriteLine(e);
        }

        public string LeftKeybind => KeyCodeConverter.FromKeyCode(Keybinds.LeftButtonScanCode).DisplayName;
        public string RightKeybind => KeyCodeConverter.FromKeyCode(Keybinds.RightButtonScanCode).DisplayName;
        public string SideKeybind => KeyCodeConverter.FromKeyCode(Keybinds.SideButtonScanCode).DisplayName;
        public string LeftMacro => KeyCodeConverter.FromKeyCode(Keybinds.LeftMacroScanCode).DisplayName;
        public string RightMacro => KeyCodeConverter.FromKeyCode(Keybinds.RightMacroScanCode).DisplayName;
        public void NotifyAllProperties()
        {
            NotifyOfPropertyChange(() => LeftKeybind);
            NotifyOfPropertyChange(() => RightKeybind);
            NotifyOfPropertyChange(() => SideKeybind);
            NotifyOfPropertyChange(() => LeftMacro);
            NotifyOfPropertyChange(() => RightMacro);
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
            switch (buttonBeingEdited)
            {
                case KeypadButton.None:
                    return;
                case KeypadButton.Left:       Keybinds.LeftButtonScanCode  = KeyCodeConverter.FromScanCode(e.Key).ScanCode; break;
                case KeypadButton.Right:      Keybinds.RightButtonScanCode = KeyCodeConverter.FromScanCode(e.Key).ScanCode; break;
                case KeypadButton.Side:       Keybinds.SideButtonScanCode  = KeyCodeConverter.FromScanCode(e.Key).ScanCode; break;
                case KeypadButton.LeftMacro:  Keybinds.LeftMacroScanCode   = KeyCodeConverter.FromScanCode(e.Key).ScanCode; break;
                case KeypadButton.RightMacro: Keybinds.RightMacroScanCode  = KeyCodeConverter.FromScanCode(e.Key).ScanCode; break;
            }
            bool success = false;
            while (!success)
            {
                success = Keybinds.PushAllValues();
                if (!success)
                    Console.WriteLine("Readback failed");
            }
            NotifyAllProperties();
        }

        public void ClickAnywhere()
        {
        }

        // left
        public void BeginEditLeftKeybind() {
            buttonBeingEdited = KeypadButton.Left;
            NotifyAllProperties();
        }
        public void StopEditLeftKeybind() {
            if (buttonBeingEdited == KeypadButton.Left)
                buttonBeingEdited = KeypadButton.None;
            NotifyAllProperties();
        }

        // right
        public void BeginEditRightKeybind()
        {
            buttonBeingEdited = KeypadButton.Right;
            NotifyAllProperties();
        }
        public void StopEditRightKeybind() {
            if (buttonBeingEdited == KeypadButton.Right)
                buttonBeingEdited = KeypadButton.None;
            NotifyAllProperties();
        }

        // side
        public void BeginEditSideKeybind()
        {
            buttonBeingEdited = KeypadButton.Side;
            NotifyAllProperties();
        }
        public void StopEditSideKeybind() {
            if (buttonBeingEdited == KeypadButton.Side)
                buttonBeingEdited = KeypadButton.None;
            NotifyAllProperties();
        }
        public void ToggleKeybindLayerButton()
        {
            Keybinds.SideButtonScanCode = 0xff;
            NotifyAllProperties();
        }

        // left macro
        public void BeginEditLeftMacro()
        {
            buttonBeingEdited = KeypadButton.LeftMacro;
            NotifyAllProperties();
        }
        public void StopEditLeftMacro() {
            if (buttonBeingEdited == KeypadButton.LeftMacro)
                buttonBeingEdited = KeypadButton.None;
            NotifyAllProperties();
        }

        // right macro
        public void BeginEditRightMacro()
        {
            buttonBeingEdited = KeypadButton.RightMacro;
            NotifyAllProperties();
        }
        public void StopEditRightMacro() {
            if (buttonBeingEdited == KeypadButton.RightMacro)
                buttonBeingEdited = KeypadButton.None;
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
