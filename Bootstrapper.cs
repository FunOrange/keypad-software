using Caliburn.Micro;
using KeypadSoftware.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KeypadSoftware
{
    public class Bootstrapper : BootstrapperBase
    {
        public Bootstrapper()
        {
            Initialize();
        }
       
        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<TopViewModel>();
        }
    }
}
