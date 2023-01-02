using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeypadSoftware
{
    class HSVConverter
    {
        public static void ColorToHSV(byte r, byte g, byte b, out double hue, out double saturation, out double value)
        {
            Color color = Color.FromArgb(r, g, b);
            int max = Math.Max(color.R, Math.Max(color.G, color.B));
            int min = Math.Min(color.R, Math.Min(color.G, color.B));

            hue = color.GetHue();
            saturation = (max == 0) ? 0 : 1d - (1d * min / max);
            value = max / 255d;
        }

        public static void ColorFromHSV(double hue, double saturation, double value, out byte r, out byte g, out byte b)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
            {
                r = (byte)v;
                g = (byte)t;
                b = (byte)p;
            }
            else if (hi == 1)
            {
                r = (byte)q;
                g = (byte)v;
                b = (byte)p;
            }
            else if (hi == 2)
            {
                r = (byte)p;
                g = (byte)v;
                b = (byte)t;
            }
            else if (hi == 3)
            {
                r = (byte)p;
                g = (byte)q;
                b = (byte)v;
            }
            else if (hi == 4)
            {
                r = (byte)t;
                g = (byte)p;
                b = (byte)v;
            }
            else
            {
                r = (byte)v;
                g = (byte)p;
                b = (byte)q;
            }
        }
    }
}
