using KeypadSoftware.Models;
using ScottPlot;
using ScottPlot.Plottable;
using System;
using System.Collections.Generic;
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

namespace KeypadSoftware.Views
{
    /// <summary>
    /// Interaction logic for DebounceView.xaml
    /// </summary>
    public partial class DebounceView : UserControl
    {
        private const double COUNTER_VISIBLE_THRESHOLD = 45;
        public DebounceView()
        {
            InitializeComponent();

            PlotEventPasserThing.ChartDataUpdated += UpdateChart;

            SignalView.Configuration.LockVerticalAxis = true;
            SignalView.Configuration.Quality = ScottPlot.Control.QualityMode.High;
            SignalView.Plot.Frame(false);
            SignalView.Plot.SetViewLimits(0, 5000);
            SignalView.Plot.Layout(padding: 0);

            double[] xs = DataGen.Range(0, 5000, 1);

            // left button (raw) line
            double[] leftButtonData = DataGen.Random( new Random(), pointCount: 5000, multiplier: 0, offset: 1.25 );
            var leftSignalPlot = SignalView.Plot.AddSignal(leftButtonData);
            leftSignalPlot.MarkerSize = 4;
            leftSignalPlot.Color = System.Drawing.Color.FromArgb(140, 103, 58, 183);
            // left button (debounced)
            var leftDebouncedSignalPlot = SignalView.Plot.AddSignal(leftButtonData);
            leftDebouncedSignalPlot.MarkerSize = 4;
            leftDebouncedSignalPlot.LineWidth = 3;
            leftDebouncedSignalPlot.Color = System.Drawing.Color.FromArgb(103, 58, 183);


            // right button (raw) line
            double[] rightButtonData = DataGen.Random( new Random(), pointCount: 5000, multiplier: 0, offset: 0 );
            var rightSignalPlot = SignalView.Plot.AddSignal(rightButtonData);
            rightSignalPlot.MarkerSize = 4;
            rightSignalPlot.Color = System.Drawing.Color.FromArgb(140, 255, 109, 0);
            // right button (debounced)
            var rightDebouncedSignalPlot = SignalView.Plot.AddSignal(rightButtonData);
            rightDebouncedSignalPlot.MarkerSize = 4;
            rightDebouncedSignalPlot.LineWidth = 3;
            rightDebouncedSignalPlot.Color = System.Drawing.Color.FromArgb(255, 109, 0);

            // X Axis
            SignalView.Plot.XAxis.MinimumTickSpacing(1);
            SignalView.Plot.XAxis.Label("Milliseconds");

            // Y Axis
            SignalView.Plot.YAxis.Grid(false);
            double[] ys = new double[] { 0, 1, 1.25, 2.25 };
            string[] ylabels = new string[] { "released", "pressed", "released", "pressed" };
            SignalView.Plot.YTicks(ys, ylabels);

            SignalView.Plot.SetAxisLimitsY(-0.1, 2.25 + 0.1);
        }

        private void UpdateChart(object sender, ChartDataEventArgs e)
        {
            if (e.LeftRawInputYs != null)
            {
                var leftRawInputPlot = (SignalPlot)SignalView.Plot.GetPlottables()[0];
                leftRawInputPlot.Ys = e.LeftRawInputYs;
            }
            if (e.LeftDebouncedInputYs != null)
            {
                var leftDebouncedInputPlot = (SignalPlot)SignalView.Plot.GetPlottables()[1];
                leftDebouncedInputPlot.Ys = e.LeftDebouncedInputYs;
            }

            if (e.RightRawInputYs != null)
            {
                var rightRawInputPlot = (SignalPlot)SignalView.Plot.GetPlottables()[2];
                rightRawInputPlot.Ys = e.RightRawInputYs;
            }
            if (e.RightDebouncedInputYs != null)
            {
                var rightDebouncedInputPlot = (SignalPlot)SignalView.Plot.GetPlottables()[3];
                rightDebouncedInputPlot.Ys = e.RightDebouncedInputYs;
            }

            countersPlotted = new bool[5000];
            // clear all text
            var allText = SignalView.Plot.GetPlottables().Where((plot) => plot.GetType() == typeof(Text));
            foreach (IPlottable text in allText)
                SignalView.Plot.Remove(text);
            countersPlotted = new bool[5000];
            if (SignalView.Plot.GetAxisLimits().XSpan < COUNTER_VISIBLE_THRESHOLD)
                PlotCounters();

            SignalView.Render();
        }

