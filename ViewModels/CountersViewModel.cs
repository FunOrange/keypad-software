using Caliburn.Micro;
using KeypadSoftware.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public CountersViewModel(KeypadSerial keypad)
        {
            Counters = new CountersModel(keypad);
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
