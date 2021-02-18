using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeypadSoftware.Models
{
    class DebounceModel
    {
        public bool Initialized = false;
        public int LeftButtonPressDebounceTime;
        public int LeftButtonReleaseDebounceTime;
        public int RightButtonPressDebounceTime;
        public int RightButtonReleaseDebounceTime;
        public int SideButtonDebounceTime;
    }
}
