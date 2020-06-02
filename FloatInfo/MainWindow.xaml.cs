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

namespace FloatInfo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        private FloatInfoViewModel viewModel;
        public MainWindow()
        {
            InitializeComponent();
            viewModel = base.DataContext as FloatInfoViewModel;
            viewModel.OnInfo += ViewModel_OnInfo;
        }

        private void ViewModel_OnInfo(object sender, string e)
        {
            throw new NotImplementedException();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            viewModel.QueryKeyboardAsync();
        }
    }
}
