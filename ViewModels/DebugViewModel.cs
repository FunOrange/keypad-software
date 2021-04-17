using Caliburn.Micro;
using KeypadSoftware.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace KeypadSoftware.ViewModels
{
    class DebugViewModel : Screen
    {

        Timer writeTimer;

        #region Properties
        public int WriteInterval { get; set; }


        private Color _baseColourAll = Color.FromRgb(0, 0, 255);
        public Color BaseColourAll
        {
            get
            {
                return _baseColourAll;
            }
            set
            {
                writeTimer.Change(WriteInterval, Timeout.Infinite);
                _baseColourAll = value;
                BaseColourAllTextBox = ColorToHexString(_baseColourAll);
                DebugView.Instance?.ForceSelectedColor(value);
                NotifyOfPropertyChange(() => BaseColourAllTextBox);
            }
        }

        public string BaseColourAllTextBox { get; set; } = "#0000FF";

        #endregion



        public DebugViewModel(KeypadSerial keypad)
        {
            writeTimer = new Timer(WriteValues);
            WriteInterval = 100;
        }

        private void WriteValues(object state)
        {
            Console.WriteLine("--- Write Values ---");
        }


        #region Color picker bullshit
        private static String ColorToHexString(Color c)
        {
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }
        public void BaseColourAllTextBoxKeyUp(KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Tab)
            {
                try
                {
                    BaseColourAll = (Color)ColorConverter.ConvertFromString(BaseColourAllTextBox);
                }
                catch
                {
                }
            }
        }
        #endregion
    }
}
