using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edge_Detection
{
    class Redraw
    {
        public Bitmap draw(int Sensitivity,int[,] RedChannel,int[,] GreenChannel,int[,] BlueChannel,int height,int width)
        {
            Bitmap result = new Bitmap(width, height);
            LockBitmap res = new LockBitmap(result);
            res.LockBits();
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    if (RedChannel[i, j] >= Sensitivity || GreenChannel[i, j] >= Sensitivity )
                        res.SetPixel(i, j, Properties.Settings.Default.Pen);
                    else
                        res.SetPixel(i, j, Properties.Settings.Default.Pallete);
            res.UnlockBits();
            return result;
        } 
    }
}
