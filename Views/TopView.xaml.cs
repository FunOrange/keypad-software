using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace KeypadSoftware.Views
{
    /// <summary>
    /// Interaction logic for TopView.xaml
    /// </summary>
    public partial class TopView : Window
    {
        public TopView()
        {
            InitializeComponent();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }

        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("minimizing");
            WindowState = WindowState.Minimized;
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("closing");
            Close();
        }
    }
}
