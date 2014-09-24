using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Raticon
{
    public class EntryPoint
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr GetForegroundWindow();

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool FreeConsole();

        [DllImport("kernel32", SetLastError = true)]
        static extern bool AttachConsole(int dwProcessId);

        const int ATTACH_PARENT_PROCESS = -1;
 
        /// <summary>
        /// Application Entry Point.
        /// </summary>
        [System.STAThreadAttribute()]
        public static void Main(string[] args)
        {
            if (!AttachConsole(ATTACH_PARENT_PROCESS))
            {
                GuiApp.Run();
            }
            else
            {
                ConsoleApp.Run(args);
                DisposeConsole();
            }
        }

        static void DisposeConsole()
        {
            if (GetConsoleWindow() == GetForegroundWindow())
            {
                SendKeys.SendWait("{ENTER}");
            }
            FreeConsole();
        }

    }
}
