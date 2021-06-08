using Caliburn.Micro;
using KeypadSoftware.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace KeypadSoftware.ViewModels
{
    public class CountersViewModel : Screen, IKeypadViewModel
    {

        #region View Properties
        public CountersModel Counters { get; set; }
        public uint LeftButtonClickCount => Counters.LeftButtonClickCount;
        public uint RightButtonClickCount => Counters.RightButtonClickCount;
        public uint SideButtonClickCount => Counters.SideButtonClickCount;
        #endregion

        Timer ReadTimer;


        public CountersViewModel(KeypadSerial keypad)
        {
            Counters = new CountersModel(keypad);
            // Create a timer with a second interval
            ReadTimer = new Timer(1000);
            // Hook up the Elapsed event for the timer. 
            ReadTimer.Elapsed += ReadCounters;
            ReadTimer.AutoReset = true;
            ReadTimer.Enabled = true;
        }
        
        public void ReadCounters(Object source, ElapsedEventArgs e)
        {
            if (IsActive)
                PullAllValues();
        }

        public void onKeyDown (ActionExecutionContext context)
        {
            Console.WriteLine(context);
        }
        public void PullAllValues()
        {
            Counters.PullAllValues();
            NotifyOfPropertyChange(() => LeftButtonClickCount);
            NotifyOfPropertyChange(() => RightButtonClickCount);
            NotifyOfPropertyChange(() => SideButtonClickCount);
        }
    }
}
