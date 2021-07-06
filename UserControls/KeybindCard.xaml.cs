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

        public string HasKeybindLayerToggleButton
        {
            get { return (string)GetValue(HasKeybindLayerToggleButtonProperty); }
            set { SetValue(HasKeybindLayerToggleButtonProperty, value); }
        }
        public static readonly DependencyProperty HasKeybindLayerToggleButtonProperty =
            DependencyProperty.Register("HasKeybindLayerToggleButton", typeof(bool), typeof(KeybindCard), new PropertyMetadata(false));

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

        public event EventHandler KeybindSet;
        public event EventHandler KeybindLayerToggleClick;

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
            // Clear checkboxes
            Checkbox0.IsChecked = false;
            Checkbox1.IsChecked = false;
            Checkbox2.IsChecked = false;
            Checkbox3.IsChecked = false;
            Checkbox4.IsChecked = false;
            Checkbox5.IsChecked = false;
            Checkbox6.IsChecked = false;
            Checkbox7.IsChecked = false;
            NotifyCheckboxChanged();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        #region Click Event Handlers

        private void VolumeMinus_Click(object sender, EventArgs e)
        {
            KeybindSet?.Invoke(this, new KeybindEventArgs(0xee));
        }
        private void VolumePlus_Click(object sender, EventArgs e)
        {
            KeybindSet?.Invoke(this, new KeybindEventArgs(0xed));
        }
        private void SkipPrevious_Click(object sender, EventArgs e)
        {
            KeybindSet?.Invoke(this, new KeybindEventArgs(0xf1));
        }
        private void SkipNext_Click(object sender, EventArgs e)
        {
            KeybindSet?.Invoke(this, new KeybindEventArgs(0xf2));
        }
        private void PlayPause_Click(object sender, EventArgs e)
        {
            KeybindSet?.Invoke(this, new KeybindEventArgs(0xe8));
        }
        private void SplitLeft_Click(object sender, EventArgs e)
        {
            KeybindSet?.Invoke(this, new KeybindEventArgs(0x50, leftWin: true));
        }
        private void SplitRight_Click(object sender, EventArgs e)
        {
            KeybindSet?.Invoke(this, new KeybindEventArgs(0x4f, leftWin: true));
        }
        private void Apply_Click(object sender, EventArgs e)
        {
            string hexString = CustomScancode.Text;
            if (hexString.StartsWith("0x"))
                hexString = hexString.Remove(0, 2);
            if (hexString.Length == 0)
                return;

            byte hexValue = (byte)int.Parse(hexString, NumberStyles.HexNumber);
            
            KeybindSet?.Invoke(this, new KeybindEventArgs(
                hexValue,
                leftCtrl: (bool)Checkbox0.IsChecked,
                leftShift: (bool)Checkbox1.IsChecked,
                leftAlt: (bool)Checkbox2.IsChecked,
                leftWin: (bool)Checkbox3.IsChecked,
                rightCtrl: (bool)Checkbox4.IsChecked,
                rightShift: (bool)Checkbox5.IsChecked,
                rightAlt: (bool)Checkbox6.IsChecked,
                rightWin: (bool)Checkbox7.IsChecked
            ));

            // Clear checkboxes
            Checkbox0.IsChecked = false;
            Checkbox1.IsChecked = false;
            Checkbox2.IsChecked = false;
            Checkbox3.IsChecked = false;
            Checkbox4.IsChecked = false;
            Checkbox5.IsChecked = false;
            Checkbox6.IsChecked = false;
            Checkbox7.IsChecked = false;
            NotifyCheckboxChanged();
        }
        #endregion

        public event EventHandler CheckboxChanged;
        public void CheckboxClicked(object sender, EventArgs e)
        {
            NotifyCheckboxChanged();
            InputTextbox.Focus();
        }
        private void NotifyCheckboxChanged()
        {
            CheckboxChanged?.Invoke(this, new BoolArrayEventArgs(
                new bool[]
                {
                    (bool)Checkbox0.IsChecked,
                    (bool)Checkbox1.IsChecked,
                    (bool)Checkbox2.IsChecked,
                    (bool)Checkbox3.IsChecked,
                    (bool)Checkbox4.IsChecked,
                    (bool)Checkbox5.IsChecked,
                    (bool)Checkbox6.IsChecked,
                    (bool)Checkbox7.IsChecked
                }
            ));
        }

        private void CustomScancode_TextChanged(object sender, EventArgs e)
        {
            // Validate hex string
            Apply.IsEnabled = new HexadecimalRule().Validate(CustomScancode.Text, CultureInfo.InvariantCulture).IsValid;
        }

        private void ToggleKeybindLayerButton_Click(object sender, RoutedEventArgs e)
        {
            // Clear checkboxes
            Checkbox0.IsChecked = false;
            Checkbox1.IsChecked = false;
            Checkbox2.IsChecked = false;
            Checkbox3.IsChecked = false;
            Checkbox4.IsChecked = false;
            Checkbox5.IsChecked = false;
            Checkbox6.IsChecked = false;
            Checkbox7.IsChecked = false;
            NotifyCheckboxChanged();
            KeybindLayerToggleClick?.Invoke(this, EventArgs.Empty);
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            KeybindSet?.Invoke(this, new KeybindEventArgs(0x0));
        }
    }
}
