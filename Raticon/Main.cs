using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Raticon
{
    public class EntryPoint
    {
        //[DllImport("kernel32.dll", SetLastError = true)]
        //static extern bool AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool FreeConsole();

        [DllImport("kernel32", SetLastError = true)]
        static extern bool AttachConsole(int dwProcessId);
 
        /// <summary>
        /// Application Entry Point.
        /// </summary>
        [System.STAThreadAttribute()]
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public static void Main(string[] args)
        {

            if (!AttachConsole(-1))
            {
                Raticon.App app = new Raticon.App();
                app.InitializeComponent();
                app.Run();
            }
            else
            {
                Console.WriteLine("Hi there");

                FreeConsole();
            }
        }

    }
}
