using System;
using System.Globalization;
using System.Windows.Controls;

namespace KeypadSoftware
{
    class HexadecimalRule : ValidationRule
    {
        public HexadecimalRule() { }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
                return ValidationResult.ValidResult;
            string stringValue = (string)value;
            if (stringValue.StartsWith("0x"))
                stringValue = stringValue.Remove(0, 2);
            int hexValue = 0;

            try
            {
                if (stringValue.Length > 0)
                    hexValue = int.Parse(stringValue, NumberStyles.HexNumber);
            }
            catch (Exception)
            {
                return new ValidationResult(false, "Invalid hex number");
            }
            if (hexValue < 0 || hexValue > 0xff)
            {
                return new ValidationResult(false, "Not between 0x0 - 0xff");
            }

            return ValidationResult.ValidResult;
        }
    }
}
