using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Edge_Detection
{
    class RobertsCross
    {
        //This Class is built for creating Edge based image on Robert's Cross Operator
        Bitmap original1, original2, original3;// Three Instances of BITMAP Class
        int width, height;//Stores Height and Width of the Image received as input
        public int[,] bufferr; public int[,] bufferg; public int[,] bufferb;//Stores the dot position in respect to Red, Green and Blue Colour Channel of the Image
        Boolean rdone = false, gdone = false, bdone = false;
        public Bitmap convert(Bitmap original, int sensitivity)
        {
            Copy cp = new Copy();
            //Performing Deepcopy in the 3 Instance of Bitmap object
            original1 = cp.DeepCopy(original);
            original2 = cp.DeepCopy(original);
            original3 = cp.DeepCopy(original);
            width = original.Width; height = original.Height;
            bufferb = new int[width, height];
            bufferr = new int[width, height];
            bufferg = new int[width, height];
            Thread t0 = new Thread(new ThreadStart(mappingForRed));
            Thread t1 = new Thread(new ThreadStart(mappingForBlue));
            Thread t2 = new Thread(new ThreadStart(mappingForGreen));
            t0.Start();//Thread started for Blue Color Channel
            t1.Start();//Thread started for Green Color Channel
            t2.Start();//Thread started for Red Color Channel
            while (rdone == false || bdone == false || gdone == false) ;
            Redraw rd = new Redraw();
            return rd.draw(sensitivity, bufferr, bufferg, bufferb, height, width);
        }
        private void mappingForRed()
        {
            //Function to determine Robert's Cross Edges Based on Red Color Channel
            LockBitmap org = new LockBitmap(original1);
            org.LockBits();
            for (int i = 0; i < width - 1; i++)
            {
                for (int j = 0; j < height - 1; j++)
                {
                    int gx = org.GetPixel(i, j).R - org.GetPixel(i + 1, j + 1).R;
                    int gy = org.GetPixel(i, j + 1).R - org.GetPixel(i + 1, j).R;
                    int d = int.Parse("" + Math.Round(Math.Sqrt((gx * gx) + (gy * gy))));
                    bufferr[i, j] = d;
                }
            }
            org.UnlockBits();
            rdone = true;
        }
        private void mappingForBlue()
        {
            //Function to determine Robert's Cross Edges Based on Blue Color Channel
            LockBitmap org = new LockBitmap(original2);
            org.LockBits();
            for (int i = 0; i < width - 1; i++)
            {
                for (int j = 0; j < height - 1; j++)
                {
                    int gx = Math.Abs(org.GetPixel(i, j).B) - Math.Abs(org.GetPixel(i + 1, j + 1).B);
                    int gy = Math.Abs(org.GetPixel(i, j + 1).B) - Math.Abs(org.GetPixel(i + 1, j).B);
                    int d = int.Parse("" + Math.Round(Math.Sqrt((gx * gx) + (gy * gy))));
                    bufferb[i, j] = d;
                }
            }
            org.UnlockBits();
            bdone = true;
        }
        private void mappingForGreen()
        {
            //Function to determine Robert's Cross Edges Based on Green Color Channel
            LockBitmap org = new LockBitmap(original3);
            org.LockBits();
            for (int i = 0; i < width - 1; i++)
            {
                for (int j = 0; j < height - 1; j++)
                {
                    int gx = Math.Abs(org.GetPixel(i, j).G) - Math.Abs(org.GetPixel(i + 1, j + 1).G);
                    int gy = Math.Abs(org.GetPixel(i, j + 1).G) - Math.Abs(org.GetPixel(i + 1, j).G);
                    int d = int.Parse("" + Math.Round(Math.Sqrt((gx * gx) + (gy * gy))));
                    bufferg[i, j] = d;
                }
            }
            org.UnlockBits();
            gdone = true;
        }
    }
}
