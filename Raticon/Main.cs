using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Raticon
{
    public class EntryPoint
    {

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool FreeConsole();

        [DllImport("kernel32", SetLastError = true)]
        static extern bool AttachConsole(int dwProcessId);

        const int ATTACH_PARENT_PROCESS = -1;

        class Options
        {
            [Option('f', "folder", HelpText = "Folder to scan read.")]
            public string Folder { get; set; }

            [Option("gui", HelpText = "Run Raticon as a graphical windows app.")]
            public bool RunGui { get; set; }

            [HelpOption]
            public string GetUsage()
            {
                var help = new HelpText
                {
                    AdditionalNewLineAfterOption = false,
                    AddDashesToOption = true
                };
                help.AddPreOptionsLine(@"Usage: Raticon -f D:\Path\To\Media");
                help.AddOptions(this);
                return help;
            }

        }
 
        /// <summary>
        /// Application Entry Point.
        /// </summary>
        [System.STAThreadAttribute()]
        public static void Main(string[] args)
        {
            if (!AttachConsole(ATTACH_PARENT_PROCESS))
            {
                StartGuiApp();
            }
            else
            {
                RunConsoleApp(args);
                FreeConsole();
            }
        }

        static void StartGuiApp()
        {
            Raticon.App app = new Raticon.App();
            app.InitializeComponent();
            app.Run();
        }

        static void RunConsoleApp(string[] args)
        {
            var options = new Options();
            if (!CommandLine.Parser.Default.ParseArguments(args, options))
            {
                return;
            }

            if(options.RunGui)
            {
                StartGuiApp();
            }
        }
    }
}
