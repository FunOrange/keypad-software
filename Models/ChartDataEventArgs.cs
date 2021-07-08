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
        public double[] LeftRawInputYs;
        public double[] RightRawInputYs;
        public double[] LeftDebouncedInputYs;
        public double[] RightDebouncedInputYs;
        public ChartDataEventArgs(double[] leftRawInputYs, double[] rightRawInputYs, double[] leftDebouncedInputYs, double[] rightDebouncedInputYs)
        {
            LeftRawInputYs = leftRawInputYs;
            RightRawInputYs = rightRawInputYs;
            LeftDebouncedInputYs = leftDebouncedInputYs;
            RightDebouncedInputYs = rightDebouncedInputYs;
        }
    }
}
