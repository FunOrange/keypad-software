using ScottPlot.Plottable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeypadSoftware.Models
{
    public class ChartDataEventArgs : EventArgs
    {
        public double[] LeftRawInput;
        public double[] RightRawInput;
        //public double[] LeftDebouncedInput;
        //public double[] RightDebouncedInput;
        public ChartDataEventArgs(double[] leftRawInput, double[] rightRawInput)
        {
            LeftRawInput = leftRawInput;
            RightRawInput = rightRawInput;
        }
    }
}
