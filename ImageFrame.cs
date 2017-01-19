using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edge_Detection
{
    class ImageFrame
    {
        public Bitmap AddFrame(Bitmap Image)
        {
            int width = Image.Width;
            int height = Image.Height;
            int fpix = Properties.Settings.Default.FramingPixel;
            Color c = Properties.Settings.Default.FramingColor;
            Bitmap result = new Bitmap(width + (2 * fpix), height + (2 * fpix));
            LockBitmap org = new LockBitmap(Image);
            LockBitmap res = new LockBitmap(result);
            org.LockBits();
            res.LockBits();
            for (int i = 0; i < fpix; i++)
                for (int j = 0; j < width + (2 * fpix); j++)
                {
                    res.SetPixel(j, i, c);
                    res.SetPixel(j, height+fpix+i, c);
                }
            for(int i=fpix;i<height+fpix;i++)
            {
                for (int j = 0; j < fpix; j++)
                    res.SetPixel(j, i, c);
                for (int j = fpix; j < width + fpix; j++)
                {
                    int x=org.GetPixel(j-fpix,i-fpix).R;
                    int y=org.GetPixel(j - fpix, i-fpix).G;
                    int z= org.GetPixel(j - fpix, i-fpix).B; 
                    res.SetPixel(j, i, Color.FromArgb(x,y,z));
                }
                for (int j = width+fpix; j < width+(2*fpix); j++)
                    res.SetPixel(j, i, c);
            }
            res.UnlockBits();
            org.UnlockBits();
            return result;
        }
    }
}
