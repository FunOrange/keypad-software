﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KeypadSoftware.Views
{
    /// <summary>
    /// Interaction logic for LightingView.xaml
    /// </summary>
    public partial class LightingView : UserControl
    {
        public LightingView()
        {
            InitializeComponent();
        }

        private void ColorPicker_ColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {
            Console.WriteLine(e.NewValue);
        }
    }
}
