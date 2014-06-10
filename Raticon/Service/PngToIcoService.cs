using System;
using System.Collections.Generic;
using System.Drawing.IconLib;
using System.Linq;
using System.Text;

namespace Raticon.Service
{
    public class PngToIcoService
    {
        public void Convert(string pngPath, string icoPath)
        {
            MultiIcon mIcon = new MultiIcon();
            mIcon.Add("Untitled").CreateFrom(pngPath, IconOutputFormat.FromWin95);
            mIcon.SelectedIndex = 0;
            mIcon.Save(icoPath, MultiIconFormat.ICO);
        }
    }
}
