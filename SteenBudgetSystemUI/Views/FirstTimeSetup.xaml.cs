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
using System.Windows.Shapes;
using SteenBudgetSystemLib.Services;
using SteenBudgetSystemLib.ViewModel;

namespace SteenBudgetSystemUI
{
    /// <summary>
    /// Interaction logic for FirstTimeSetup.xaml
    /// </summary>
    public partial class FirstTimeSetup : Window
    {
        public FirstTimeSetup(FirstTimeSetupViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }

}
