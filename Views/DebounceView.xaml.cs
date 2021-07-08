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
            leftSignalPlot.MarkerSize = 1;
            leftSignalPlot.Color = System.Drawing.Color.FromArgb(140, 103, 58, 183);
            // left button (debounced)
            var leftDebouncedSignalPlot = SignalView.Plot.AddSignal(leftButtonData);
            leftDebouncedSignalPlot.MarkerSize = 1;
            leftDebouncedSignalPlot.LineWidth = 3;
            leftDebouncedSignalPlot.Color = System.Drawing.Color.FromArgb(103, 58, 183);


            // right button (raw) line
            double[] rightButtonData = DataGen.Random( new Random(), pointCount: 5000, multiplier: 0, offset: 0 );
            var rightSignalPlot = SignalView.Plot.AddSignal(rightButtonData);
            rightSignalPlot.MarkerSize = 1;
            rightSignalPlot.Color = System.Drawing.Color.FromArgb(140, 255, 109, 0);
            // right button (debounced)
            var rightDebouncedSignalPlot = SignalView.Plot.AddSignal(rightButtonData);
            rightDebouncedSignalPlot.MarkerSize = 1;
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

            SignalView.Render();
        }
    }
}
