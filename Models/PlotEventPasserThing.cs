using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeypadSoftware.Models
{
    public class PlotEventPasserThing
    {
        public static event EventHandler<ChartDataEventArgs> ChartDataUpdated;
        public static void RaiseChartDataUpdatedEvent(object sender, ChartDataEventArgs data)
        {
            ChartDataUpdated?.Invoke(sender, data);
        }
    }
}
