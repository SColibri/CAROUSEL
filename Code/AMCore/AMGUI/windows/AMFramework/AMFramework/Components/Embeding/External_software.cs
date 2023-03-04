﻿using System;
using System.Diagnostics;
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

        public static void LoadExec(string Path_to_exe)
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