        bool[] countersPlotted = new bool[5000];
        private void PlotCounters()
        {
            IPlottable[] plots = SignalView.Plot.GetPlottables();
            double[] leftRawInputPlot = ((SignalPlot)plots[0]).Ys;
            double[] leftDebouncedInputPlot = ((SignalPlot)plots[1]).Ys;
            double[] rightRawInputPlot = ((SignalPlot)plots[2]).Ys;
            double[] rightDebouncedInputPlot = ((SignalPlot)plots[3]).Ys;
            int pointCount = leftRawInputPlot.Length;

            double xmin = SignalView.Plot.GetAxisLimits().XMin;
            double xmax = SignalView.Plot.GetAxisLimits().XMax;
            // Work out transition stability counters
            int transitionStabilityCounter = 0;
            double previousRawY = 0;
            for (int x = 0; x < pointCount; x++) {
                double rawy = leftRawInputPlot[x];
                double deby = leftDebouncedInputPlot[x];
                if (rawy != deby)
                {
                    if (x > xmin && x < xmax && !countersPlotted[x])
                    {
                        countersPlotted[x] = true;
                        Text text = SignalView.Plot.AddText(transitionStabilityCounter.ToString(), x, rawy);
                        text.Alignment = (rawy > (1.25 + 2.25) / 2) ? Alignment.LowerCenter : Alignment.UpperCenter;
                        text.Color = System.Drawing.Color.FromArgb(103, 58, 183);
                    }
                    transitionStabilityCounter++;
                }
                else if (transitionStabilityCounter > 0 && previousRawY == rawy)
                {
                    if (x > xmin && x < xmax && !countersPlotted[x])
                    {
                        countersPlotted[x] = true;
                        Text text = SignalView.Plot.AddText(transitionStabilityCounter.ToString() + "!", x, rawy);
                        text.Alignment = (rawy > (1.25 + 2.25) / 2) ? Alignment.LowerCenter : Alignment.UpperCenter;
                        text.Color = System.Drawing.Color.FromArgb(103, 58, 183);
                    }
                    transitionStabilityCounter = 0;
                }
                else
                    transitionStabilityCounter = 0;
                previousRawY = rawy;
            }
            for (int x = 0; x < pointCount; x++) {
                double rawy = rightRawInputPlot[x];
                double deby = rightDebouncedInputPlot[x];
                if (rawy != deby)
                {
                    if (x > xmin && x < xmax && !countersPlotted[x])
                    {
                        countersPlotted[x] = true;
                        Text text = SignalView.Plot.AddText(transitionStabilityCounter.ToString(), x, rawy);
                        text.Alignment = (rawy > 0.5) ? Alignment.LowerCenter : Alignment.UpperCenter;
                        text.Color = System.Drawing.Color.FromArgb(255, 109, 0);
                    }
                    transitionStabilityCounter++;
                }
                else if (transitionStabilityCounter > 0 && previousRawY == rawy)
                {
                    if (x > xmin && x < xmax && !countersPlotted[x])
                    {
                        countersPlotted[x] = true;
                        Text text = SignalView.Plot.AddText(transitionStabilityCounter.ToString() + "!", x, rawy);
                        text.Alignment = (rawy > 0.5) ? Alignment.LowerCenter : Alignment.UpperCenter;
                        text.Color = System.Drawing.Color.FromArgb(255, 109, 0);
                    }
                    transitionStabilityCounter = 0;
                }
                else
                    transitionStabilityCounter = 0;
                previousRawY = rawy;
            }
        }

        private void SignalView_AxesChanged(object sender, EventArgs e)
        {
            // Check if zoom level is at the level where counters are visible
            if (SignalView.Plot.GetAxisLimits().XSpan < COUNTER_VISIBLE_THRESHOLD)
                PlotCounters();
            else
            {
                // clear all text
                var allText = SignalView.Plot.GetPlottables().Where((plot) => plot.GetType() == typeof(Text));
                foreach (IPlottable text in allText)
                    SignalView.Plot.Remove(text);
                countersPlotted = new bool[5000];
            }
        }
    }
}
