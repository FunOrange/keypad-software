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
                Debounce.LeftButtonPressDebounceTime = value;
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
                    Debounce.LeftButtonPressDebounceTime = parsedValue;
                    NotifyOfPropertyChange(() => LeftButtonPressDebounceTime);
                    RestartWriteTimer();
                }
            }
        }

        public int LeftButtonReleaseDebounceTime
        {
            get { return Debounce.LeftButtonReleaseDebounceTime; }
            set {
                Debounce.LeftButtonReleaseDebounceTime = value;
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
                    Debounce.LeftButtonReleaseDebounceTime = parsedValue;
                    NotifyOfPropertyChange(() => LeftButtonReleaseDebounceTime);
                    RestartWriteTimer();
                }
            }
        }

        public int RightButtonPressDebounceTime
        {
            get { return Debounce.RightButtonPressDebounceTime; }
            set {
                Debounce.RightButtonPressDebounceTime = value;
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
                    Debounce.RightButtonPressDebounceTime = parsedValue;
                    NotifyOfPropertyChange(() => RightButtonPressDebounceTime);
                    RestartWriteTimer();
                }
            }
        }

        public int RightButtonReleaseDebounceTime
        {
            get { return Debounce.RightButtonReleaseDebounceTime; }
            set {
                Debounce.RightButtonReleaseDebounceTime = value;
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
                    Debounce.RightButtonReleaseDebounceTime = parsedValue;
                    NotifyOfPropertyChange(() => RightButtonReleaseDebounceTime);
                    RestartWriteTimer();
                }
            }
        }

        public int SideButtonDebounceTime
        {
            get { return Debounce.SideButtonDebounceTime; }
            set {
                Debounce.SideButtonDebounceTime = value;
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
                    Debounce.SideButtonDebounceTime = parsedValue;
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
