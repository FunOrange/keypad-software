using Caliburn.Micro;
using KeypadSoftware.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Defaults;

namespace KeypadSoftware.ViewModels
{
    public class DebounceViewModel : Screen, IKeypadViewModel
    {
        public enum SignalInput
        {
            LiveView,
            Trigger,
            Simulate
        }
        private SignalInput _selectedInput;
        public SignalInput SelectedInput
        {
            get { return _selectedInput; }
            set {
                _selectedInput = value;
                NotifyOfPropertyChange(() => CanSelectLiveView);
                NotifyOfPropertyChange(() => CanSelectTrigger);
                NotifyOfPropertyChange(() => CanSelectSimulate);
            }
        }

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

        #region Chart Properties
        public ChartValues<ObservableValue> LeftButtonStateValues { get; set; } = new ChartValues<ObservableValue>
        {
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(1),
            new ObservableValue(1),
            new ObservableValue(1),
            new ObservableValue(0),
            new ObservableValue(1),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(1),
            new ObservableValue(1),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(1),
            new ObservableValue(0),
            new ObservableValue(1),
            new ObservableValue(1),
            new ObservableValue(1),
            new ObservableValue(1),
            new ObservableValue(1),
            new ObservableValue(1),
            new ObservableValue(1),
            new ObservableValue(1),
            new ObservableValue(1),
            new ObservableValue(1),
            new ObservableValue(1),
            new ObservableValue(1),
            new ObservableValue(0),
            new ObservableValue(1),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
        };
        public ChartValues<ObservableValue> RightButtonStateValues { get; set; } = new ChartValues<ObservableValue>
        {
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(1),
            new ObservableValue(1),
            new ObservableValue(1),
            new ObservableValue(0),
            new ObservableValue(1),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(1),
            new ObservableValue(1),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(1),
            new ObservableValue(0),
            new ObservableValue(1),
            new ObservableValue(1),
            new ObservableValue(1),
            new ObservableValue(1),
            new ObservableValue(1),
            new ObservableValue(1),
            new ObservableValue(1),
            new ObservableValue(1),
            new ObservableValue(1),
            new ObservableValue(1),
            new ObservableValue(1),
            new ObservableValue(1),
            new ObservableValue(0),
            new ObservableValue(1),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
            new ObservableValue(0),
        };
        #endregion
        public DebounceViewModel(KeypadSerial keypad)
        {
            Debounce = new DebounceModel(keypad);
            writeValuesTimer = new Timer(1000);
            writeValuesTimer.Elapsed += WriteValuesTimerElapsed;
            writeValuesTimer.AutoReset = false;
            writeValuesTimer.Enabled = false;

            // generate data points
            var r = new Random();
            List<ObservableValue> datapoints = new int[1000].Select((_) => new ObservableValue(r.Next(0, 1))).ToList();
            LeftButtonStateValues = new ChartValues<ObservableValue>(datapoints);
            NotifyOfPropertyChange(() => LeftButtonStateValues);
        }

        public void SelectLiveView()
        {
            SelectedInput = SignalInput.LiveView;
        }
        public void SelectTrigger()
        {
            SelectedInput = SignalInput.Trigger;
        }
        public void SelectSimulate()
        {
            SelectedInput = SignalInput.Simulate;
        }

        public bool CanSelectLiveView => SelectedInput != SignalInput.LiveView;
        public bool CanSelectTrigger => SelectedInput != SignalInput.Trigger;
        public bool CanSelectSimulate => SelectedInput != SignalInput.Simulate;


        public void PullAllValues()
        {
        }
        public void PushAllValues()
        {
        }
    }
}
