using Caliburn.Micro;
using KeypadSoftware.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private int _baseColourNumLEDs = 4;
        public int BaseColourNumLEDs {
            get {
                return _baseColourNumLEDs;
            }
            set {
                writeTimer.Change(WriteInterval, Timeout.Infinite);
                _baseColourNumLEDs = value;
            }
        }


        private Color _baseColourAll = Color.FromRgb(0, 64, 64);
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


        // Selected LEDs
        public ObservableCollection<SelectableItem> Leds { get; set; }

        public class EepromByte
        {
            public int Address { get; set; }
            public byte Data { get; set; }
            public EepromByte(int a, byte d)
            {
                Address = a;
                Data = d;
            }
        }
        private ObservableCollection<EepromByte> _eepromContents;
        public ObservableCollection<EepromByte> EepromContents
        {
            get { return _eepromContents; }
            set {
                _eepromContents = value;
                NotifyOfPropertyChange(() => EepromContents);
            }
        }



        #endregion


        KeypadSerial keypad;

        public DebugViewModel(KeypadSerial _keypad)
        {
            writeTimer = new Timer(WriteBaseColourAll);
            WriteInterval = 1;
            keypad = _keypad;

            Leds = new ObservableCollection<SelectableItem>()
            {
                new SelectableItem("LED 0"),
                new SelectableItem("LED 1"),
                new SelectableItem("LED 2"),
                new SelectableItem("LED 3"),
                new SelectableItem("LED 4"),
                new SelectableItem("LED 5"),
                new SelectableItem("LED 6"),
                new SelectableItem("LED 7"),
                new SelectableItem("LED 8"),
                new SelectableItem("LED 9"),
                new SelectableItem("LED 10"),
                new SelectableItem("LED 11")
            };
            EepromContents = new ObservableCollection<EepromByte>();
        }

        private void WriteBaseColourAll(object state)
        {
            Console.WriteLine("--- Write Values ---");
            var baseColours = new List<Color>();
            for (int i = 0; i < KeypadSerial.NUM_LEDS; i++) {
                if (Leds[i].IsSelected)
                    baseColours.Add(BaseColourAll);
                else
                    baseColours.Add(Color.FromRgb(0, 0, 0));
            }
            keypad.WriteBaseColour(baseColours);
        }

        public void SelectedLedsChanged()
        {
            writeTimer.Change(WriteInterval, Timeout.Infinite);
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

        public void SaveToEeprom()
        {
            Console.WriteLine("Saving config to EEPROM...");
            keypad.SaveConfigToEeprom();
        }

        public void ResetEeprom()
        {
            keypad.ResetEeprom();
        }
        public void ReadEeprom()
        {
            byte[] eeprom = keypad.ReadEeprom();

            var eepromTupleList = new List<EepromByte>();
            for (int i = 0; i < eeprom.Length; i++)
            {
                eepromTupleList.Add(new EepromByte(i, eeprom[i]));
            }

            EepromContents = new ObservableCollection<EepromByte>(eepromTupleList);
        }
    }
}
