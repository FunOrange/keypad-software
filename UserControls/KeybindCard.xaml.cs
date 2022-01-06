using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KeypadSoftware.UserControls
{
    /// <summary>
    /// Interaction logic for KeybindCard.xaml
    /// </summary>
    public partial class KeybindCard : UserControl
    {
        public KeybindCard()
        {
            InitializeComponent();
        }


        #region Properties
        public string HeaderColour
        {
            get { return (string)GetValue(HeaderColourProperty); }
            set { SetValue(HeaderColourProperty, value); }
        }
        public static readonly DependencyProperty HeaderColourProperty =
            DependencyProperty.Register("HeaderColour", typeof(string), typeof(KeybindCard), new PropertyMetadata("PrimaryMid"));

        public string HasLedToggleButton
        {
            get { return (string)GetValue(HasLedToggleButtonProperty); }
            set { SetValue(HasLedToggleButtonProperty, value); }
        }
        public static readonly DependencyProperty HasLedToggleButtonProperty =
            DependencyProperty.Register("HasLedToggleButton", typeof(bool), typeof(KeybindCard), new PropertyMetadata(false));

        public string KeybindText
        {
            get { return (string)GetValue(KeybindTextProperty); }
            set { SetValue(KeybindTextProperty, value); }
        }
        public static readonly DependencyProperty KeybindTextProperty =
            DependencyProperty.Register("KeybindText", typeof(string), typeof(KeybindCard), new PropertyMetadata(""));

        public string CustomKeybindScanCode { get; set; }
        public string HeaderText
        {
            get { return (string)GetValue(HeaderTextProperty); }
            set { SetValue(HeaderTextProperty, value); }
        }
        public static readonly DependencyProperty HeaderTextProperty =
            DependencyProperty.Register("HeaderText", typeof(string), typeof(KeybindCard), new PropertyMetadata(""));
        #endregion


        public event EventHandler BeginEditKeybind;
        public event EventHandler KeybindSet;
        public event EventHandler LedToggleClick;

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            // check if key is not a modifier key
            var modifierKeys = new Key[] {
                Key.LeftCtrl, Key.LeftShift, Key.LeftAlt, Key.LWin,
                Key.RightCtrl, Key.RightShift, Key.RightAlt, Key.RWin
            };
            if (modifierKeys.Contains(e.Key))
                return;

            InputTextbox.Text = "";
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        #region Click Event Handlers

        private void Apply_Click(object sender, EventArgs e)
        {
            string hexString = CustomScancode.Text;
            if (hexString.StartsWith("0x"))
                hexString = hexString.Remove(0, 2);
            if (hexString.Length == 0)
                return;

            byte hexValue = (byte)int.Parse(hexString, NumberStyles.HexNumber);
            
            KeybindSet?.Invoke(this, new KeybindEventArgs(hexValue));
        }
        #endregion

        private void CustomScancode_TextChanged(object sender, EventArgs e)
        {
            // Validate hex string
            Apply.IsEnabled = new HexadecimalRule().Validate(CustomScancode.Text, CultureInfo.InvariantCulture).IsValid;
        }

        private void ToggleLedButton_Click(object sender, RoutedEventArgs e)
        {
            LedToggleClick?.Invoke(this, EventArgs.Empty);
        }
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            KeybindSet?.Invoke(this, new KeybindEventArgs(0x0));
        }

        private void InputTextbox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            BeginEditKeybind?.Invoke(this, EventArgs.Empty);
        }
    }
}
