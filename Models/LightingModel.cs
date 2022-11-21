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
        public List<Color> BaseColor;

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
        public byte[] FlashBlendingMethod;
        public byte[] FlashHoldMethod;
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

            BaseColor = keypad.ReadBaseColour();
            HueDelta = keypad.ReadHueDelta();
            HuePeriod = keypad.ReadHuePeriod();

            ValueDim = keypad.ReadValueDim();
            ValuePeriod = keypad.ReadValuePeriod();

            FlashLeftColor = keypad.ReadFlashLeftColour();
            FlashRightColor = keypad.ReadFlashRightColour();
            FlashBlendingMethod = keypad.ReadFlashBlendingMethod();
            FlashHoldMethod = keypad.ReadFlashHoldMethod();
            FlashDecayRate = keypad.ReadFlashDecayRate();

            LedsDisabled = keypad.ReadLedsDisabled();
        }

        public bool PushAllValues()
        {
            keypad.WriteComponentEnableMask(HueSweepEnabledMask, ValueSweepEnabledMask, KeypressFlashEnabledMask);

            keypad.WriteBaseColour(BaseColor);
            keypad.WriteHueDelta(HueDelta);
            keypad.WriteHuePeriod(HuePeriod);

            keypad.WriteValueDim(ValueDim);
            keypad.WriteValuePeriod(ValuePeriod);

            keypad.WriteFlashLeftColour(FlashLeftColor);
            keypad.WriteFlashRightColour(FlashRightColor);
            keypad.WriteFlashBlendingMethod(FlashBlendingMethod);
            keypad.WriteFlashHoldMethod(FlashHoldMethod);
            keypad.WriteFlashDecayRate(FlashDecayRate);

            keypad.WriteLedsDisabled(LedsDisabled);
            return true;
        }
    }
}
