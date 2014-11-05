using CommandLine;
using CommandLine.Text;
using Raticon.Model;
using Raticon.Service;
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
            [Option('l', "list", HelpText = "List ratings for all films in folder", MutuallyExclusiveSet = "action")]
            public bool List { get; set; }

            [Option('d', "decorate", HelpText = "Decorate all movies in the folder with icons", MutuallyExclusiveSet = "action")]
            public bool Decorate { get; set; }

            [Option('w', "watch", HelpText = "", MutuallyExclusiveSet = "watch")]
            public bool Watch { get; set; }

            [Option("gui", HelpText = "Run Raticon as a graphical windows app.", MutuallyExclusiveSet = "action")]
            public bool RunGui { get; set; }

            [ValueList(typeof(List<string>))]
            public IList<string> Folders { get; set; }

            [HelpOption]
            public string GetUsage()
            {
                var help = new HelpText
                {
                    AdditionalNewLineAfterOption = false,
                    AddDashesToOption = true
                };
                help.AddPreOptionsLine(@"Usage: Raticon --decorate D:\Path\To\Media");
                //help.AddPreOptionsLine(@"       Raticon --list D:\Path\To\Media");
                help.AddOptions(this);
                return help;
            }
        }

        public static void Run(string[] args)
        {
            var options = new Options();
            var parser = new CommandLine.Parser(s =>
            {
                s.CaseSensitive = false;
                s.MutuallyExclusive = true;
                s.HelpWriter = Console.Out;
                s.ParsingCulture = System.Globalization.CultureInfo.InvariantCulture;
            });
            if (!parser.ParseArguments(args, options))
            {
                return;
            }

            if (options.Folders.Count > 0)
            {
                var firstFolder = options.Folders.First();
                var collection = new MediaCollection<ConsoleFilm>(firstFolder).Items;

                if (options.List)
                {
                    foreach (var film in collection)
                    {
                        Console.WriteLine(String.Format("{0,-3} {1,-9} {2}", film.Rating, film.ImdbId, film.FolderName));
                    }
                }

                if(options.Decorate)
                {
                    new IconService().ProcessValidFilms(collection, validFilms =>
                        Console.WriteLine("Complete! " + validFilms.Count() + " folders have been decorated with icons."));
                }

                if(options.Watch)
                {
                    new ConsoleFilmProcessingWatcher(firstFolder).InfiniteWait();
                }
            }

            if (options.RunGui)
            {
                GuiApp.Run();
            }
        }
    }
}
