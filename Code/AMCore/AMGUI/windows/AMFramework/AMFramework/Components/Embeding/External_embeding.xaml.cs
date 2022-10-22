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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AMFramework.Components.Embeding
{
    /// <summary>
    /// Interaction logic for External_embeding.xaml
    /// </summary>
    public partial class External_embeding : UserControl
    {
        public External_embeding()
        {
            InitializeComponent();
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        private void LoadExec(string Path_to_exe)
        {
            Process process = Process.Start(Path_to_exe);
            process.WaitForInputIdle();

            while (process.MainWindowHandle == IntPtr.Zero)
            {
                System.Threading.Thread.Sleep(10);
                process.Refresh();
            }

            Window NewWindow = new();
            NewWindow.Show();
            var HandleObject = new WindowInteropHelper(NewWindow).Handle;

            SetParent(process.MainWindowHandle, HandleObject);
        }
    }
}
