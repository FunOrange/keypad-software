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

        Dictionary<Key, bool> ModifiersHeld = new Dictionary<Key, bool>();
        Dictionary<Key, bool> GetKeybindModifiers(KeypadButton whichButton)
        {
            switch (whichButton)
            {
                case KeypadButton.Left:       return Keybinds.LeftButtonModifiers;
                case KeypadButton.Right:      return Keybinds.RightButtonModifiers;
                case KeypadButton.Side:       return Keybinds.SideButtonModifiers;
                case KeypadButton.LeftMacro:  return Keybinds.LeftMacroModifiers;
                case KeypadButton.RightMacro: return Keybinds.RightMacroModifiers;
                default:
                    return null;
            }
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

        public string LeftKeybind => FormatKeybindText(KeypadButton.Left, Keybinds.LeftButtonScanCode);
        public string RightKeybind => FormatKeybindText(KeypadButton.Right, Keybinds.RightButtonScanCode);
        public string SideKeybind => FormatKeybindText(KeypadButton.Side, Keybinds.SideButtonScanCode);
        public string LeftMacro => FormatKeybindText(KeypadButton.LeftMacro, Keybinds.LeftMacroScanCode);
        public string RightMacro => FormatKeybindText(KeypadButton.RightMacro, Keybinds.RightMacroScanCode);

        bool isEditingModifiers = false;
        string FormatKeybindText(KeypadButton whichButton, byte scanCode)
        {
            string ret = "";
            // ctrl + shift + alt
            if (isEditingModifiers && whichButton == buttonBeingEdited)
            {
                if (ModifiersHeld[Key.LeftCtrl])
                    ret += "Ctrl + ";
                if (ModifiersHeld[Key.LeftShift])
                    ret += "Shift + ";
                if (ModifiersHeld[Key.LeftAlt])
                    ret += "Alt + ";
                if (ModifiersHeld[Key.RightCtrl])
                    ret += "RCtrl + ";
                if (ModifiersHeld[Key.RightShift])
                    ret += "RShift + ";
                if (ModifiersHeld[Key.RightAlt])
                    ret += "RAlt + ";
                ret += "...";
            }
            else
            {
                var keybindModifiers = GetKeybindModifiers(whichButton);
                if (keybindModifiers[Key.LeftCtrl])
                    ret += "Ctrl + ";
                if (keybindModifiers[Key.LeftShift])
                    ret += "Shift + ";
                if (keybindModifiers[Key.LeftAlt])
                    ret += "Alt + ";
                if (keybindModifiers[Key.RightCtrl])
                    ret += "RCtrl + ";
                if (keybindModifiers[Key.RightShift])
                    ret += "RShift + ";
                if (keybindModifiers[Key.RightAlt])
                    ret += "RAlt + ";
                ret += KeyCodeConverter.FromKeyCode(scanCode).DisplayName;
            }
            return ret;
        }
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
            ModifiersHeld[Key.LeftCtrl] = false;
            ModifiersHeld[Key.LeftShift] = false;
            ModifiersHeld[Key.LeftAlt] = false;
            ModifiersHeld[Key.RightCtrl] = false;
            ModifiersHeld[Key.RightShift] = false;
            ModifiersHeld[Key.RightAlt] = false;
        }
        
        public void KeyDownAnywhere(object sender, KeyEventArgs e) {
            if (buttonBeingEdited == KeypadButton.None)
                return;

            Key key = e.Key == Key.System ? e.SystemKey : e.Key;

            // Check if modifier is being pressed
            if (ModifiersHeld.ContainsKey(key))
            {
                ModifiersHeld[key] = true;
                isEditingModifiers = true;
                NotifyAllProperties();
                return;
            }

            switch (buttonBeingEdited)
            {
                case KeypadButton.Left:
                    Keybinds.LeftButtonScanCode = KeyCodeConverter.FromScanCode(key).ScanCode;
                    Keybinds.LeftButtonModifiers = new Dictionary<Key, bool>(ModifiersHeld);
                    break;
                case KeypadButton.Right:
                    Keybinds.RightButtonScanCode = KeyCodeConverter.FromScanCode(key).ScanCode;
                    Keybinds.RightButtonModifiers = new Dictionary<Key, bool>(ModifiersHeld);
                    break;
                case KeypadButton.Side:
                    Keybinds.SideButtonScanCode = KeyCodeConverter.FromScanCode(key).ScanCode;
                    Keybinds.SideButtonModifiers = new Dictionary<Key, bool>(ModifiersHeld);
                    break;
                case KeypadButton.LeftMacro:
                    Keybinds.LeftMacroScanCode = KeyCodeConverter.FromScanCode(key).ScanCode;
                    Keybinds.LeftMacroModifiers = new Dictionary<Key, bool>(ModifiersHeld);
                    break;
                case KeypadButton.RightMacro:
                    Keybinds.RightMacroScanCode = KeyCodeConverter.FromScanCode(key).ScanCode;
                    Keybinds.RightMacroModifiers = new Dictionary<Key, bool>(ModifiersHeld);
                    break;
            }
            isEditingModifiers = false;

            if (!Keybinds.PushAllValues())
                Console.WriteLine("Readback failed");

            NotifyAllProperties();
        }

        public void KeyUpAnywhere(object sender, KeyEventArgs e) {
            if (buttonBeingEdited == KeypadButton.None)
                return;

            Key key = e.Key == Key.System ? e.SystemKey : e.Key;
            // Check if modifier is being pressed
            if (ModifiersHeld.ContainsKey(key))
                ModifiersHeld[key] = false;

            // Check if all modifiers were released
            if (ModifiersHeld.All(kvp => kvp.Value == false))
                isEditingModifiers = false;


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
