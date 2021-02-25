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
        void PullAllValues();
        // Write values to keypad, so that keypad values match model values
        void PushAllValues();
    }
}
