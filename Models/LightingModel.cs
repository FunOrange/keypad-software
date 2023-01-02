using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeypadSoftware.Models
{
    public class LightingModel
    {
        private KeypadSerial keypad = null;

        // Base color
        public List<Color> BaseColour;

        // Hue
        public UInt16 HueSweepEnabledMask = 0x0;
        public List<UInt16> HueDelta;
        public List<UInt32> HuePeriod;

        // Value dim
        public UInt16 ValueSweepEnabledMask = 0x0;
        public List<float> ValueDim;
        public List<UInt32> ValuePeriod;

        // Keypress flash
        public UInt16 KeypressFlashEnabledMask = 0x0;
        public List<Color> FlashLeftColor;
        public List<Color> FlashRightColor;
        public byte[] FlashBlendingMethod = { 1, 1, 1, 1 };
        public byte[] FlashHoldMethod = { 0, 0, 0, 0 };
        public List<float> FlashDecayRate;

        public bool LedsDisabled = false;

        public LightingModel(KeypadSerial _keypad)
        {
            keypad = _keypad;
        }

        // Reads current values from keypad
        public void PullAllValues()
        {
            List<UInt16> componentEnableMasks = keypad.ReadComponentEnableMask();
            HueSweepEnabledMask = componentEnableMasks[0];
            ValueSweepEnabledMask = componentEnableMasks[1];
            KeypressFlashEnabledMask = componentEnableMasks[2];

            BaseColour = keypad.ReadBaseColour();
            HueDelta = keypad.ReadHueDelta();
            HuePeriod = keypad.ReadHuePeriod();

            ValueDim = keypad.ReadValueDim();
            ValuePeriod = keypad.ReadValuePeriod();

            FlashLeftColor = keypad.ReadFlashLeftColour();
            FlashRightColor = keypad.ReadFlashRightColour();
            //FlashBlendingMethod = keypad.ReadFlashBlendingMethod();
            //FlashHoldMethod = keypad.ReadFlashHoldMethod();
            FlashDecayRate = keypad.ReadFlashDecayRate();

            LedsDisabled = keypad.ReadLedsDisabled();
        }

        public void ApplySolid()
        {
            keypad.WriteLedsDisabled(false);

            byte hueSweepEnabledMask = 0x00;
            byte valueSweepEnabledMask = 0x00;
            byte keypressFlashEnabledMask = 0x00;
            keypad.WriteComponentEnableMask(hueSweepEnabledMask, valueSweepEnabledMask, keypressFlashEnabledMask);

            keypad.WriteBaseColour(BaseColour);
            Console.WriteLine($"KeybindsModel.ApplySolid: Saving config to EEPROM...");
            keypad.SaveConfigToEeprom();
            Console.WriteLine($"KeybindsModel.ApplySolid: Config saved to EEPROM!");
        }
        public void ApplyBaseColour()
        {
            keypad.WriteBaseColour(BaseColour);
        }
        public void ApplyRainbow()
        {
            keypad.WriteLedsDisabled(false);

            byte hueSweepEnabledMask = 0xff;
            byte valueSweepEnabledMask = 0x00;
            byte keypressFlashEnabledMask = 0x00;
            keypad.WriteComponentEnableMask(hueSweepEnabledMask, valueSweepEnabledMask, keypressFlashEnabledMask);

            keypad.WriteBaseColour(BaseColour);
            keypad.WriteHueDelta(HueDelta);
            keypad.WriteHuePeriod(HuePeriod);
            Console.WriteLine($"KeybindsModel.ApplyRainbow: Saving config to EEPROM...");
            keypad.SaveConfigToEeprom();
            Console.WriteLine($"KeybindsModel.ApplyRainbow: Config saved to EEPROM!");
        }
        public void ApplyFade()
        {
            keypad.WriteLedsDisabled(false);

            byte hueSweepEnabledMask = 0x00;
            byte valueSweepEnabledMask = 0xff;
            byte keypressFlashEnabledMask = 0x00;
            keypad.WriteComponentEnableMask(hueSweepEnabledMask, valueSweepEnabledMask, keypressFlashEnabledMask);

            keypad.WriteBaseColour(BaseColour);
            keypad.WriteValueDim(ValueDim);
            keypad.WriteValuePeriod(ValuePeriod);
            Console.WriteLine($"KeybindsModel.ApplyFade: Saving config to EEPROM...");
            keypad.SaveConfigToEeprom();
            Console.WriteLine($"KeybindsModel.ApplyFade: Config saved to EEPROM!");
        }
        public void ApplyFlash()
        {
            keypad.WriteLedsDisabled(false);

            byte hueSweepEnabledMask = 0x00;
            byte valueSweepEnabledMask = 0x00;
            byte keypressFlashEnabledMask = 0xff;
            keypad.WriteComponentEnableMask(hueSweepEnabledMask, valueSweepEnabledMask, keypressFlashEnabledMask);

            keypad.WriteBaseColour(Enumerable.Repeat(Color.FromRgb(0, 0, 0), KeypadSerial.NUM_LEDS).ToList());
            keypad.WriteFlashLeftColour(FlashLeftColor);
            keypad.WriteFlashRightColour(FlashRightColor);
            keypad.WriteFlashBlendingMethod(FlashBlendingMethod);
            keypad.WriteFlashHoldMethod(FlashHoldMethod);
            keypad.WriteFlashDecayRate(FlashDecayRate);
            Console.WriteLine($"KeybindsModel.ApplyFlash: Saving config to EEPROM...");
            keypad.SaveConfigToEeprom();
            Console.WriteLine($"KeybindsModel.ApplyFlash: Config saved to EEPROM!");
        }
        public void TurnOff()
        {
            keypad.WriteLedsDisabled(true);
            keypad.SaveConfigToEeprom();
        }
        public bool PushAllValues()
        {
            keypad.WriteComponentEnableMask(HueSweepEnabledMask, ValueSweepEnabledMask, KeypressFlashEnabledMask);

            keypad.WriteBaseColour(BaseColour);
            keypad.WriteHueDelta(HueDelta);
            keypad.WriteHuePeriod(HuePeriod);

            keypad.WriteValueDim(ValueDim);
            keypad.WriteValuePeriod(ValuePeriod);

            keypad.WriteFlashLeftColour(FlashLeftColor);
            keypad.WriteFlashRightColour(FlashRightColor);
            keypad.WriteFlashBlendingMethod(FlashBlendingMethod);
            keypad.WriteFlashHoldMethod(FlashHoldMethod);
            keypad.WriteFlashDecayRate(FlashDecayRate);
            keypad.SaveConfigToEeprom();
            return true;
        }
    }
}
