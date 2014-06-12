using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.IconLib;
using System.Linq;
using System.Text;

namespace Raticon.Service
{
    public class PngToIcoService
    {
        public void Convert(Bitmap bitmap, string icoPath)
        {
            MultiIcon mIcon = new MultiIcon();
            mIcon.Add("Untitled").CreateFrom(bitmap, IconOutputFormat.FromWin95);
            mIcon.SelectedIndex = 0;
            mIcon.Save(icoPath, MultiIconFormat.ICO);
        }
    }
}
