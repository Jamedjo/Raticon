using CommandLine;
using CommandLine.Text;
using Raticon.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raticon
{
    class ConsoleApp
    {
        class Options
        {
            [Option('f', "folder", HelpText = "Folder to scan.")]
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

        public static void Run(string[] args)
        {
            var options = new Options();
            if (!CommandLine.Parser.Default.ParseArguments(args, options))
            {
                return;
            }

            if (options.Folder != null)
            {
                var collection = new MediaCollection<CachedFilm>(options.Folder.ToString()).Items;

                foreach(var film in collection)
                {
                    Console.WriteLine(String.Format("{0,-3} {1,-9} {2}", film.Rating, film.ImdbId, film.FolderName));
                }
            }

            if (options.RunGui)
            {
                GuiApp.Run();
            }
        }
    }
}
