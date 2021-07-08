using Caliburn.Micro;
using KeypadSoftware.Models;
using ScottPlot.Plottable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace KeypadSoftware.ViewModels
{
    public class DebounceViewModel : Screen, IKeypadViewModel
    {
        Timer writeValuesTimer;
        void RestartWriteTimer()
        {
            writeValuesTimer.Stop();
            writeValuesTimer.Start();
        }
        void WriteValuesTimerElapsed(Object source, ElapsedEventArgs e)
        {
            PushAllValues();
        }

        #region View Properties
        public DebounceModel Debounce;
        public int LeftButtonPressDebounceTime
        {
            get { return Debounce.LeftButtonPressDebounceTime; }
            set {
                Debounce.LeftButtonPressDebounceTime = value.Clamp(0, 255);
                NotifyOfPropertyChange(() => LeftButtonPressDebounceTimeString);
                RestartWriteTimer();
            }
        }
        public string LeftButtonPressDebounceTimeString
        {
            get { return Debounce.LeftButtonPressDebounceTime.ToString(); }
            set {
                int parsedValue;
                if (int.TryParse(value, out parsedValue))
                {
                    Debounce.LeftButtonPressDebounceTime = parsedValue.Clamp(0, 255);
                    NotifyOfPropertyChange(() => LeftButtonPressDebounceTime);
                    RestartWriteTimer();
                }
            }
        }

        public int LeftButtonReleaseDebounceTime
        {
            get { return Debounce.LeftButtonReleaseDebounceTime; }
            set {
                Debounce.LeftButtonReleaseDebounceTime = value.Clamp(0, 255);
                NotifyOfPropertyChange(() => LeftButtonReleaseDebounceTimeString);
                RestartWriteTimer();
            }
        }
        public string LeftButtonReleaseDebounceTimeString
        {
            get { return Debounce.LeftButtonReleaseDebounceTime.ToString(); }
            set {
                int parsedValue;
                if (int.TryParse(value, out parsedValue))
                {
                    Debounce.LeftButtonReleaseDebounceTime = parsedValue.Clamp(0, 255);
                    NotifyOfPropertyChange(() => LeftButtonReleaseDebounceTime);
                    RestartWriteTimer();
                }
            }
        }

        public int RightButtonPressDebounceTime
        {
            get { return Debounce.RightButtonPressDebounceTime; }
            set {
                Debounce.RightButtonPressDebounceTime = value.Clamp(0, 255);
                NotifyOfPropertyChange(() => RightButtonPressDebounceTimeString);
                RestartWriteTimer();
            }
        }
        public string RightButtonPressDebounceTimeString
        {
            get { return Debounce.RightButtonPressDebounceTime.ToString(); }
            set {
                int parsedValue;
                if (int.TryParse(value, out parsedValue))
                {
                    Debounce.RightButtonPressDebounceTime = parsedValue.Clamp(0, 255);
                    NotifyOfPropertyChange(() => RightButtonPressDebounceTime);
                    RestartWriteTimer();
                }
            }
        }

        public int RightButtonReleaseDebounceTime
        {
            get { return Debounce.RightButtonReleaseDebounceTime; }
            set {
                Debounce.RightButtonReleaseDebounceTime = value.Clamp(0, 255);
                NotifyOfPropertyChange(() => RightButtonReleaseDebounceTimeString);
                RestartWriteTimer();
            }
        }
        public string RightButtonReleaseDebounceTimeString
        {
            get { return Debounce.RightButtonReleaseDebounceTime.ToString(); }
            set {
                int parsedValue;
                if (int.TryParse(value, out parsedValue))
                {
                    Debounce.RightButtonReleaseDebounceTime = parsedValue.Clamp(0, 255);
                    NotifyOfPropertyChange(() => RightButtonReleaseDebounceTime);
                    RestartWriteTimer();
                }
            }
        }

        public int SideButtonDebounceTime
        {
            get { return Debounce.SideButtonDebounceTime; }
            set {
                Debounce.SideButtonDebounceTime = value.Clamp(0, 255);
                NotifyOfPropertyChange(() => SideButtonDebounceTimeString);
                RestartWriteTimer();
            }
        }
        public string SideButtonDebounceTimeString
        {
            get { return Debounce.SideButtonDebounceTime.ToString(); }
            set {
                int parsedValue;
                if (int.TryParse(value, out parsedValue))
                {
                    Debounce.SideButtonDebounceTime = parsedValue.Clamp(0, 255);
                    NotifyOfPropertyChange(() => SideButtonDebounceTime);
                    RestartWriteTimer();
                }
            }
        }
        #endregion

        public DebounceViewModel(KeypadSerial keypad)
        {
            Debounce = new DebounceModel(keypad);
            writeValuesTimer = new Timer(1000);
            writeValuesTimer.Elapsed += WriteValuesTimerElapsed;
            writeValuesTimer.AutoReset = false;
            writeValuesTimer.Enabled = false;
        }

        public void Capture()
        {
            // Update chart with new values
            var (left, right) = Debounce.ReadRawButtonStateBuffer();
            var leftRawInputData = left.Select(x => x ? 1 : 0).Select(x => x + 1.25).ToArray();
            var rightRawInputData = right.Select(x => x ? 1 : 0).Select(x => (double)x).ToArray();
            PlotEventPasserThing.RaiseChartDataUpdatedEvent(this, new ChartDataEventArgs( leftRawInputData, rightRawInputData ));
        }

        private double[] CreateDebouncedInputPlot(IEnumerable<bool> data, int pressDebounceMs, int releaseDebounceMs)
        {
            return new double[0];
        }

        public void PullAllValues()
        {
        }
        public void PushAllValues()
        {
        }
    }
}
