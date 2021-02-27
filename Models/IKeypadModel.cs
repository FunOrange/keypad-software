using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeypadSoftware.Models
{
    interface IKeypadModel
    {
        // Read values from keypad, so that keypad values are reflected onto model
        void PullAllValues();
    }
}
