using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raticon
{
    class GuiApp
    {
        public static void Run()
        {
            Raticon.App app = new Raticon.App();
            app.InitializeComponent();
            app.Run();
        }
    }
}
