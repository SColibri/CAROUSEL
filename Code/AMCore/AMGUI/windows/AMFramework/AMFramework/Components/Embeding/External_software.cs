using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace AMFramework.Components.Embeding
{
    internal class External_software
    {

        public External_software()
        {

        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        static public void LoadExec(string Path_to_exe)
        {
            Process process = Process.Start(Path_to_exe);
            process.WaitForInputIdle();

            while (process.MainWindowHandle == IntPtr.Zero)
            {
                System.Threading.Thread.Sleep(10);
                process.Refresh();
            }

            Window NewWindow = new Window();
            NewWindow.Show();
            var HandleObject = new WindowInteropHelper(NewWindow).Handle;

            SetParent(process.MainWindowHandle, HandleObject);
        }

    }
}
