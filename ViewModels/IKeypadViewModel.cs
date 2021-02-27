using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeypadSoftware.ViewModels
{
    interface IKeypadViewModel
    {
        // Read values from keypad, so that model values match keypad values
        // The top view calls this function to initialize the data on the page
        void PullAllValues();
    }
}
