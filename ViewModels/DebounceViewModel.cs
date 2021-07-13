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

        public DebounceModel Debounce;

        #region View Properties
        public int LeftButtonPressDebounceTime
        {
            get { return Debounce.LeftButtonPressDebounceTime; }
            set {
                Debounce.LeftButtonPressDebounceTime = value.Clamp(0, 255);
                NotifyOfPropertyChange(() => LeftButtonPressDebounceTimeString);
                RestartWriteTimer();
                UpdateChartDebounceOnly();
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
                    UpdateChartDebounceOnly();
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
                UpdateChartDebounceOnly();
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
                    UpdateChartDebounceOnly();
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
                UpdateChartDebounceOnly();
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
                    UpdateChartDebounceOnly();
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
                UpdateChartDebounceOnly();
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
                    UpdateChartDebounceOnly();
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
                UpdateChartDebounceOnly();
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
                    UpdateChartDebounceOnly();
                }
            }
        }
        #endregion
        void NotifyAllProperties()
        {
            NotifyOfPropertyChange(() => LeftButtonPressDebounceTime);
            NotifyOfPropertyChange(() => LeftButtonPressDebounceTimeString);
            NotifyOfPropertyChange(() => LeftButtonReleaseDebounceTime);
            NotifyOfPropertyChange(() => LeftButtonReleaseDebounceTimeString);
            NotifyOfPropertyChange(() => RightButtonPressDebounceTime);
            NotifyOfPropertyChange(() => RightButtonPressDebounceTimeString);
            NotifyOfPropertyChange(() => RightButtonReleaseDebounceTime);
            NotifyOfPropertyChange(() => RightButtonReleaseDebounceTimeString);
            NotifyOfPropertyChange(() => SideButtonDebounceTime);
            NotifyOfPropertyChange(() => SideButtonDebounceTimeString);
        }

        public DebounceViewModel(KeypadSerial keypad)
        {
            Debounce = new DebounceModel(keypad);
            writeValuesTimer = new Timer(500);
            writeValuesTimer.Elapsed += WriteValuesTimerElapsed;
            writeValuesTimer.AutoReset = false;
            writeValuesTimer.Enabled = false;
        }

        public void Capture()
        {
            // Read raw input state buffer from keypad
            var (left, right) = Debounce.ReadRawButtonStateBuffer();
            // Update chart with new values
            UpdateChart(left, right);
        }

        static int pulseWidthMin = 115;
        static int pulseWidthMax = 125;
        static int bounceDurationMin = 2;
        static int bounceDurationMax = 10;
        static double bounceProbability = 0.5;
        bool leftTrueState = true;
        bool rightTrueState = true;
        public void Random()
        {
            // Update chart with new values
            Debounce.LeftRawInput = GenerateRandomInput().ToList();
            Debounce.RightRawInput = GenerateRandomInput(false).ToList();
            UpdateChart(Debounce.LeftRawInput, Debounce.RightRawInput);
        }

        bool[] GenerateRandomInput(bool initialState = true)
        {
            bool[] rawInput = new bool[5000];
            var rand = new Random();
            bool trueInput = initialState;
            int pulseProgress = 0;
            int currentBounceDuration = 0;
            for (int t = 0; t < 5000; t++)
            {
                if (pulseProgress == 0)
                {
                    trueInput = !trueInput;
                    pulseProgress = rand.Next(pulseWidthMin, pulseWidthMax);
                    currentBounceDuration = rand.Next(bounceDurationMin, bounceDurationMax);
                }
                else if (pulseProgress < currentBounceDuration)
                {
                    if (rand.NextDouble() < bounceProbability)
                        rawInput[t] = !trueInput;
                    else
                        rawInput[t] = trueInput;
                }
                else
                {
                    rawInput[t] = trueInput;
                }

                pulseProgress--;
            }
            return rawInput;
        }

        void UpdateChartDebounceOnly()
        {
            // Use last read data
            var left = Debounce.LeftRawInput;
            var right = Debounce.RightRawInput;
            UpdateChart(left, right);
        }

        void UpdateChart(IEnumerable<bool> left, IEnumerable<bool> right)
        {
            // left
            double[] leftRawInputYs = left.Select(x => x ? 2.25 : 1.25).ToArray();
            double[] leftDebouncedInputYs =
                CreateDebouncedInput(left, Debounce.LeftButtonPressDebounceTime, Debounce.LeftButtonReleaseDebounceTime)
                .Select(x => x ? 2.25 : 1.25)
                .ToArray();

            // right
            double[] rightRawInputYs = right.Select(x => x ? 1.0 : 0.0).ToArray();
            double[] rightDebouncedInputYs =
                CreateDebouncedInput(right, Debounce.RightButtonPressDebounceTime, Debounce.RightButtonReleaseDebounceTime)
                .Select(x => x ? 1.0 : 0.0)
                .ToArray();

            // chart update event
            var eventArgs = new ChartDataEventArgs(leftRawInputYs, rightRawInputYs, leftDebouncedInputYs, rightDebouncedInputYs);
            PlotEventPasserThing.RaiseChartDataUpdatedEvent(this, eventArgs);
        }



        private bool[] CreateDebouncedInput(IEnumerable<bool> data, int pressDebounceMs, int releaseDebounceMs)
        {
            var debouncedData = new List<bool>();

            // state variables
            bool currentDebouncedState = false;
            int transitionStability = 0;


            foreach (bool state in data)
            {
                // if transition detected
                if (state != currentDebouncedState)
                {
                    // if false -> true (press transition)
                    if (currentDebouncedState == false)
                    {
                        if (transitionStability++ == pressDebounceMs)
                        {
                            // key pressed event
                            currentDebouncedState = true;
                            transitionStability = 0;
                        }
                    }
                    // if true -> false (release transition)
                    else if (currentDebouncedState == true)
                    {
                        // debounced status is true (pressed), looking at release transition
                        if (transitionStability++ == releaseDebounceMs)
                        {
                            // key released event
                            currentDebouncedState = false;
                            transitionStability = 0;
                        }
                    }
                }
                // no button state change, or bounce occurred while transitioning between states
                else
                {
                    if (transitionStability > 0)
                    {
                        // bounce event
                    }
                    transitionStability = 0;
                }
                debouncedData.Add(currentDebouncedState);
            }
            return debouncedData.ToArray();
        }

        public void PullAllValues()
        {
            Debounce.PullAllValues();
            NotifyAllProperties();
        }
        public void PushAllValues()
        {
            Debounce.PushAllValues();
        }
    }
}
