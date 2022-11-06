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

namespace AMFramework.Views.Case
{
    /// <summary>
    /// Interaction logic for CaseView_Data.xaml
    /// </summary>
    public partial class CaseView_Data : UserControl
    {
        public CaseView_Data()
        {
            InitializeComponent();
        }

        public CaseView_Data(Controller.Controller_Cases cCase)
        {
            InitializeComponent();
            DataContext = cCase;
        }
    }
}
