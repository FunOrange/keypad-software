using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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

        public string HeaderColour
        {
            get { return (string)GetValue(HeaderColourProperty); }
            set { SetValue(HeaderColourProperty, value); }
        }
        public static readonly DependencyProperty HeaderColourProperty =
            DependencyProperty.Register("HeaderColour", typeof(string), typeof(KeybindCard), new PropertyMetadata("PrimaryMid"));


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

        public event EventHandler BeginEditKeybind;
        public event EventHandler StopEditKeybind;
        public event EventHandler KeybindSet;

        private void TextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            BeginEditKeybind?.Invoke(this, e);
        }

        private void TextBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            StopEditKeybind?.Invoke(this, e);
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            InputTextbox.Text = "";
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void VolumeMinus_Click(object sender, RoutedEventArgs e)
        {
            KeybindSet?.Invoke(this, new KeybindEventArgs(0xee));
        }
        private void VolumePlus_Click(object sender, RoutedEventArgs e)
        {
            KeybindSet?.Invoke(this, new KeybindEventArgs(0xed));
        }
        private void SkipPrevious_Click(object sender, RoutedEventArgs e)
        {
            KeybindSet?.Invoke(this, new KeybindEventArgs(0xf1));
        }
        private void SkipNext_Click(object sender, RoutedEventArgs e)
        {
            KeybindSet?.Invoke(this, new KeybindEventArgs(0xf2));
        }
        private void PlayPause_Click(object sender, RoutedEventArgs e)
        {
            KeybindSet?.Invoke(this, new KeybindEventArgs(0xe8));
        }
        private void SplitLeft_Click(object sender, RoutedEventArgs e)
        {
            KeybindSet?.Invoke(this, new KeybindEventArgs(0x50, leftWin: true));
        }
        private void SplitRight_Click(object sender, RoutedEventArgs e)
        {
            KeybindSet?.Invoke(this, new KeybindEventArgs(0x4f, leftWin: true));
        }
        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            // TODO:
            KeybindSet?.Invoke(this, new KeybindEventArgs(
                0x4f,
                leftWin: true
            ));
        }
    }
}
