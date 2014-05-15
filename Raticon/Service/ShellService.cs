using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Raticon.Service
{
    public class ShellService
    {
        public string Execute(string command, string workingDirectory = null)
        {
            string output;
            var startInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                RedirectStandardOutput = true, UseShellExecute =  false,
                FileName = "cmd.exe",
                Arguments = "/C " + command,
                WorkingDirectory = workingDirectory ?? System.Environment.SystemDirectory
            };

            using (var process = Process.Start(startInfo))
            {
                process.WaitForExit();
                output = process.StandardOutput.ReadToEnd();
            }
            return output;
        }
    }
}
