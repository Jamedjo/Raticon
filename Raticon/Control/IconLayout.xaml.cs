using Microsoft.Practices.ServiceLocation;
using Raticon.ViewModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Interop;
using System.Windows.Threading;

namespace Raticon.Control
{
    /// <summary>
    /// Interaction logic for IconLayout.xaml
    /// </summary>
    public partial class IconLayout : UserControl
    {
        public IconLayout()
        {
            InitializeComponent();
        }

        public IconLayout(object dataContext) : this()
        {
            this.DataContext = dataContext;
        }

        public Bitmap RenderToBitmap()
        {
            return RenderTargetBitmapTo32bppArgb(AsRenderTargetBitmap());
        }

        private RenderTargetBitmap AsRenderTargetBitmap()
        {
            var size = new System.Windows.Size(256, 256);
            this.Measure(size);
            this.Arrange(new Rect(size));

            RenderTargetBitmap rtb = new RenderTargetBitmap((int)size.Width, (int)size.Height, 96, 96, PixelFormats.Default);
            rtb.Render(this);
            return rtb;
        }

        private Bitmap RenderTargetBitmapTo32bppArgb(RenderTargetBitmap rtb)
        {
            MemoryStream stream = new MemoryStream();
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(rtb));
            encoder.Save(stream);

            //Bitmap png = new Bitmap((int)rtb.Width, (int)rtb.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            //System.Drawing.Graphics.FromImage(png).DrawImage(new Bitmap(stream), 0, 0);

            return new Bitmap(stream);//png;
        }
    }
}
