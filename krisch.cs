using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Edge_Detection
{
    class krisch
    {
        //This Class is built for creating Edge based image on Krisch Compass Mask Algoritihm
        Bitmap original1, original2, original3; // Three Instances of BITMAP Class
        int width, height;//Stores Height and Width of the Image received as input
        public int[,] bufferr; public int[,] bufferg; public int[,] bufferb;//Stores the dot position in respect to Red, Green and Blue Colour Channel of the Image
        Boolean rdone = false, gdone = false, bdone = false;
        public Bitmap convert(Bitmap original, int sensitivity)
        {
            //Function that Converts Image to Edge Based Krisch Images
            Copy cp = new Copy();
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
            while (rdone == false || bdone == false || gdone == false) ; //For Thread Synchronization
            Redraw rd = new Redraw();
            return rd.draw(sensitivity, bufferr, bufferg, bufferb, height, width);
        }
        private void mappingForRed()
        {
            //Function to determine Krisch Edges Based on Red Color Channel
            LockBitmap org = new LockBitmap(original1);
            org.LockBits();
            for (int i = 1; i < width - 1; i++)
            {
                for (int j = 1; j < height - 1; j++)
                {
                    int[] gVal = new int[9];
                    int ctr = 0;
                    for (int k = -1; k <= 1; k++)
                        for (int l = -1; l <= 1; l++)
                            gVal[ctr++] = org.GetPixel((i + l), (j + k)).R;
                    int max = 0;
                    int N = (5 * (gVal[0] + gVal[1] + gVal[2])) - (3 * (gVal[3] + gVal[5] + gVal[6] + gVal[7] + gVal[8]));
                    if (N > max)
                        max = N;
                    int NW = (5 * (gVal[0] + gVal[1] + gVal[3])) - (3 * (gVal[2] + gVal[5] + gVal[6] + gVal[7] + gVal[8]));
                    if (NW > max)
                        max = NW;
                    int W = (5 * (gVal[0] + gVal[3] + gVal[6])) - (3 * (gVal[1] + gVal[2] + gVal[5] + gVal[7] + gVal[8]));
                    if (W > max)
                        max = W;
                    int SW = (5 * (gVal[3] + gVal[6] + gVal[7])) - (3 * (gVal[0] + gVal[1] + gVal[2] + gVal[5] + gVal[8]));
                    if (SW > max)
                        max = SW;
                    int S = (5 * (gVal[6] + gVal[7] + gVal[8])) - (3 * (gVal[0] + gVal[1] + gVal[2] + gVal[3] + gVal[5]));
                    if (S > max)
                        max = S;
                    int SE = (5 * (gVal[5] + gVal[7] + gVal[8])) - (3 * (gVal[0] + gVal[1] + gVal[2] + gVal[3] + gVal[6]));
                    if (SE > max)
                        max = SE;
                    int E = (5 * (gVal[2] + gVal[5] + gVal[8])) - (3 * (gVal[0] + gVal[1] + gVal[3] + gVal[6] + gVal[7]));
                    if (E > max)
                        max = E;
                    int NE = (5 * (gVal[1] + gVal[2] + gVal[5])) - (3 * (gVal[0] + gVal[3] + gVal[6] + gVal[7] + gVal[8]));
                    if (NE > max)
                        max = NE;
                    bufferr[i, j] = max;
                }
            }
            org.UnlockBits();
            rdone = true;
        }
        private void mappingForBlue()
        {
            //Function to determine Krisch Edges Based on Blue Color Channel
            LockBitmap org = new LockBitmap(original2);
            org.LockBits();
            for (int i = 1; i < width - 1; i++)
            {
                for (int j = 1; j < height - 1; j++)
                {
                    int[] gVal = new int[9];
                    int ctr = 0;
                    for (int k = -1; k <= 1; k++)
                        for (int l = -1; l <= 1; l++)
                            gVal[ctr++] = org.GetPixel((i + l), (j + k)).B;
                    int max = 0;
                    int N = (5 * (gVal[0] + gVal[1] + gVal[2])) - (3 * (gVal[3] + gVal[5] + gVal[6] + gVal[7] + gVal[8]));
                    if (N > max)
                        max = N;
                    int NW = (5 * (gVal[0] + gVal[1] + gVal[3])) - (3 * (gVal[2] + gVal[5] + gVal[6] + gVal[7] + gVal[8]));
                    if (NW > max)
                        max = NW;
                    int W = (5 * (gVal[0] + gVal[3] + gVal[6])) - (3 * (gVal[1] + gVal[2] + gVal[5] + gVal[7] + gVal[8]));
                    if (W > max)
                        max = W;
                    int SW = (5 * (gVal[3] + gVal[6] + gVal[7])) - (3 * (gVal[0] + gVal[1] + gVal[2] + gVal[5] + gVal[8]));
                    if (SW > max)
                        max = SW;
                    int S = (5 * (gVal[6] + gVal[7] + gVal[8])) - (3 * (gVal[0] + gVal[1] + gVal[2] + gVal[3] + gVal[5]));
                    if (S > max)
                        max = S;
                    int SE = (5 * (gVal[5] + gVal[7] + gVal[8])) - (3 * (gVal[0] + gVal[1] + gVal[2] + gVal[3] + gVal[6]));
                    if (SE > max)
                        max = SE;
                    int E = (5 * (gVal[2] + gVal[5] + gVal[8])) - (3 * (gVal[0] + gVal[1] + gVal[3] + gVal[6] + gVal[7]));
                    if (E > max)
                        max = E;
                    int NE = (5 * (gVal[1] + gVal[2] + gVal[5])) - (3 * (gVal[0] + gVal[3] + gVal[6] + gVal[7] + gVal[8]));
                    if (NE > max)
                        max = NE;
                    bufferb[i, j] = max;
                }
            }
            org.UnlockBits();
            bdone = true;
        }
        private void mappingForGreen()
        {
            //Function to determine Krisch Edges Based on Green Color Channel
            LockBitmap org = new LockBitmap(original3);
            org.LockBits();
            for (int i = 1; i < width - 1; i++)
            {
                for (int j = 1; j < height - 1; j++)
                {
                    int[] gVal = new int[9];
                    int ctr = 0;
                    for (int k = -1; k <= 1; k++)
                        for (int l = -1; l <= 1; l++)
                            gVal[ctr++] = org.GetPixel((i + l), (j + k)).G;
                    int max = 0;
                    int N = (5 * (gVal[0] + gVal[1] + gVal[2])) - (3 * (gVal[3] + gVal[5] + gVal[6] + gVal[7] + gVal[8]));
                    if (N > max)
                        max = N;
                    int NW = (5 * (gVal[0] + gVal[1] + gVal[3])) - (3 * (gVal[2] + gVal[5] + gVal[6] + gVal[7] + gVal[8]));
                    if (NW > max)
                        max = NW;
                    int W = (5 * (gVal[0] + gVal[3] + gVal[6])) - (3 * (gVal[1] + gVal[2] + gVal[5] + gVal[7] + gVal[8]));
                    if (W > max)
                        max = W;
                    int SW = (5 * (gVal[3] + gVal[6] + gVal[7])) - (3 * (gVal[0] + gVal[1] + gVal[2] + gVal[5] + gVal[8]));
                    if (SW > max)
                        max = SW;
                    int S = (5 * (gVal[6] + gVal[7] + gVal[8])) - (3 * (gVal[0] + gVal[1] + gVal[2] + gVal[3] + gVal[5]));
                    if (S > max)
                        max = S;
                    int SE = (5 * (gVal[5] + gVal[7] + gVal[8])) - (3 * (gVal[0] + gVal[1] + gVal[2] + gVal[3] + gVal[6]));
                    if (SE > max)
                        max = SE;
                    int E = (5 * (gVal[2] + gVal[5] + gVal[8])) - (3 * (gVal[0] + gVal[1] + gVal[3] + gVal[6] + gVal[7]));
                    if (E > max)
                        max = E;
                    int NE = (5 * (gVal[1] + gVal[2] + gVal[5])) - (3 * (gVal[0] + gVal[3] + gVal[6] + gVal[7] + gVal[8]));
                    if (NE > max)
                        max = NE;
                    bufferg[i, j] = max;
                }
            }
            org.UnlockBits();
            gdone = true;
        }        
    }
}
