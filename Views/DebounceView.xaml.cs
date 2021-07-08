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

            double[] xs = DataGen.Range(0, 5000, 1);

            // left button
            double[] leftButtonData = DataGen.Random( new Random(), pointCount: 5000, multiplier: 0, offset: 1.25 );
            var leftSignalPlot = SignalView.Plot.AddSignal(leftButtonData);
            SignalView.Plot.PlotFill(xs, leftButtonData, baseline: 1.25, fillColor: System.Drawing.Color.FromArgb(70, 20, 20, 200));
            leftSignalPlot.MarkerSize = 1;

            // right button
            double[] rightButtonData = DataGen.Random( new Random(), pointCount: 5000, multiplier: 0, offset: 0 );
            var rightSignalPlot = SignalView.Plot.AddSignal(rightButtonData);
            SignalView.Plot.PlotFill(xs, rightButtonData, baseline: 0, fillColor: System.Drawing.Color.FromArgb(70, 200, 20, 20));
            rightSignalPlot.MarkerSize = 1;

            // X Axis
            SignalView.Plot.XAxis.MinimumTickSpacing(1);
            SignalView.Plot.XAxis.Label("Milliseconds");

            // Y Axis
            //SignalView.Plot.YAxis.Ticks(false);
            SignalView.Plot.YAxis.Grid(false);
            double[] ys = new double[] { 0, 1, 1.25, 2.25 };
            string[] ylabels = new string[] { "released", "pressed", "released", "pressed" };
            SignalView.Plot.YTicks(ys, ylabels);

            SignalView.Plot.SetAxisLimitsY(-0.1, 2.25 + 0.1);
        }

        private void UpdateChart(object sender, ChartDataEventArgs e)
        {
            var leftRawInputPlot = (SignalPlot)SignalView.Plot.GetPlottables()[0];
            leftRawInputPlot.Ys = e.LeftRawInput;
            var leftRawInputFill = (ScottPlot.Plottable.Polygon)SignalView.Plot.GetPlottables()[1];
            leftRawInputPlot.Ys.CopyTo(leftRawInputFill.Ys, 1);

            var rightRawInputPlot = (SignalPlot)SignalView.Plot.GetPlottables()[2];
            rightRawInputPlot.Ys = e.RightRawInput;
            var rightRawInputFill = (ScottPlot.Plottable.Polygon)SignalView.Plot.GetPlottables()[1];
            rightRawInputPlot.Ys.CopyTo(rightRawInputFill.Ys, 1);

            SignalView.Render();
        }
    }
}
