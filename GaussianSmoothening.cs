using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edge_Detection
{
    class GaussianSmoothening
    {
        //Class meant for Gaussian Smoothening of an image
        int size; double[,] convulationmatrix;
        void mode1()
        {
            size = 3;
            convulationmatrix = new double[size, size];
            convulationmatrix[0, 0] = 0.01221045877937173;
            convulationmatrix[0, 1] = 0.09022364353601825;
            convulationmatrix[0, 2] = 0.01221045877937173;
            convulationmatrix[1, 0] = 0.09022364353601825;
            convulationmatrix[1, 1] = 0.6666666666666666;
            convulationmatrix[1, 2] = 0.09022364353601825;
            convulationmatrix[2, 0] = 0.01221045877937173;
            convulationmatrix[2, 1] = 0.09022364353601825;
            convulationmatrix[2, 2] = 0.01221045877937173;
        }
        void mode2()
        {
            size = 5;
            convulationmatrix = new double[size, size];
            convulationmatrix[0, 0] = 7.502425725282328E-8;
            convulationmatrix[0, 1] = 3.0266823431582297E-5;
            convulationmatrix[0, 2] = 2.236429554041043E-4;
            convulationmatrix[0, 3] = 3.0266823431582297E-5;
            convulationmatrix[0, 4] = 7.502425725282328E-8;
            convulationmatrix[1, 0] = 3.0266823431582297E-5;
            convulationmatrix[1, 1] = 0.01221045877937173;
            convulationmatrix[1, 2] = 0.09022364353601825;
            convulationmatrix[1, 3] = 0.01221045877937173;
            convulationmatrix[1, 4] = 3.0266823431582297E-5;
            convulationmatrix[2, 0] = 2.236429554041043E-4;
            convulationmatrix[2, 1] = 0.09022364353601825;
            convulationmatrix[2, 2] = 0.6666666666666666;
            convulationmatrix[2, 3] = 0.09022364353601825;
            convulationmatrix[2, 4] = 2.236429554041043E-4;
            convulationmatrix[3, 0] = 3.0266823431582297E-5;
            convulationmatrix[3, 1] = 0.01221045877937173;
            convulationmatrix[3, 2] = 0.09022364353601825;
            convulationmatrix[3, 3] = 0.01221045877937173;
            convulationmatrix[3, 4] = 3.0266823431582297E-5;
            convulationmatrix[4, 0] = 7.502425725282328E-8;
            convulationmatrix[4, 1] = 3.0266823431582297E-5;
            convulationmatrix[4, 2] = 2.236429554041043E-4;
            convulationmatrix[4, 3] = 3.0266823431582297E-5;
            convulationmatrix[4, 4] = 7.502425725282328E-8;
        }
        void mode3()
            {
                size = 7;
                convulationmatrix = new double[size, size];
                convulationmatrix[0, 0] = 1.546385999549167E-16;
                convulationmatrix[0, 1] = 3.406118921035215E-12;
                convulationmatrix[0, 2] = 1.374120900957869E-9;
                convulationmatrix[0, 3] = 1.0153442764399889E-8;
                convulationmatrix[0, 4] = 1.374120900957869E-9;
                convulationmatrix[0, 5] = 3.406118921035215E-12;
                convulationmatrix[0, 6] = 1.546385999549167E-16;
                convulationmatrix[1, 0] = 3.406118921035215E-12;
                convulationmatrix[1, 1] = 7.502425725282328E-8;
                convulationmatrix[1, 2] = 3.0266823431582297E-5;
                convulationmatrix[1, 3] = 2.236429554041043E-4;
                convulationmatrix[1, 4] = 3.0266823431582297E-5;
                convulationmatrix[1, 5] = 7.502425725282328E-8;
                convulationmatrix[1, 6] = 3.406118921035215E-12;
                convulationmatrix[2, 0] = 1.374120900957869E-9;
                convulationmatrix[2, 1] = 3.0266823431582297E-5;
                convulationmatrix[2, 2] = 0.01221045877937173;
                convulationmatrix[2, 3] = 0.09022364353601825;
                convulationmatrix[2, 4] = 0.01221045877937173;
                convulationmatrix[2, 5] = 3.0266823431582297E-5;
                convulationmatrix[2, 6] = 1.374120900957869E-9;
                convulationmatrix[3, 0] = 1.0153442764399889E-8;
                convulationmatrix[3, 1] = 2.236429554041043E-4;
                convulationmatrix[3, 2] = 0.09022364353601825;
                convulationmatrix[3, 3] = 0.6666666666666666;
                convulationmatrix[3, 4] = 0.09022364353601825;
                convulationmatrix[3, 5] = 2.236429554041043E-4;
                convulationmatrix[3, 6] = 1.0153442764399889E-8;
                convulationmatrix[4, 0] = 1.374120900957869E-9;
                convulationmatrix[4, 1] = 3.0266823431582297E-5;
                convulationmatrix[4, 2] = 0.01221045877937173;
                convulationmatrix[4, 3] = 0.09022364353601825;
                convulationmatrix[4, 4] = 0.01221045877937173;
                convulationmatrix[4, 5] = 3.0266823431582297E-5;
                convulationmatrix[4, 6] = 1.374120900957869E-9;
                convulationmatrix[5, 0] = 3.406118921035215E-12;
                convulationmatrix[5, 1] = 7.502425725282328E-8;
                convulationmatrix[5, 2] = 3.0266823431582297E-5;
                convulationmatrix[5, 3] = 2.236429554041043E-4;
                convulationmatrix[5, 4] = 3.0266823431582297E-5;
                convulationmatrix[5, 5] = 7.502425725282328E-8;
                convulationmatrix[5, 6] = 3.406118921035215E-12;
                convulationmatrix[6, 0] = 1.546385999549167E-16;
                convulationmatrix[6, 1] = 3.406118921035215E-12;
                convulationmatrix[6, 2] = 1.374120900957869E-9;
                convulationmatrix[6, 3] = 1.0153442764399889E-8;
                convulationmatrix[6, 4] = 1.374120900957869E-9;
                convulationmatrix[6, 5] = 3.406118921035215E-12;
                convulationmatrix[6, 6] = 1.546385999549167E-16;

            }

        public GaussianSmoothening()
        {
            Boolean manual = false;
            if (Properties.Settings.Default.GaussianWeight == 0.5)
            {
                if (Properties.Settings.Default.GaussianMatrix == 3)
                    mode1();
                else if (Properties.Settings.Default.GaussianMatrix == 5)
                    mode2();
                else if (Properties.Settings.Default.GaussianMatrix == 7)
                    mode3();
                else
                    manual = true;
            }
            else manual = true;
            if(manual==true)
            {
                size = Properties.Settings.Default.GaussianMatrix;
                double weight = Properties.Settings.Default.GaussianWeight;
                convulationmatrix = new double[size, size];
                double pie = 22 / 7, e = 2.71828;
                int mid = int.Parse("" + Math.Round(Math.Floor(double.Parse("" + size / 2))));
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        int x = Math.Abs(mid - i);
                        int y = Math.Abs(mid - j);
                        double res = (1 / (2 * pie * Math.Pow(weight, 2))) * Math.Pow(e, (((Math.Pow(x, 2) + Math.Pow(y, 2)) / (2 * Math.Pow(weight, 2))) * -1));
                        convulationmatrix[i, j] = res;
                    }
                }
            }
        }
        public Bitmap smooth(Bitmap original)
        {
            //Function to apply Gaussian smoothening over the image received
            int mid = int.Parse("" + Math.Round(Math.Floor(double.Parse("" + size / 2))));
            int width = original.Width, height = original.Height;
            Bitmap result = new Bitmap(width, height);
            LockBitmap org = new LockBitmap(original);
            LockBitmap res = new LockBitmap(result);
            org.LockBits();
            res.LockBits();
            for (int i = mid; i < width - mid; i++)
            {
                for (int j = mid; j < height - mid; j++)
                {
                    double dg = 0.0, dr = 0.0, db = 0.0;
                    for (int x = 0; x < size; x++)
                        for (int y = 0; y < size; y++)
                        {
                            dr += convulationmatrix[x, y] * org.GetPixel(i - (mid - x), j - (mid - y)).R;
                            dg += convulationmatrix[x, y] * org.GetPixel(i - (mid - x), j - (mid - y)).G;
                            db += convulationmatrix[x, y] * org.GetPixel(i - (mid - x), j - (mid - y)).B;
                        }
                    int dir = int.Parse("" + Math.Round(dr));
                    int dib = int.Parse("" + Math.Round(db));
                    int dig = int.Parse("" + Math.Round(dg));
                    if (dir > 255)
                        dir = 255;
                    if (dib > 255)
                        dib = 255;
                    if (dig > 255)
                        dig = 255;
                    res.SetPixel(i, j, Color.FromArgb(dir, dig, dib));
                }
            }
            org.UnlockBits();
            res.UnlockBits();
            return result;
        }
    }
}
