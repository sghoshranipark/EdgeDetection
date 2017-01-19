using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Edge_Detection
{
    class Canny
    {
        int width, height;//Stores the Width and Height of the BITMAP Image Received
        Copy cp = new Copy();//Declares an object for Class Copy
        public int[,] redmap; public int[,] bluemap; public int[,] greenmap;//Stores the dot position in respect to Red, Green and Blue Colour Channel of the Image
        Bitmap original1, original2, original3;//Three Instances of Bitmap
        Boolean rdone = false, bdone = false, gdone = false;//Global Variable for Synchronization
        int upThreshold, lowerThreshold;
        public Bitmap convert(Bitmap originalImage, int upperlimit, int lowerlimit)
        {
            //The Primary Function accessed by other classes to convert and return the canny edge detected image to the caller
            Bitmap original = new Copy().DeepCopy(originalImage);
            GaussianSmoothening gs = new GaussianSmoothening();
            original=gs.smooth(original);
            upThreshold = upperlimit;
            lowerThreshold = lowerlimit;
            width = original.Width; height = original.Height;
            redmap = new int[width,height];
            bluemap = new int[width, height];
            greenmap = new int[width, height];
            original1 = cp.DeepCopy(original);
            original2 = cp.DeepCopy(original);
            original3 = cp.DeepCopy(original);
            Thread t0, t1, t2;
            t0 = new Thread(new ThreadStart(mappingforBlue));
            t1 = new Thread(new ThreadStart(mappingforGreen));
            t2 = new Thread(new ThreadStart(mappingforRed));
            t0.Start();//Thread started for Blue Color Channel
            t1.Start();//Thread started for Green Color Channel
            t2.Start();//Thread started for Red Color Channel
            while (rdone == false||bdone == false || gdone == false) ; //For Thread Synchronization
            Redraw rd = new Redraw();
            return rd.draw(1, redmap, greenmap, bluemap, height, width);
        }
        private void postprocess(int i, int j, int[,] buffer)
        {
            //A recursive function to define continuity of weak edges
            buffer[i, j] = 225;
            if (i <= width - 1 && j <= height - 1 && buffer[i + 1, j + 1] == 175)
                postprocess(i + 1, j + 1, buffer);
            if (i <= width - 1 && j >= 1 && buffer[i + 1, j - 1] == 175)
                postprocess(i + 1, j - 1, buffer);
            if (i >= width - 1 && j >= 1 && buffer[i - 1, j - 1] == 175)
                postprocess(i - 1, j - 1, buffer);
            if (i >= 1 && j <= height - 1 && buffer[i - 1, j + 1] == 175)
                postprocess(i - 1, j + 1, buffer);
            if (i <= width - 1 && buffer[i + 1, j] == 175)
                postprocess(i + 1, j, buffer);
            if (i >= 1 && buffer[i - 1, j] == 175)
                postprocess(i - 1, j, buffer);
            if (j <= height - 1 && buffer[i, j + 1] == 175)
                postprocess(i, j + 1, buffer);
            if (j >= 1 && buffer[i, j - 1] == 175)
                postprocess(i, j - 1, buffer);
        }
        private void mappingforBlue()
        {
            //Function to determine Canny Edges Based on Blue Color Channel
            LockBitmap org = new LockBitmap(original1);
            org.LockBits();
            int[,] buffer1 = new int[width, height];
            int[,] buffer = new int[width, height];
            int[,] gradient = new int[width, height];
            for (int i = 1; i < width - 1; i++)
            {
                for (int j = 1; j < height - 1; j++)
                {
                    //Horizontal Scaling
                    int gx = (org.GetPixel(i - 1, j - 1).B * (-1)) + (org.GetPixel(i - 1, j).B * (-2)) + (org.GetPixel(i - 1, j + 1).B * (-1)) + (org.GetPixel(i + 1, j - 1).B) + (org.GetPixel(i + 1, j).B * 2) + (org.GetPixel(i + 1, j + 1).B);
                    //Vertical Scaling
                    int gy = (org.GetPixel(i - 1, j - 1).B * (-1)) + (org.GetPixel(i, j - 1).B * (-2)) + (org.GetPixel(i + 1, j - 1).B * (-1)) + (org.GetPixel(i - 1, j + 1).B) + (org.GetPixel(i, j + 1).B * 2) + (org.GetPixel(i + 1, j + 1).B);
                    //Throw Output
                    int d = int.Parse("" + Math.Round(Math.Sqrt((gx * gx) + (gy * gy))));
                    buffer1[i, j] = d;
                    if (gy != 0 && gx != 0)
                    {
                        Double radianvalue = Math.Atan(gy / gx);
                        if (radianvalue < 0)
                            gradient[i, j] = int.Parse("" + (90 - Math.Round(radianvalue * ((180 * 7) / 22))));
                        else
                            gradient[i, j] = int.Parse("" + Math.Round(radianvalue * ((180 * 7) / 22)));
                    }
                    else
                    {
                        int de = 0;
                        if (gy > gx)
                            de = gy;
                        else
                            de = gx;
                        gradient[i, j] = int.Parse("" + Math.Round(Math.Atan(de) * ((180 * 7) / 22)));
                    }
                }
            }//Sobel Edge Gradient Value Complete (Step 2)
            for (int i = 1; i < width - 1; i++)
            {
                for (int j = 1; j < height - 1; j++)
                {
                    if ((gradient[i, j] < 22 && gradient[i, j] >= 337) || (gradient[i, j] >= 158 && gradient[i, j] < 203))
                    {
                        if (buffer1[i, j] > buffer1[i + 1, j] && buffer1[i, j] > buffer1[i - 1, j])
                            buffer[i, j] = buffer1[i, j];
                        else
                            buffer[i, j] = 0;
                    }
                    else if ((gradient[i, j] >= 68 && gradient[i, j] <= 113) || (gradient[i, j] >= 248 && gradient[i, j] < 293))
                    {
                        if (buffer1[i, j] > buffer1[i, j + 1] && buffer1[i, j] > buffer1[i, j - 1])
                            buffer[i, j] = buffer1[i, j];
                        else
                            buffer[i, j] = 0;
                    }
                    else if ((gradient[i, j] >= 23 && gradient[i, j] <= 67) || (gradient[i, j] >= 203 && gradient[i, j] < 248))//for 45
                    {
                        if (buffer1[i, j] > buffer1[i - 1, j + 1] && buffer1[i, j] > buffer1[i + 1, j - 1])
                            buffer[i, j] = buffer1[i, j];
                        else
                            buffer[i, j] = 0;
                    }
                    else
                    {
                        //For 135
                        if (buffer1[i, j] > buffer1[i - 1, j - 1] && buffer1[i, j] > buffer1[i + 1, j + 1])
                            buffer[i, j] = buffer1[i, j];
                        else
                            buffer[i, j] = 0;
                    }
                }
            }
            //Step 3 Complete for Supressing and thinning edges
            for (int i = 1; i < width - 1; i++)
            {
                for (int j = 1; j < height - 1; j++)
                {
                    if (buffer[i, j] <= lowerThreshold)
                        buffer[i, j] = 0;
                    else if (buffer[i, j] >= upThreshold)
                        buffer[i, j] = 225;
                    else if (buffer[i, j] != 0)
                        buffer[i, j] = 175;
                }
            }
            //Step 4 Bound Upper and Lower Threshold Complete
            for (int i = 1; i < width - 1; i++)
                for (int j = 1; j < height - 1; j++)
                    if (buffer[i, j] == 225)
                        postprocess(i, j, buffer);
            //Step 5 to recalculate issues done
            org.UnlockBits();
            bluemap = cp.DeepCopy(buffer);
            bdone = true;
        }
        private void mappingforGreen()
        {
            //Function to determine Canny Edges Based on Green Color Channel
            LockBitmap org = new LockBitmap(original2);
            org.LockBits();
            int[,] buffer1 = new int[width, height];
            int[,] buffer = new int[width, height];
            int[,] gradient = new int[width, height];
            for (int i = 1; i < width - 1; i++)
            {
                for (int j = 1; j < height - 1; j++)
                {
                    //Horizontal Scaling
                    int gx = (org.GetPixel(i - 1, j - 1).G * (-1)) + (org.GetPixel(i - 1, j).G * (-2)) + (org.GetPixel(i - 1, j + 1).G * (-1)) + (org.GetPixel(i + 1, j - 1).G) + (org.GetPixel(i + 1, j).G * 2) + (org.GetPixel(i + 1, j + 1).G);
                    //Vertical Scaling
                    int gy = (org.GetPixel(i - 1, j - 1).G * (-1)) + (org.GetPixel(i, j - 1).G * (-2)) + (org.GetPixel(i + 1, j - 1).G * (-1)) + (org.GetPixel(i - 1, j + 1).G) + (org.GetPixel(i, j + 1).G * 2) + (org.GetPixel(i + 1, j + 1).G);
                    //Throw Output
                    int d = int.Parse("" + Math.Round(Math.Sqrt((gx * gx) + (gy * gy))));
                    buffer1[i, j] = d;
                    if (gy != 0 && gx != 0)
                    {
                        Double radianvalue = Math.Atan(gy / gx);
                        if (radianvalue < 0)
                            gradient[i, j] = int.Parse("" + (90 - Math.Round(radianvalue * ((180 * 7) / 22))));
                        else
                            gradient[i, j] = int.Parse("" + Math.Round(radianvalue * ((180 * 7) / 22)));
                    }
                    else
                    {
                        int de = 0;
                        if (gy > gx)
                            de = gy;
                        else
                            de = gx;
                        gradient[i, j] = int.Parse("" + Math.Round(Math.Atan(de) * ((180 * 7) / 22)));
                    }
                }
            }//Sobel Edge Gradient Value Complete (Step 2)
            for (int i = 1; i < width - 1; i++)
            {
                for (int j = 1; j < height - 1; j++)
                {
                    if ((gradient[i, j] < 22 && gradient[i, j] >= 337) || (gradient[i, j] >= 158 && gradient[i, j] < 203))
                    {
                        if (buffer1[i, j] > buffer1[i + 1, j] && buffer1[i, j] > buffer1[i - 1, j])
                            buffer[i, j] = buffer1[i, j];
                        else
                            buffer[i, j] = 0;
                    }
                    else if ((gradient[i, j] >= 68 && gradient[i, j] <= 113) || (gradient[i, j] >= 248 && gradient[i, j] < 293))
                    {
                        if (buffer1[i, j] > buffer1[i, j + 1] && buffer1[i, j] > buffer1[i, j - 1])
                            buffer[i, j] = buffer1[i, j];
                        else
                            buffer[i, j] = 0;
                    }
                    else if ((gradient[i, j] >= 23 && gradient[i, j] <= 67) || (gradient[i, j] >= 203 && gradient[i, j] < 248))//for 45
                    {
                        if (buffer1[i, j] > buffer1[i - 1, j + 1] && buffer1[i, j] > buffer1[i + 1, j - 1])
                            buffer[i, j] = buffer1[i, j];
                        else
                            buffer[i, j] = 0;
                    }
                    else
                    {
                        //For 135
                        if (buffer1[i, j] > buffer1[i - 1, j - 1] && buffer1[i, j] > buffer1[i + 1, j + 1])
                            buffer[i, j] = buffer1[i, j];
                        else
                            buffer[i, j] = 0;
                    }
                }
            }
            //Step 3 Complete for Supressing and thinning edges
            for (int i = 1; i < width - 1; i++)
            {
                for (int j = 1; j < height - 1; j++)
                {
                    if (buffer[i, j] <= lowerThreshold)
                        buffer[i, j] = 0;
                    else if (buffer[i, j] >= upThreshold)
                        buffer[i, j] = 225;
                    else if (buffer[i, j] != 0)
                        buffer[i, j] = 175;
                }
            }
            //Step 4 Bound Upper and Lower Threshold Complete
            for (int i = 1; i < width - 1; i++)
            {
                for (int j = 1; j < height - 1; j++)
                {
                    if (buffer[i, j] == 225)
                        postprocess(i, j, buffer);
                }
            }
            //Step 5 to recalculate hysterisis done
            org.UnlockBits();
            greenmap = cp.DeepCopy(buffer);
            gdone = true;
        }
        private void mappingforRed()
        {
            //Function to determine Canny Edges Based on Red Color Channel
            LockBitmap org = new LockBitmap(original3);
            org.LockBits();
            int[,] buffer1 = new int[width, height];
            int[,] buffer = new int[width, height];
            int[,] gradient = new int[width, height];
            for (int i = 1; i < width - 1; i++)
            {
                for (int j = 1; j < height - 1; j++)
                {
                    //Horizontal Scaling
                    int gx = (org.GetPixel(i - 1, j - 1).R * (-1)) + (org.GetPixel(i - 1, j).R * (-2)) + (org.GetPixel(i - 1, j + 1).R * (-1)) + (org.GetPixel(i + 1, j - 1).R) + (org.GetPixel(i + 1, j).R * 2) + (org.GetPixel(i + 1, j + 1).R);
                    //Vertical Scaling
                    int gy = (org.GetPixel(i - 1, j - 1).R * (-1)) + (org.GetPixel(i, j - 1).R * (-2)) + (org.GetPixel(i + 1, j - 1).R * (-1)) + (org.GetPixel(i - 1, j + 1).R) + (org.GetPixel(i, j + 1).R * 2) + (org.GetPixel(i + 1, j + 1).R);
                    //Throw Output
                    int d = int.Parse("" + Math.Round(Math.Sqrt((gx * gx) + (gy * gy))));
                    buffer1[i, j] = d;
                    if (gy != 0 && gx != 0)
                    {
                        Double radianvalue = Math.Atan(gy / gx);
                        if (radianvalue < 0)
                            gradient[i, j] = int.Parse("" + (90 - Math.Round(radianvalue * ((180 * 7) / 22))));
                        else
                            gradient[i, j] = int.Parse("" + Math.Round(radianvalue * ((180 * 7) / 22)));
                    }
                    else
                    {
                        int de = 0;
                        if (gy > gx)
                            de = gy;
                        else
                            de = gx;
                        gradient[i, j] = int.Parse("" + Math.Round(Math.Atan(de) * ((180 * 7) / 22)));
                    }
                }
            }//Sobel Edge Gradient Value Complete (Step 2)
            for (int i = 1; i < width - 1; i++)
            {
                for (int j = 1; j < height - 1; j++)
                {
                    if ((gradient[i, j] <= 22 && gradient[i,j]>=337)|| (gradient[i, j] >= 158 && gradient[i, j] < 203))
                    {
                        if (buffer1[i, j] > buffer1[i + 1, j] && buffer1[i, j] > buffer1[i - 1, j])
                            buffer[i, j] = buffer1[i, j];
                        else
                            buffer[i, j] = 0;
                    }
                    else if ((gradient[i, j] >= 68 && gradient[i, j] <= 113)|| (gradient[i, j] >= 248 && gradient[i, j] < 293))
                    {
                        if (buffer1[i, j] > buffer1[i, j + 1] && buffer1[i, j] > buffer1[i, j - 1])
                            buffer[i, j] = buffer1[i, j];
                        else
                            buffer[i, j] = 0;
                    }
                    else if ((gradient[i, j] >= 23 && gradient[i, j] <= 67)|| (gradient[i, j] >= 203 && gradient[i, j] < 248))//for 45
                    {
                        if (buffer1[i, j] > buffer1[i - 1, j + 1] && buffer1[i, j] > buffer1[i + 1, j - 1])
                            buffer[i, j] = buffer1[i, j];
                        else
                            buffer[i, j] = 0;
                    }
                    else
                    {
                        //For 135
                        if (buffer1[i, j] > buffer1[i - 1, j - 1] && buffer1[i, j] > buffer1[i + 1, j + 1])
                            buffer[i, j] = buffer1[i, j];
                        else
                            buffer[i, j] = 0;
                    }
                }
            }
            //Step 3 Complete for Supressing and thinning edges
            for (int i = 1; i < width - 1; i++)
            {
                for (int j = 1; j < height - 1; j++)
                {
                    if (buffer[i, j] <= lowerThreshold)
                        buffer[i, j] = 0;
                    else if (buffer[i, j] >= upThreshold)
                        buffer[i, j] = 225;
                    else if (buffer[i, j] != 0)
                        buffer[i, j] = 175;
                }
            }
            //Step 4 Bound Upper and Lower Threshold Complete
            for (int i = 1; i < width - 1; i++)
            {
                for (int j = 1; j < height - 1; j++)
                {
                    if (buffer[i, j] == 225)
                        postprocess(i, j, buffer);
                }
            }
            //Step 5 to recalculate issues done
            org.UnlockBits();
            redmap = cp.DeepCopy(buffer);
            rdone = true;
        }
    }
}
