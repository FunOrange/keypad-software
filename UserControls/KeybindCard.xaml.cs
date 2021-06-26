using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
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

        public string HeaderText
        {
            get { return (string)GetValue(HeaderTextProperty); }
            set { SetValue(HeaderTextProperty, value); }
        }
        public static readonly DependencyProperty HeaderTextProperty =
            DependencyProperty.Register("HeaderText", typeof(string), typeof(KeybindCard), new PropertyMetadata(""));

        public event EventHandler BeginEditKeybind;
        public event EventHandler StopEditKeybind;

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
    }
}
