using Caliburn.Micro;
using KeypadSoftware.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace KeypadSoftware.ViewModels
{
    public class LightingViewModel : Screen, IKeypadViewModel
    {
        #region View Properties
        // MODEL ////////////////////////////////
        public LightingModel Lighting { get; set; }
        /////////////////////////////////////////

        public Color BaseColourAll {
            get {
                return Lighting.BaseColour.FirstOrDefault();
            }
            set {
                Lighting.BaseColour = Enumerable.Repeat(value, KeypadSerial.NUM_LEDS).ToList();
                Lighting.ApplyBaseColour();
                NotifyAllProperties();
            }
        }
        public Brush BaseColourAllBrush => new SolidColorBrush(Lighting.BaseColour.FirstOrDefault());
        public UInt16 HueDeltaAll {
            get {
                return Lighting.HueDelta.FirstOrDefault();
            }
            set {
                Lighting.HueDelta = Enumerable.Repeat((UInt16)(value < 0 ? 0 : value > 360 ? 360U : value), KeypadSerial.NUM_LEDS).ToList();
                NotifyAllProperties();
            }
        }
        public UInt32 HueSpeedAll
        {
            get
            {
                UInt32 huePeriod = Lighting.HuePeriod.FirstOrDefault();
                return 6000 / (huePeriod == 0 ? 1 : huePeriod);
            }
            set
            {
                Lighting.HuePeriod = Enumerable.Repeat(6000 / (value == 0 ? 1 : value), KeypadSerial.NUM_LEDS).ToList();
                NotifyAllProperties();
            }
        }
        public Brush EndColourBrush
        {
            get
            {
                Color baseColour = Lighting.BaseColour.FirstOrDefault();
                HSVConverter.ColorToHSV(baseColour.R, baseColour.G, baseColour.B, out double h, out double s, out double v);
                HSVConverter.ColorFromHSV(h + Lighting.HueDelta.FirstOrDefault(), s, v, out byte r, out byte g, out byte b);
                return new SolidColorBrush(Color.FromRgb(r, g, b));
            }
        }

        public UInt32 ValueSpeedAll
        {
            get
            {
                UInt32 valuePeriod = Lighting.ValuePeriod.FirstOrDefault();
                return 6000 / (valuePeriod == 0 ? 1 : valuePeriod);
            }
            set
            {
                Lighting.ValuePeriod = Enumerable.Repeat(6000 / (value == 0 ? 1 : value), KeypadSerial.NUM_LEDS).ToList();
                NotifyAllProperties();
            }
        }
        public float ValueDimAll {
            get {
                return Lighting.ValueDim.FirstOrDefault();
            }
            set {
                Lighting.ValueDim = Enumerable.Repeat((float)(value < 0 ? 0f : value > 1 ? 1f : value), KeypadSerial.NUM_LEDS).ToList();
                NotifyAllProperties();
            }
        }
        public Color FlashLeftColour {
            get {
                return Lighting.FlashLeftColor.FirstOrDefault();
            }
            set {
                Lighting.FlashLeftColor = new List<Color>() { value, value, Color.FromRgb(0, 0, 0), Color.FromRgb(0, 0, 0) };
                NotifyAllProperties();
            }
        }
        public Color FlashRightColour {
            get {
                return Lighting.FlashRightColor[2];
            }
            set {
                Lighting.FlashRightColor = new List<Color>() { Color.FromRgb(0, 0, 0), Color.FromRgb(0, 0, 0), value, value };
                NotifyAllProperties();
            }
        }
        public float FlashDecayRateAll {
            get {
                return Lighting.FlashDecayRate.FirstOrDefault();
            }
            set {
                Lighting.FlashDecayRate = Enumerable.Repeat(value, KeypadSerial.NUM_LEDS).ToList();
                NotifyAllProperties();
            }
        }

        public void NotifyAllProperties()
        {
            NotifyOfPropertyChange(() => BaseColourAll);
            NotifyOfPropertyChange(() => BaseColourAllBrush);
            NotifyOfPropertyChange(() => HueDeltaAll);
            NotifyOfPropertyChange(() => EndColourBrush);
            NotifyOfPropertyChange(() => HueSpeedAll);
            NotifyOfPropertyChange(() => ValueSpeedAll);
            NotifyOfPropertyChange(() => ValueDimAll);
            NotifyOfPropertyChange(() => FlashLeftColour);
            NotifyOfPropertyChange(() => FlashRightColour);
            NotifyOfPropertyChange(() => FlashDecayRateAll);
        }
        #endregion View Properties

        public LightingViewModel(KeypadSerial _keypad)
        {
            Lighting = new LightingModel(_keypad);
        }

        #region Event Handlers
        public void BaseColorChanged(RoutedPropertyChangedEventArgs<Color> e)
        {
            NotifyAllProperties();
        }
        public void ApplySolid() => Lighting.ApplySolid();
        public void ApplyRainbow() => Lighting.ApplyRainbow();
        public void ApplyFade() => Lighting.ApplyFade();
        public void ApplyFlash() => Lighting.ApplyFlash();
        public void TurnOff() => Lighting.TurnOff();
        
        #endregion Event Handlers

        public void PullAllValues()
        {
            Lighting.PullAllValues();
            NotifyOfPropertyChange(() => Lighting);
            NotifyAllProperties();
        }

    }
}
