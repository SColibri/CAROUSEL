using AMControls.Charts.DataPointContextMenu;
using AMControls.Charts.Implementations;
using AMControls.Charts.Implementations.DataSeries;
using AMControls.Charts.Interfaces;
using AMControls_Test.Charts;
using AMControls_Test.Custom;
using AMControls_Test.WindowObjects.Notify;
using Catel.Collections;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AMControls_Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Options
            Dictionary<Type, bool> enabledOptions = new Dictionary<Type, bool>();

            // ------------------------------------------------------------
            //                           PLOTS
            // ------------------------------------------------------------
            enabledOptions.Add(typeof(ScatterExample), false);
            enabledOptions.Add(typeof(ParallaxExample), true);
            enabledOptions.Add(typeof(SpyderExample), true);

            // ------------------------------------------------------------
            //                           OTHER
            // ------------------------------------------------------------
            enabledOptions.Add(typeof(NotifyCorner_Test), false);
            enabledOptions.Add(typeof(ScriptingView), false);
            enabledOptions.Add(typeof(Viewer3DTest), false);

            // Create and open selected options
            foreach (KeyValuePair<Type, bool> entry in enabledOptions)
            {
                if (!entry.Value) continue;

                Window? control = (Window?)Activator.CreateInstance(entry.Key);
                control?.Show();
            }

            // close this window
            this.Close();
        }
    }
}
