using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KeypadSoftware.Views
{
    /// <summary>
    /// Interaction logic for DebugView.xaml
    /// </summary>
    public partial class DebugView : UserControl
    {
        public static DebugView Instance;
        Stopwatch sw;
        public DebugView()
        {
            InitializeComponent();
            Instance = this;
            sw = new Stopwatch();
            sw.Start();
        }

        public void ForceSelectedColor(Color c) {
            if (sw.IsRunning && sw.ElapsedMilliseconds < 10) // prevent infinite recursion
                return;

            sw.Restart();
            sw.Start();

            BaseColourAllCp.SelectedColor = c;

        }
    }
}
