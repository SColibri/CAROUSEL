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
using AMFramework_Lib.Model;

namespace AMFramework.Views.Case
{
    /// <summary>
    /// Interaction logic for Case_newitem.xaml
    /// </summary>
    public partial class Case_newitem : UserControl
    {
        public Case_newitem()
        {
            InitializeComponent();
        }

        public Case_newitem(Model_Case caseModel)
        {
            InitializeComponent();
            DataContext = caseModel;
        }
    }
}
