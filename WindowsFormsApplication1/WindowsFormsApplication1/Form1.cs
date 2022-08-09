using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    
    using ClassLibrary1;
    using System.Windows.Forms.DataVisualization.Charting;

    public partial class Form1 : Form
    {
        
        double R = 18.3e-4;
        double Wd = 15e-4;
        double Wh = 20e-4;
        double Wc = 10e-4;
        int gr;
  
        int mf = 3;
        int mc = 1, md = 2, mh = 2;
        double lambda = 10;
        double eta = 1.2;
        double gmax = 400;

        double gmin, L, W, Lold=0, Wold=0, time0=0, time=0, path=0, path0= -0.5e-3;





        int n, num=0, num0=0;

        double h;
        double[] xj, yj1, yj2;
        double fi, xf, yf, dfi;
        double x0;
        double p=10, tau, G=0;
        int n2, nf, N, i1, i2, ihf;
        double zn, zm;
        int a, kr;

        double[] k, b, S;

        double[] x, y, x_new, y_new, Wi, gammai, Cr, Crf, V, finorm, fig, Jn, Jx, Jy, T;
        double Sv, Sv_new, Sv0, dy;
        double dSv;

        double CsCl, d, dCdT;
        double mu2;
        double mu3 ;
        double mu4;
        double dCdP; 

        double Hmin = 100;
        double[] xc;
        bool stop;

        DrawZXY DZXY = new DrawZXY();





        int m1; 
        int m2;
        double[] hxj;
        double[] hyj1;

        private void button5_Click(object sender, EventArgs e)
        {
            stop = true;
        }

        double[] hyj2;

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

 

        double t = 5;    
        double Snf2;

        

        int[] jmax;

       

        double[] yc1, yc2;

        
        private void chart7_Click(object sender, EventArgs e)
        {

        }

        double[,] C, C_new;
        double[] hxc;
        double[] hyc1;
        double[] hyc2;

        double Jxr = 0;
        double Jxl = 0;
        double Jyd = 0;
        double Jyu = 0;
        double tmax = 1e-1;  ////////////
        int kmax;
        int m;
        private void button3_Click(object sender, EventArgs e)
        {
            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();
            chart1.Series[2].Points.Clear();
            chart1.Series[3].Points.Clear();
            chart1.Series[4].Points.Clear();
            chart2.Series[0].Points.Clear();
            chart2.Series[1].Points.Clear();
            chart2.Series[2].Points.Clear();
            chart3.Series[0].Points.Clear();
            chart3.Series[1].Points.Clear();
            chart2.Series[2].Points.Clear();
        }

        public Form1()
        {
            InitializeComponent();
        }
        public double gamma(double fi, double M)
        {
            gmin = gmax / eta;
            double gamma;
            gamma = gmin + (gmax - gmin) * Math.Abs(Math.Sin(M/2 * fi));
            return gamma;
        }
       
        public double D(double fi)
        {

            double d = 0;
            if (fi >= 0 && fi < Math.PI / 2)
                d = 2 * (gmax - gmin) * Math.Cos(2 * fi);
            if (fi >= Math.PI / 2 && fi <= Math.PI)
                d = -2 * (gmax - gmin) * Math.Cos(2 * fi);
            return d;
        }

        public double X(double fi)
        {
            fi += Math.PI / 2;
            double X;
            X = 1 / lambda * (gamma(fi, Convert.ToDouble(comboBox1.Text)) * Math.Sin(fi) + D(fi - Math.PI / 2) * Math.Cos(fi));
            return X;
        }

        public double Y(double fi)
        {
            fi += Math.PI / 2;
            double Y;
            Y = -1 / lambda * (gamma(fi, Convert.ToDouble(comboBox1.Text)) * Math.Cos(fi) - D(fi - Math.PI / 2) * Math.Sin(fi));
            return Y;
        }
        public double X2(double fi)
        {

            double X;
            X = R * Math.Cos(fi);
            return X;
        }

        public double Y2(double fi)
        {

            double Y;
            Y = R * Math.Sin(fi);
            return Y;
        }

        void clearC()
        {

            chart2.Series[1].Points.Clear();

        }
        
        void clearCr()
        {
            chart2.Series[0].Points.Clear();

            chart2.Series[2].Points.Clear();
        }
        
        
        // рисуем фактические концентрации (синие -серия 1) вдоль ИКСА
        void drawC()
        {
            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= jmax[i]; j++)
                {
                    if (i <= n2)
                        chart2.Series[1].Points.AddXY((xc[i]-x[1])*1e4, C[i, j]);
                    else
                        chart2.Series[1].Points.AddXY((xc[i]-x[1])*1e4, C[i, j]);

                }
            }
           
        }


        void DrawCXYdll(double fi_grad, double[] Xc, double[] Yc1, double[] Yc2, double[,] T)
        {
            int n = Xc.GetLength(0) - 1;

            int m = Yc1.GetLength(0) - 1;

            int mm = 20;

            double xpr, ypr;
            double[,] Xpr = new double[n + 1, mm + 1];
            double[,] Ypr = new double[n + 1, mm + 1];
            double[,] CC = new double[n + 1, mm + 1];
            double[] YYc = new double[mm + 1];
            double[] Ycmax = new double[n + 1];


            for (int i = 1; i <= n; i++)
                if (i <= n2) Ycmax[i] = Yc1[jmax[i]];
                else Ycmax[i] = Yc2[jmax[i]];



            chart2.Series[0].Points.Clear();
            chart2.Series[1].Points.Clear();
            chart2.Series[2].Points.Clear();
            chart2.Series[3].Points.Clear();
            chart2.Series[4].Points.Clear();
            chart2.Series[5].Points.Clear();
            chart2.Series[6].Points.Clear();
            chart2.Series[7].Points.Clear();
            chart2.Series[8].Points.Clear();
            chart2.Series[9].Points.Clear();
            chart2.Series[10].Points.Clear();




            //контур
            double kc = 100000;
            DZXY.GetXpYp(fi_grad, 0, 0, kc * C[1, 1], out xpr, out ypr); chart2.Series[0].Points.AddXY(xpr, ypr);
            for (int i = 1; i <= n; i++)
            {
                DZXY.GetXpYp(fi_grad, xc[i] - xc[1], Ycmax[i], kc * C[i, jmax[i]], out xpr, out ypr);
                chart2.Series[0].Points.AddXY(xpr, ypr);
            }
            DZXY.GetXpYp(fi_grad, xc[n] - xc[1], 0, kc * C[n, 1], out xpr, out ypr); chart2.Series[0].Points.AddXY(xpr, ypr);


            DZXY.GetXpYp(fi_grad, xc[n] - xc[1], 0, kc * C[n, 1], out xpr, out ypr); chart2.Series[0].Points.AddXY(xpr, ypr);
            for (int i = n; i >= 1; i--)
            {
                DZXY.GetXpYp(fi_grad, xc[i] - xc[1], -Ycmax[i], kc * C[i, jmax[i]], out xpr, out ypr);
                chart2.Series[0].Points.AddXY(xpr, ypr);
            }
            DZXY.GetXpYp(fi_grad, 0, 0, kc * C[1, 1], out xpr, out ypr); chart2.Series[0].Points.AddXY(xpr, ypr);




            //горизонтали часть 1 
            for (int j = 1; j <= m1; j++)
                if (j % 2 != 0)
                {
                    for (int i = 1; i <= n2; i++)
                    {
                        DZXY.GetXpYp(fi_grad, xc[i] - xc[1], yc1[j], kc * C[i, j], out xpr, out ypr);
                        if (j <= jmax[i]) chart2.Series[1].Points.AddXY(xpr, ypr);
                    }
                }
                else
                {
                    for (int i = n2; i >= 1; i--)
                    {
                        DZXY.GetXpYp(fi_grad, xc[i] - xc[1], yc1[j], kc * C[i, j], out xpr, out ypr);
                        if (j <= jmax[i]) chart2.Series[1].Points.AddXY(xpr, ypr);
                    }

                }

            for (int j = 1; j <= m1; j++)
                if (j % 2 != 0)
                {
                    for (int i = 1; i <= n2; i++)
                    {
                        DZXY.GetXpYp(fi_grad, xc[i] - xc[1], -yc1[j], kc * C[i, j], out xpr, out ypr);
                        if (j <= jmax[i]) chart2.Series[9].Points.AddXY(xpr, ypr);
                    }
                }
                else
                {
                    for (int i = n2; i >= 1; i--)
                    {
                        DZXY.GetXpYp(fi_grad, xc[i] - xc[1], -yc1[j], kc * C[i, j], out xpr, out ypr);
                        if (j <= jmax[i]) chart2.Series[9].Points.AddXY(xpr, ypr);
                    }

                }





            //горизонтали часть 2
            for (int j = 1; j <= m2; j++)
                if (j % 2 != 0)
                {
                    for (int i = n2 + 1; i <= n; i++)
                    {
                        DZXY.GetXpYp(fi_grad, xc[i] - xc[1], yc2[j], kc * C[i, j], out xpr, out ypr);
                        if (j <= jmax[i]) chart2.Series[2].Points.AddXY(xpr, ypr);
                    }
                }
                else
                {
                    for (int i = n; i >= n2 + 1; i--)
                    {
                        DZXY.GetXpYp(fi_grad, xc[i] - xc[1], yc2[j], kc * C[i, j], out xpr, out ypr);
                        if (j <= jmax[i]) chart2.Series[2].Points.AddXY(xpr, ypr);
                    }
                }

            for (int j = 1; j <= m2; j++)
                if (j % 2 != 0)
                {
                    for (int i = n2 + 1; i <= n; i++)
                    {
                        DZXY.GetXpYp(fi_grad, xc[i] - xc[1], -yc2[j], kc * C[i, j], out xpr, out ypr);
                        if (j <= jmax[i]) chart2.Series[10].Points.AddXY(xpr, ypr);
                    }
                }
                else
                {
                    for (int i = n; i >= n2 + 1; i--)
                    {
                        DZXY.GetXpYp(fi_grad, xc[i] - xc[1], -yc2[j], kc * C[i, j], out xpr, out ypr);
                        if (j <= jmax[i]) chart2.Series[10].Points.AddXY(xpr, ypr);
                    }
                }



            //вертикали часть 1 
            for (int i = 1; i <= n2; i++)
                if (i % 2 != 0)
                {
                    for (int j = -jmax[i]; j <= jmax[i]; j++)
                        if (j != 0)
                        {
                            int jj = Math.Abs(j);
                            DZXY.GetXpYp(fi_grad, xc[i] - xc[1], Math.Sign(j) * yc1[jj], kc * C[i, jj], out xpr, out ypr);
                            chart2.Series[3].Points.AddXY(xpr, ypr);
                        }
                }
                else
                {
                    for (int j = jmax[i]; j >= -jmax[i]; j--)
                        if (j != 0)
                        {
                            int jj = Math.Abs(j);
                            DZXY.GetXpYp(fi_grad, xc[i] - xc[1], Math.Sign(j) * yc1[jj], kc * C[i, jj], out xpr, out ypr);
                            chart2.Series[3].Points.AddXY(xpr, ypr);
                        }

                }

            //вертикали часть 2 
            for (int i = n2 + 1; i <= n; i++)
                if (i % 2 != 0)
                {
                    for (int j = -jmax[i]; j <= jmax[i]; j++)
                        if (j != 0)
                        {
                            int jj = Math.Abs(j);
                            DZXY.GetXpYp(fi_grad, xc[i] - xc[1], Math.Sign(j) * yc2[jj], kc * C[i, jj], out xpr, out ypr);
                            chart2.Series[4].Points.AddXY(xpr, ypr);
                        }
                }
                else
                {
                    for (int j = jmax[i]; j >= -jmax[i]; j--)
                        if (j != 0)
                        {
                            int jj = Math.Abs(j);
                            DZXY.GetXpYp(fi_grad, xc[i] - xc[1], Math.Sign(j) * yc2[jj], kc * C[i, jj], out xpr, out ypr);
                            chart2.Series[4].Points.AddXY(xpr, ypr);
                        }

                }


            //ликвидусные и равновесные по контуру
            DZXY.GetXpYp(fi_grad, 0, 0, 0, out xpr, out ypr); chart2.Series[6].Points.AddXY(xpr, ypr);
            for (int i = 1; i <= n; i++)
            {
                DZXY.GetXpYp(fi_grad, xc[i] - xc[1], Ycmax[i], kc * Cr[i], out xpr, out ypr);
                chart2.Series[5].Points.AddXY(xpr, ypr);

                DZXY.GetXpYp(fi_grad, xc[i] - xc[1], Ycmax[i], kc * (xc[i] - xc[1]) * G * dCdT, out xpr, out ypr);
                chart2.Series[6].Points.AddXY(xpr, ypr);
            }
            for (int i = n; i >= 1; i--)
            {
                DZXY.GetXpYp(fi_grad, xc[i] - xc[1], -Ycmax[i], kc * Cr[i], out xpr, out ypr);
                chart2.Series[5].Points.AddXY(xpr, ypr);

                DZXY.GetXpYp(fi_grad, xc[i] - xc[1], -Ycmax[i], kc * (xc[i] - xc[1]) * G * dCdT, out xpr, out ypr);
                chart2.Series[6].Points.AddXY(xpr, ypr);
            }
            DZXY.GetXpYp(fi_grad, 0, 0, 0, out xpr, out ypr); chart2.Series[6].Points.AddXY(xpr, ypr);



            //равновесные на тыльном фронте
            DZXY.GetXpYp(fi_grad, 0, Ycmax[1], kc * Cr[0], out xpr, out ypr); chart2.Series[7].Points.AddXY(xpr, ypr);
            DZXY.GetXpYp(fi_grad, 0, -Ycmax[1], kc * Cr[0], out xpr, out ypr); chart2.Series[7].Points.AddXY(xpr, ypr);


            //равновесные на переднем фронте
            DZXY.GetXpYp(fi_grad, xc[n] - xc[1], Ycmax[n], kc * Cr[n+1], out xpr, out ypr); chart2.Series[8].Points.AddXY(xpr, ypr);
            DZXY.GetXpYp(fi_grad, xc[n] - xc[1], -Ycmax[n], kc * Cr[n+1], out xpr, out ypr); chart2.Series[8].Points.AddXY(xpr, ypr);


        }
        
        // рисуем равновесные концентрации  на кривой границе (темно-красные -серия 0) и  на плоских гранях (розовые -серия 2) - вдоль ИКСА
        void drawCr()
        {
                
             for (int i = 1; i <= n; i++)
                        if (i>=i1+1 && i<=i2) chart2.Series[2].Points.AddXY((xc[i]-x[1])*1e4, Cr[i]);
                        else chart2.Series[0].Points.AddXY((xc[i]-x[1])*1e4, Cr[i]);
             chart2.Series[2].Points.AddXY(0, Cr[0]);
             chart2.Series[2].Points.AddXY((x[nf-1]-x[1])*1e4, Cr[n+1]);

        }

        public double Vel(int a, double dC)   //скорость на границе х=l 
        {

            double vl = 0;

            if (a == 1)
            {
                if (dC > 0)
                {
                    vl = mu2 * dC * dC;
                }
                else
                {
                    vl = -mu2 * dC * dC;
                }
            }
            if (a == 2)
            {
                if (dC > 0)
                {
                    vl = mu3 * Math.Exp(-mu4 / Math.Abs(dC));
                }
                else
                {
                    vl = -mu3 * Math.Exp(-mu4 / Math.Abs(dC));
                }
            }
            return vl;
        }

        public double dC (int a, double vel)   //скорость на границе х=l 
        {

            double dC = 0;

            if (a == 1)
            {
                if (vel> 0)
                {
                    dC = Math.Sqrt(vel / mu2);
                }
                else
                {
                    dC = -Math.Sqrt(-vel / mu2);
                }
            }
            if (a == 2)
            {
                if (vel > 0)
                {
                    dC = mu4 / Math.Log(mu3 / vel);
                }
                if (vel<0)
                {
                    dC = -mu4 / Math.Log(-mu3 / vel);
                }
                if (vel == 0) dC = 0;
            }
            return dC;
        }


        // функция С2 добавлена для сшивания
        public double C2(int j)
        {
            double c2 = -1000;
            if (yc1[j] <= yc2[1]) c2 = C[n2 + 1, 1];
            if (yc1[j] > yc2[m2]) c2 = C[n2 + 1, m2];
            for (int k = 1; k < m2; k++)
            {
                if (yc1[j] > yc2[k] && yc1[j] <= yc2[k + 1])
                    c2 = C[n2 + 1, k] + (C[n2 + 1, k + 1] - C[n2 + 1, k]) / (yc2[k + 1] - yc2[k]) * (yc1[j] - yc2[k]);

            }
           
            return c2;
        }

        // функция С1 добавлена для сшивания
        public double C1(int j)
        {
            double c1 = -1000;
            if (yc2[j] <= yc1[1]) c1 = C[n2, 1];
            if (yc2[j] > yc1[m1]) c1 = C[n2, m1];
            for (int k = 1; k < m1; k++)
            {
                if (yc2[j] > yc1[k] && yc2[j] <= yc1[k + 1])
                    c1 = C[n2, k] + (C[n2, k + 1] - C[n2, k]) / (yc1[k + 1] - yc1[k]) * (yc2[j] - yc1[k]);

            }
            return c1;
        }


        public double fC(int i, double y)
        {
            double c = -1000;

            if (i <= n2)
            {
                if (y <= yc1[1]) c = C[i, 1];
                if (y > yc1[jmax[i]]) c = C[i, jmax[i]];
                for (int k = 1; k < jmax[i]; k++)
                {
                    if (y > yc1[k] && y <= yc1[k + 1])
                        c = C[i, k] + (C[i, k + 1] - C[i, k]) / (yc1[k + 1] - yc1[k]) * (y - yc1[k]);

                }
            }
            else
            {
                if (y <= yc2[1]) c = C[i, 1];
                if (y > yc2[jmax[i]]) c = C[i, jmax[i]];
                for (int k = 1; k < jmax[i]; k++)
                {
                    if (y > yc2[k] && y <= yc2[k + 1])
                        c = C[i, k] + (C[i, k + 1] - C[i, k]) / (yc2[k + 1] - yc2[k]) * (y - yc2[k]);

                }
            }

            return c;
        }



        private void button1_Click(object sender, EventArgs e)
        {
            chart1.ChartAreas[0].AxisX.RoundAxisValues();
            chart2.ChartAreas[0].AxisX.RoundAxisValues();

            chart1.ChartAreas[0].AxisX.Minimum = Math.Truncate(x[nf - 1] * 1e4 / 80) * 80;
            chart1.ChartAreas[0].AxisX.Maximum = Math.Truncate(x[nf - 1] * 1e4 / 80) * 80+80;

           
            #region расчет размеров граней по x[i], y[i]

            for (int i = 1; i < nf; i++) chart1.Series[0].Points.AddXY((x[i]-path0) * 1e4, y[i] * 1e4);

            // расчет размеров граней
            Wi[1] = 2 * y[1];
            Wi[nf] = 2 * y[nf - 1];
            for (int i = 2; i < nf; i++)
                Wi[i] = Math.Sqrt((x[i] - x[i - 1]) * (x[i] - x[i - 1]) + (y[i] - y[i - 1]) * (y[i] - y[i - 1]));

            #endregion

            #region расчет координат потоковых точек xj[i], yj1[j], yj2[j]


            for (int i = 1; i <= mf + 1; i++)  xj[i] = x[i];


            double hh = (x[mf + 2] - x[mf + 1]) / mh;
            for (int i = mf + 2; i <= mf + 1 + mh; i++) xj[i] = xj[i - 1] + hh;
            for (int i = mf + 2 + mh; i <= n + 1; i++)  xj[i] = x[i - mh + 1];

            hh = y[1] / mc;
            yj1[1] = 0;
            for (int j = 2; j <= mc + 1; j++)  yj1[j] = yj1[j - 1] + hh;
            for (int j = mc + 2; j <= mc + 1 + mf; j++)  yj1[j] = y[j - mc];

            hh = y[nf - 1] / md;
            yj2[1] = 0;
            for (int j = 2; j <= md + 1; j++) yj2[j] = yj2[j - 1] + hh;
            for (int j = md + 2; j <= md + 1 + mf; j++)  yj2[j] = y[nf - 2 - j + md + 2];


            for (int j = 1; j <= mc + 1 + mf; j++)
            {
                chart1.Series[2].Points.AddXY((xj[mf + 1 + mh / 2] - path0) * 1e4, yj1[j] * 1e4);
                if (j >= mc)
                {
                    chart1.Series[2].Points.AddXY((xj[j - mc] - path0) * 1e4, yj1[j] * 1e4);
                }
                else
                {
                    chart1.Series[2].Points.AddXY((xj[1] - path0) * 1e4, yj1[j] * 1e4);
                }
                chart1.Series[2].Points.AddXY((xj[mf + 1 + mh / 2] - path0) * 1e4, yj1[j] * 1e4);
            }

            for (int j = 1; j <= md + 1 + mf; j++)
            {
                chart1.Series[2].Points.AddXY((xj[mf + 1 + mh / 2] - path0) * 1e4, yj2[j] * 1e4);
                if (j >= md+2)
                {
                    chart1.Series[2].Points.AddXY((xj[(n + 1) / 2 + md+5+mh/2 - j] - path0) * 1e4, yj2[j] * 1e4);
                }
                else
                {
                    chart1.Series[2].Points.AddXY((xj[n + 1] - path0) * 1e4, yj2[j] * 1e4);
                }
                chart1.Series[2].Points.AddXY((xj[mf + 1 + mh / 2] - path0) * 1e4, yj2[j] * 1e4);

            }





            for (int i = 1; i <= (n + 1) /2+1; i++)
            {
                chart1.Series[1].Points.AddXY((xj[i] - path0) * 1e4, 0 * 1e4);
                if (i <= 4)
                {
                    chart1.Series[1].Points.AddXY((xj[i] - path0) * 1e4, y[i] * 1e4);
                }
                else
                {
                    chart1.Series[1].Points.AddXY((xj[i] - path0) * 1e4, y[mf + 1] * 1e4);
                }
                chart1.Series[1].Points.AddXY((xj[i] - path0) * 1e4, 0 * 1e4);

            }
            for (int i = (n + 1) / 2; i <= n + 1; i++)
            {
                chart1.Series[1].Points.AddXY((xj[i] - path0) * 1e4, 0 * 1e4);
                if (i >= (n + 1) / 2  + mh-1)
                {
                    chart1.Series[1].Points.AddXY((xj[i] - path0) * 1e4, y[i - mh+1] * 1e4);
                }
                else
                {
                    chart1.Series[1].Points.AddXY((xj[i] - path0) * 1e4, y[mf + 1] * 1e4);
                }
                chart1.Series[1].Points.AddXY((xj[i] - path0) * 1e4, 0 * 1e4);
            }

            #endregion


            #region расчет шагов по координате между потоковыми точками hxj[i], hyj1[j], hyj2[j]

            for (int i = 1; i <= n; i++)
                hxj[i] = (xj[i + 1] - xj[i]);
            for (int j = 1; j <= m1; j++)
                hyj1[j] = (yj1[j + 1] - yj1[j]);
            for (int j = 1; j <= m2; j++)
                hyj2[j] = (yj2[j + 1] - yj2[j]);
            #endregion


            #region расчет координат центральных точек xc[i], yc1[j], yc2[j]

            xc = new double[n + 1];
            yc1 = new double[m1 + 1];
            yc2 = new double[m2 + 1];


            for (int i = 1; i <= n; i++)
                xc[i] = (xj[i + 1] + xj[i]) / 2;
            for (int j = 1; j <= m1; j++)
                yc1[j] = (yj1[j + 1] + yj1[j]) / 2;
            for (int j = 1; j <= m2; j++)
                yc2[j] = (yj2[j + 1] + yj2[j]) / 2;

            for (int i = 1; i <= n; i++)
                for (int j = 1; j <= jmax[i]; j++)
                {
                    if (i <= mf + mh / 2)
                        chart1.Series[3].Points.AddXY((xc[i] - path0) * 1e4, yc1[j] * 1e4);
                    else
                        chart1.Series[3].Points.AddXY((xc[i] - path0) * 1e4, yc2[j] * 1e4);
                }

            #endregion


            #region расчет шагов по координате центральных точек hxc[i], hyc1[j], hyc2[j]
            n2 = mf + mh / 2;
            for (int i = 1; i < n; i++)
                hxc[i] = (xc[i + 1] - xc[i]);
            for (int j = 1; j < m1; j++)
                hyc1[j] = (yc1[j + 1] - yc1[j]);
            for (int j = 1; j < m2; j++)
                hyc2[j] = (yc2[j + 1] - yc2[j]);


            #endregion

            # region расчет равновесных концентраций Cr[i]

            for (int i = 1; i <= nf; i++)
            {
                finorm[i] = Math.PI - (i - 1) * dfi;
                fig[i] = Math.PI/2 - (i - 1) * dfi;
                gammai[i] = gamma(finorm[i], Convert.ToDouble(comboBox1.Text));
            }


            gammai[0] = gammai[2];
            gammai[nf + 1] = gammai[nf - 1];
            for (int i = 1; i <= nf; i++)
                Crf[i] = -dCdP * (gammai[i + 1] + gammai[i - 1] - 2 * gammai[i] * Math.Cos(dfi)) / (Wi[i] * Math.Sin(dfi));


            if (comboBox2.Text == "расчет по G")
            {

                for (int i = 1; i <= i1; i++) Cr[i] = Crf[i + 1] + (xc[i] - xc[1]) * G * dCdT;
                for (int i = i1 + 1; i <= i2; i++) Cr[i] = Crf[ihf] + (xc[i] - xc[1]) * G * dCdT;
                for (int i = i2 + 1; i <= n; i++) Cr[i] = Crf[i - mh + 2] + (xc[i] - xc[1]) * G * dCdT;
                Cr[0] = Crf[1] + (xc[1] - xc[1]) * G * dCdT;
                Cr[n + 1] = Crf[nf] + (xc[n] - xc[1]) * G * dCdT;
            }
            else
            {
                // с учетом градиента температуры
                for (int i = 1; i <= i1; i++) Cr[i] = Crf[i + 1] + (T[i] - T[1]) * dCdT;
                for (int i = i1 + 1; i <= i2; i++) Cr[i] = Crf[ihf] + (T[i] - T[1]) * dCdT; 
                for (int i = i2 + 1; i <= n; i++) Cr[i] = Crf[i - mh + 2] + (T[i] - T[1]) * dCdT; 
                Cr[0] = Crf[1] + 0;
                Cr[n + 1] = Crf[nf] + (T[n] - T[1]) * dCdT; 



                double g = 0;
                for (int i = 1; i < n; i++)
                {
                    g = (T[i + 1] - T[i]) / (xc[i + 1] - xc[i]);
                }
            }

            #endregion

            

            # region расчет концентраций C_new[i,j]



            Hmin = 100;

            for (int j = 1; j < m1; j++) if (hyc1[j] < Hmin)  Hmin = hyc1[j];
            for (int j = 1; j < m2; j++) if (hyc2[j] < Hmin) Hmin = hyc2[j];
            for (int i = 1; i < n; i++) if (hxc[i] < Hmin) Hmin = hxc[i];
            
            

  

            
            tau = Hmin * Hmin / (d * p);
             kmax =  Convert.ToInt32(tmax / tau);


            //начальное распределение концентраций
            for (int i = 1; i <= n; i++)  for (int j = 1; j <= jmax[i]; j++)  C[i, j] = C_new[i,j] = Cr[i];

            // выбор механизма межфазных процессов
            if (radioButton1.Checked)  
            {
                a = 1;
            }
            if (radioButton2.Checked)
            {
                a = 2;
            }

            // выбор констант для разных систем
            if (radioButton3.Checked)
            {
                kr = 1;
            }
            if (radioButton4.Checked)
            {
                kr = 2;
            }



            if (kr == 2)
            {
                gmax = 400;
                eta = 1.1;
                CsCl = 0.5;
                d = 1e-4;
                dCdT = 1e-3;
                mu2 = 8000;
                mu3 = 1E5;
                mu4 = 5E-4;
                dCdP = 6e-11;
            }
            if (kr == 1)
            {
                gmax = 50;
                eta = 1.2;
                CsCl = 18e-3;
                d = 2.2e-5;
                dCdT = 2e-5;
                mu2 = 3e6;
                mu3 = 1E5;
                mu4 = 0.8E-5;
                dCdP = 8.94e-12;
            }



            // ОСНОВНОЙ ЦИКЛ ПО ВРЕМЕНИ
            for (int k = 0; k < kmax; k++)
            {

                //ВНУТРЕННИЕ ТОЧКИ
                    for (int i = 2; i <= n -1; i++)
                    {
                        for (int j = 1; j < jmax[i]; j++)
                        {
                        
                            //нижние потоки
                        if (j == 1)
                                Jyd = 0;
                            else
                            {
                                if (i <= n2) zn = hyc1[j - 1]; else zn = hyc2[j - 1];
                                Jyd = -d * (C[i, j] - C[i, j - 1]) / zn;
                            }


                        //левые потоки
                        if (i == n2 + 1) Jxl = -d * (C[i, j] - C1(j)) / hxc[i - 1];
                        else
                            Jxl = -d * (C[i, j] - C[i - 1, j]) / hxc[i - 1];

                        //правые потоки
                        if (i == n2) Jxr = -d * (C2(j) - C[i, j]) / hxc[i];
                        else
                            Jxr = -d * (C[i + 1, j] - C[i, j]) / hxc[i];


                        //верхние потоки    
                            if (i <= n2) zn = hyc1[j]; else zn = hyc2[j];
                        Jyu = -d * (C[i, j + 1] - C[i, j]) / zn;

                            
                            //пересчет концентраций
                            if (i <= n2) zm = hyj1[j]; else zm = hyj2[j];
                            C_new[i, j] = C[i, j] + (-(Jxr - Jxl) / hxj[i] - (Jyu - Jyd) / zm) * tau;
                        }
                    }



                //потоки на тыльном фронте
                chart4.Series[0].Points.Clear();
                for (int j = 1; j < jmax[1]; j++)
                {
                    Jxl = -Vel(a, C[1, j] - Cr[0]) * CsCl;  
                    Jxr = -d * (C[2, j] - C[1, j]) / hxc[1];
                    Jyu = -d * (C[1, j + 1] - C[1, j]) / hyc1[j];
                    if (j == 1) Jyd = 0;
                    else Jyd = -d * (C[1, j] - C[1, j - 1]) / hyc1[j-1];
                    C_new[1, j] = C[1, j] + (-(Jxr - Jxl) / hxj[1] - (Jyu - Jyd) / hyj1[j]) * tau;

                    chart4.Series[0].Points.AddXY(j, Vel(a, C[1, j] - Cr[0]));
                    
                }


                   //потоки на переднем фронте
                chart5.Series[0].Points.Clear();
                for (int j = 1; j < jmax[n]; j++)
                {
                    Jxl = -d * (C[n, j] - C[n - 1, j]) / hxc[n - 1];
                    Jxr = Vel(a, C[n, j] - Cr[n + 1]) * CsCl;
                    Jyu = -d * (C[n, j + 1] - C[n, j]) / hyc2[j];
                    if (j == 1) Jyd = 0;
                    else Jyd = -d * (C[n, j] - C[n, j - 1]) / hyc2[j - 1];
                    C_new[n, j] = C[n, j] + (-(Jxr - Jxl) / hxj[n] - (Jyu - Jyd) / hyj2[j]) * tau;
                    
                    chart5.Series[0].Points.AddXY(j, Vel(a, C[n, j] - Cr[n+1]));
                    
                }

                chart6.Series[0].Points.Clear();

                if (comboBox1.Text == "4")
                {
                    // ЕСЛИ 4 ГРАНИ, ТО ТАК:
                    //потоки на горизонтальной грани


                    for (int i = i1 + 1; i <= i2; i++)
                    {
                        if (i == n2 + 1) Jxl = -d * (C[i, jmax[i]] - C1(jmax[i])) / hxc[i - 1];
                        else
                            Jxl = -d * (C[i, jmax[i]] - C[i - 1, jmax[i]]) / hxc[i - 1];

                        if (i == n2) Jxr = -d * (C2(jmax[i]) - C[i, jmax[i]]) / hxc[i];
                        else
                            Jxr = -d * (C[i + 1, jmax[i]] - C[i, jmax[i]]) / hxc[i];


                        if (i <= n2) zn = hyc1[jmax[i] - 1]; else zn = hyc2[jmax[i] - 1];
                        Jyd = -d * (C[i, jmax[i]] - C[i, jmax[i] - 1]) / zn;

                        Jyu = Vel(a, C[i, jmax[i]] - Cr[i]) * CsCl;
                        if (i <= n2) zm = hyj1[jmax[i]]; else zm = hyj2[jmax[i]];

                        C_new[i, jmax[i]] = C[i, jmax[i]] + (-(Jxr - Jxl) / hxj[i] - (Jyu - Jyd) / zm) * tau;

                        chart6.Series[0].Points.AddXY(i, Vel(a, C[i, jmax[i]] - Cr[i]));

                       

                    } 
                  }  
                    // ЕСЛИ 2 ГРАНИ, ТО ТАК - просто остаются прежними - равновесными:
                    if (comboBox1.Text == "2")  for (int i = i1 + 1; i <= i2; i++) C_new[i, jmax[i]] = Cr[i];

                
                //перегон в рабочий массив
                for (int i = 1; i <= n; i++) for (int j = 1; j <= jmax[i]; j++) C[i, j] = C_new[i, j];


                if (k % 100 == 0 && checkBox3.Checked) 
                {
                    clearC(); DrawCXYdll((double)numericUpDown1.Value, xc, yc1, yc2, C); 

                    
                    chart2.Update(); 
                }


            }
            clearC(); DrawCXYdll((double)numericUpDown1.Value, xc, yc1, yc2, C); 
            


            for (int i = 2; i <= mf+1; i++)
            {
                Jx[i] = -d * (C[i , jmax[i-1]] - C[i-1, jmax[i-1]]) / hxc[i-1]; 
                Jy[i] = -d * (C[i-1, jmax[i-1]] - C[i-1, jmax[i-1] - 1]) / hyc1[jmax[i-1]-1] ;
            }
                         for (int i = ihf+1; i <= nf-1; i++)
             {
                 int ic = i - 2 + mh;
                 Jx[i] = -d * (C[ic, jmax[ic]] - C[ic - 1, jmax[ic]]) / hxc[ic - 1];
                 Jy[i] = -d * (C[ic, jmax[ic]] - C[ic, jmax[ic] - 1]) / hyc2[jmax[ic] - 1];
             }
            
            
           
            //при учете кинетики
            double sum = 0;
            for (int j = 1; j <= md; j++) sum += Vel(a, C[n, j] - Cr[n+1]);
            Jn[nf] = sum / md *CsCl; 

            sum = 0;
            for (int j = 1; j <= mc; j++) sum += Vel(a, C[1, j] - Cr[0]);
            Jn[1] = sum / mc * CsCl;
          
            sum = 0;
            for (int i = i1 + 1; i <= i1 + mh; i++)
            sum += Vel(a, C[i, jmax[i]] - Cr[i]);
            Jn[ihf] = sum / mh *CsCl;  



            chart3.Series[0].Points.Clear();

                for (int i = 2; i < nf; i++)
                {
                    if (i != ihf) Jn[i] = -Jx[i] * Math.Sin(fig[i]) + Jy[i] * Math.Cos(fig[i]);
                }
                 for (int i = 1; i <= nf; i++)
                 { 
                     V[i] = -Jn[i] / CsCl;
                     chart3.Series[0].Points.AddXY(i, V[i]);
                 }
            #endregion


        }

        void initial_Data()
        {
            
            chart1.ChartAreas[0].AxisX.Title="x, мкм";
            chart1.ChartAreas[0].AxisY.Title = "y, мкм";
            chart1.ChartAreas[0].AxisX.TitleFont = new Font("Arial", 13, FontStyle.Regular);
            chart1.ChartAreas[0].AxisY.TitleFont = new Font("Arial", 13, FontStyle.Regular);
            n = 2 * mf + mh;
            i1 = mf;
            i2 = i1 + mh;
            ihf = mf + 2;
            m1 = mc + mf;
            m2 = md + mf;

            chart1.ChartAreas[0].AxisX.RoundAxisValues();
            chart2.ChartAreas[0].AxisX.RoundAxisValues();

            hxj = new double[n + 2];
            hyj1 = new double[m1 + 2];
            hyj2 = new double[m2 + 2];

            hxc = new double[n + 2];
            hyc1 = new double[m1 + 2];
            hyc2 = new double[m2 + 2];


            nf = 2 * mf + 3; 
            k = new double[nf + 1];
            b = new double[nf + 1];
            S = new double[nf + 2];
            x_new = new double[nf + 1];
            y_new = new double[nf + 1];
    

            #region расчет jmax[i]
            jmax = new int[n + 1];
            jmax[1] = mc + 1;
            for (int i = 2; i <= mf; i++)
            {
                jmax[i] = jmax[i - 1] + 1;
            }
            for (int i = mf + 1; i <= mf + mh / 2; i++)
            {
                jmax[i] = jmax[i - 1];
            }
            jmax[n] = md + 1;
            for (int i = n - 1; i >= n - mf + 1; i--)
            {
                jmax[i] = jmax[i + 1] + 1;
            }
            for (int i = n - mf; i >= n - mf - mh / 2 + 1; i--)
            {
                jmax[i] = jmax[i + 1];
            }
            #endregion
            #region расчет координат граней x[i], y[i]


           
            Wi = new double[nf + 1];   // массив для размеров граней


           

            #endregion

            xj = new double[n + 2];
            yj1 = new double[n + 2];
            yj2 = new double[n + 2];

            Jn = new double[nf + 1];
            Jx = new double[nf + 1];
            Jy = new double[nf + 1];
            V = new double[nf + 2];

            gammai = new double[nf + 2];
            Crf = new double[nf + 1];
            Cr = new double[n + 2];
            finorm = new double[nf + 2];
            fig = new double[nf + 2];

            if (m1 > m2) m = m1; else m = m2;
            C = new double[n + 2, m + 2];
            C_new = new double[n + 2, m + 2];

            label1.Text = "Номер формы: " + 0.ToString("0") + "  Время (c): " + (0 * t).ToString("0");
            label4.Text = "Площадь включения (исходная):" + 0.ToString("0") +
                   "  непоправленная:" + (0).ToString("0") +
                   "  текущая:" + (0).ToString("0") + "  dy:" + (0).ToString("0"); ;
            label5.Text= "Передний фронт(мкм):  " + 0.ToString("0") ;
            label6.Text = "Тыльный фронт(мкм):  " + 0.ToString("0");

        }

        private void Form1_Load(object sender, EventArgs e)
        {

            initial_Data();
            x = new double[nf + 1];
            y = new double[nf + 1];

            dfi = Math.PI / (nf - 1);
            x0 = -R * Math.Cos(dfi / 2);


            for (int i = 1; i < nf; i++)
            {
                fi = Math.PI + dfi / 2 - i * dfi;
                x[i] = X2(fi) - x0;
                y[i] = Y2(fi);

                chart1.Series[0].Points.AddXY((x[i] - path0) * 1e4, y[i] * 1e4);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            
            Snf2 = V[ihf]*t; //вертикальное смещение горизонтальной (центральной) грани

            
            for (int i = 1; i <= nf; i++)
            {
                if (i == ihf) S[i] = 0;
                else S[i] = -V[i] / Math.Sin(fig[i]) * t; 
            }
            

            //расчет старых k и b  (наклоны граней k не меняются в ходе эволюции формы)
            for (int i = 1; i < nf-1; i++)
            {
                k[i+1] = (y[i + 1] - y[i]) / (x[i + 1] - x[i]);
                b[i+1] = y[i] + x[i] * (y[i] - y[i + 1]) / (x[i + 1] - x[i]);
            }

            
            b[nf / 2 + 1] += Snf2; //учет вертикального смещения горизонтальной грани

            //расчет новых координат концов внутренних граней
            for (int i = 2; i < nf-1; i++)
            {
                x_new[i] = ((b[i + 1] - b[i]) - (k[i + 1]*S[i+1]  - k[i]*S[i] )) / (k[i] - k[i + 1]);
                y_new[i] = k[i] * (x_new[i] - S[i]) + b[i];

            }

            //расчет новых координат концов  крайних (вертикальных) граней
            x_new[1] = x[1] + S[1];
            y_new[1] = k[2] * (x_new[1] - S[2]) + b[2];

            x_new[nf-1] = x[nf-1] + S[nf];
            y_new[nf-1] = k[nf-1] * (x_new[nf-1] - S[nf-1]) + b[nf-1];

            chart1.Series[4].Points.Clear();
            //построение новой формы
            for (int i = 1; i < nf; i++)
                chart1.Series[4].Points.AddXY((x_new[i] - path0) * 1e4, y_new[i] * 1e4);

            for (int i = 1; i < nf; i++) 
            { 
                x[i] = x_new[i]; 
                y[i] = y_new[i]; 
            }
       
        }

        private void button4_Click(object sender, EventArgs e)
        {
            chart4.Series[0].ToolTip = "X = #VALX, Y = #VALY";

            button5.Focus();
            stop = false;

            G = Convert.ToDouble(textBox1.Text);
            t = Convert.ToDouble(textBox2.Text); //шаг по времени для изменения формы
            time = time0;

            chart8.ChartAreas[0].AxisX.Minimum = time0;
            
            for (num = 1+num0; num <= 100000+num0; num++)
            {

                chart1.Series[0].Points.Clear();
                chart1.Series[1].Points.Clear();
                chart1.Series[2].Points.Clear();
                chart1.Series[3].Points.Clear();
                chart1.Series[4].Points.Clear();
                chart3.Series[0].Points.Clear();
                chart3.Series[1].Points.Clear();



                button1.PerformClick();
                button2.PerformClick();

                //толщина
                L = (x[nf-1] - x[1]);
                Lold = L;
                //ширина
                W = 2*y[mf+1];

                #region расчет площади включения Sv

                Sv_new = 0;
                for (int j = 1; j < nf - 1; j++) Sv_new += (y[j + 1] + y[j]) * (x[j + 1] - x[j]);
                if (num == 1+num0) Sv0 = Sv_new;
               
                dSv = -Sv_new + Sv0;
                dy = dSv / (x[nf - 1] - x[1]) / 2;
                for (int j = 1; j <= nf - 1; j++) y[j] += dy;   
                Sv = 0;
                for (int j = 1; j < nf - 1; j++) Sv += (y[j + 1] + y[j]) * (x[j + 1] - x[j]);
                label4.Text = "Площадь включения (исходная):" + (Sv0 * 1e8).ToString("0.0000") +
                   "  непоправленная:" + (Sv_new * 1e8).ToString("0.0000")+
                   "  текущая:" + (Sv * 1e8).ToString("0.0000") +"  dy:" + (dy * 1e4).ToString("0.00e-00");
                label4.Update();
                #endregion

                time += t;  //общее время изменения формы
                path = x[1];  // общий пройденный путь

                if (num == 1 + num0 || (num-num0) % 10 == 0)
                {

                    label1.Text = "Номер формы: " + num.ToString("0") + "  Время (c): " + (time).ToString("0") + "  Путь (мкм): " + (path*1e4).ToString("0.00")
                        + "  Толщина (мкм): " + (L * 1e4).ToString("0.00") + "  Ширина(мкм): " + (W * 1e4).ToString("0.00");
                        
                    label5.Text= "Передний фронт(мкм):" + (2 * y[nf - 1] * 1e4).ToString("0.00");
                    label6.Text =  "Тыльный фронт(мкм):" + (2 * y[1] * 1e4).ToString("0.00");
                    label1.Update();
                    label5.Update();
                    label6.Update();

                    chart7.Series[0].Points.AddXY(time, 2 * y[nf - 1] * 1e4);  //Wd
                    chart7.Series[1].Points.AddXY(time, 2 * y[1] * 1e4);  //Wc
                    chart7.Series[2].Points.AddXY(time, L * 1e4);  //L
                    chart7.Series[3].Points.AddXY(time, W * 1e4);  // W

                    chart8.Series[0].Points.AddXY(time, dC(a, V[1]));
                    chart8.Series[1].Points.AddXY(time, dC(a, V[nf]));


                    chart1.Update();
                    chart2.Update();
                    chart3.Update();
                    chart4.Update();
                    chart5.Update();
                    chart6.Update();
                    chart7.Update();
                    if (stop) break;

                    label8.Text = "";
                    for (int z = 1; z < nf - 1; z++)
                        label8.Text += k[z + 1].ToString("0.000000 ");
                    label8.Update();


                }
                Application.DoEvents();

                

            }
        }


        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
               
            {
                StreamReader fr = new StreamReader(openFileDialog1.FileName);



                num0 = Convert.ToInt32(fr.ReadLine());
                time0 = Convert.ToDouble(fr.ReadLine());
                path0 = Convert.ToDouble(fr.ReadLine());

                path0 = 0;  

                G = Convert.ToDouble(fr.ReadLine());
                t = Convert.ToDouble(fr.ReadLine());


                textBox1.Text = G.ToString("0.00");
                textBox2.Text = t.ToString("0.00");

                mf= Convert.ToInt32 (fr.ReadLine());
                mh = Convert.ToInt32(fr.ReadLine());
                nf = Convert.ToInt32(fr.ReadLine());
                mc = Convert.ToInt32(fr.ReadLine());
                md = Convert.ToInt32(fr.ReadLine());

                x = new double[nf + 1];
                y = new double[nf + 1];

                for (int i = 0; i < nf; i++)
                    x[i] = Convert.ToDouble(fr.ReadLine())+path0;

                for (int i = 0; i < nf; i++)
                    y[i] = Convert.ToDouble(fr.ReadLine());

                a = Convert.ToInt32(fr.ReadLine());
                kr = Convert.ToInt32(fr.ReadLine());
                mu2 = Convert.ToDouble(fr.ReadLine());
                mu3 = Convert.ToDouble(fr.ReadLine());
                mu4 = Convert.ToDouble(fr.ReadLine());

                if (a == 1) radioButton1.Checked = true;
                if (a == 2) radioButton2.Checked = true;
                if (kr == 1) radioButton3.Checked = true;
                if (kr == 2) radioButton4.Checked = true;

                int m = Convert.ToInt32(fr.ReadLine());
                double tt;
                for (int i = 0; i < m; i++)
                {
                    fr.ReadLine();
                    tt=Convert.ToDouble(fr.ReadLine());

                 
                    
                    chart7.Series[0].Points.AddXY(tt, Convert.ToDouble(fr.ReadLine()));
                    chart7.Series[1].Points.AddXY(tt, Convert.ToDouble(fr.ReadLine()));
                    chart7.Series[2].Points.AddXY(tt, Convert.ToDouble(fr.ReadLine()));
                    chart7.Series[3].Points.AddXY(tt, Convert.ToDouble(fr.ReadLine()));

                }

                fr.Close();
            }

            initial_Data();
            
        }

        private void сохранитьФормуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamWriter fr = new StreamWriter(saveFileDialog1.FileName);

                fr.WriteLine("{0}  ", num);
                fr.WriteLine("{0, 18:e9}  ", time);
                fr.WriteLine("{0, 18:e9}  ", path);
                fr.WriteLine("{0, 18:f2}  ", G);
                fr.WriteLine("{0, 18:f2}  ", t);

                fr.WriteLine("{0}  ", mf);
                fr.WriteLine("{0}  ", mh);
                fr.WriteLine("{0}  ", nf);
                fr.WriteLine("{0}  ", mc);
                fr.WriteLine("{0}  ", md);

                for (int i = 0; i < nf; i++)
                    fr.WriteLine("{0, 12:e3}  ", x[i]);
                for (int i = 0; i < nf; i++)
                    fr.WriteLine("{0, 12:e3} ", y[i]);

                fr.WriteLine("{0}  ", a);
                fr.WriteLine("{0}  ", kr);
                fr.WriteLine("{0, 12:e3} ", mu2);
                fr.WriteLine("{0, 12:e3} ", mu3);
                fr.WriteLine("{0, 12:e3} ", mu4);

                fr.WriteLine("{0}  ", chart7.Series[0].Points.Count);
                for (int i = 0; i < chart7.Series[0].Points.Count; i++)
                {
                    fr.WriteLine("{0}  ", i);
                    fr.WriteLine("{0, 12:e3}", chart7.Series[0].Points[i].XValue);
                    fr.WriteLine("{0, 12:e3}", chart7.Series[0].Points[i].YValues[0]);
                    fr.WriteLine("{0, 12:e3}", chart7.Series[1].Points[i].YValues[0]);
                    fr.WriteLine("{0, 12:e3}", chart7.Series[2].Points[i].YValues[0]);
                    fr.WriteLine("{0, 12:e3}", chart7.Series[3].Points[i].YValues[0]);


                }

                fr.Close();
            }

        }
        private void сохранитьФорму2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamWriter fr = new StreamWriter(saveFileDialog1.FileName);

                fr.WriteLine("{0}  ", num);
                fr.WriteLine("{0, 18:e9}  ", time);
                fr.WriteLine("{0, 18:e9}  ", path);
                fr.WriteLine("{0, 18:f2}  ", G);
                fr.WriteLine("{0, 18:f2}  ", t);

                fr.WriteLine("{0}  ", mf);
                fr.WriteLine("{0}  ", mh);
                fr.WriteLine("{0}  ", nf);
                fr.WriteLine("{0}  ", mc);
                fr.WriteLine("{0}  ", md);

                for (int i = 0; i < nf; i++)
                    fr.WriteLine("{0, 12:e3}  ", x[i]);
                for (int i = 0; i < nf; i++)
                    fr.WriteLine("{0, 12:e3} ", y[i]);

                fr.WriteLine("{0}  ", a);
                fr.WriteLine("{0}  ", kr);
                fr.WriteLine("{0} ", gmax);
                fr.WriteLine("{0} ", p);
                fr.WriteLine("{0} ", comboBox1.Text);
                fr.WriteLine("{0, 12:e3} ", mu2);
                fr.WriteLine("{0, 12:e3} ", mu3);
                fr.WriteLine("{0, 12:e3} ", mu4);
                fr.WriteLine("{0, 12:e3} ", d);
                fr.WriteLine("{0, 12:e3} ", eta);
                fr.WriteLine("{0, 12:e3} ", CsCl);
                fr.WriteLine("{0, 12:e3} ", dCdT);
                fr.WriteLine("{0, 12:e3} ", dCdP);


                fr.WriteLine("{0}  ", chart7.Series[0].Points.Count);
                for (int i = 0; i < chart7.Series[0].Points.Count; i++)
                {
                    fr.WriteLine("{0}  ", i);
                    fr.WriteLine("{0, 12:e3}", chart7.Series[0].Points[i].XValue);
                    fr.WriteLine("{0, 12:e3}", chart7.Series[0].Points[i].YValues[0]);
                    fr.WriteLine("{0, 12:e3}", chart7.Series[1].Points[i].YValues[0]);
                    fr.WriteLine("{0, 12:e3}", chart7.Series[2].Points[i].YValues[0]);
                    fr.WriteLine("{0, 12:e3}", chart7.Series[3].Points[i].YValues[0]);


                }

                fr.Close();
            }
        }

        private void открытьФорму2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader fr = new StreamReader(openFileDialog1.FileName);



                num0 = Convert.ToInt32(fr.ReadLine());
                time0 = Convert.ToDouble(fr.ReadLine());
                path0 = Convert.ToDouble(fr.ReadLine());

                path0 = 0; 

                G = Convert.ToDouble(fr.ReadLine());
                t = Convert.ToDouble(fr.ReadLine());


                textBox1.Text = G.ToString("0.00");
                textBox2.Text = t.ToString("0.00");

                mf = Convert.ToInt32(fr.ReadLine());
                mh = Convert.ToInt32(fr.ReadLine());
                nf = Convert.ToInt32(fr.ReadLine());
                mc = Convert.ToInt32(fr.ReadLine());
                md = Convert.ToInt32(fr.ReadLine());

                x = new double[nf + 1];
                y = new double[nf + 1];

                for (int i = 0; i < nf; i++)
                    x[i] = Convert.ToDouble(fr.ReadLine()) + path0;

                for (int i = 0; i < nf; i++)
                    y[i] = Convert.ToDouble(fr.ReadLine());

                a = Convert.ToInt32(fr.ReadLine());
                kr = Convert.ToInt32(fr.ReadLine());
                gmax = Convert.ToInt32(fr.ReadLine());
                p = Convert.ToInt32(fr.ReadLine());
                gr = Convert.ToInt32(fr.ReadLine());
                mu2 = Convert.ToDouble(fr.ReadLine());
                mu3 = Convert.ToDouble(fr.ReadLine());
                mu4 = Convert.ToDouble(fr.ReadLine());
                d = Convert.ToDouble(fr.ReadLine());
                eta = Convert.ToDouble(fr.ReadLine());
                CsCl = Convert.ToDouble(fr.ReadLine());
                dCdT = Convert.ToDouble(fr.ReadLine());
                dCdP = Convert.ToDouble(fr.ReadLine());

                if (gr == 2) comboBox1.Text = "2";
                if (gr == 4) comboBox1.Text = "4";
                if (a == 1) radioButton1.Checked = true;
                if (a == 2) radioButton2.Checked = true;
                if (kr == 1) radioButton3.Checked = true;
                if (kr == 2) radioButton4.Checked = true;

                int m = Convert.ToInt32(fr.ReadLine());
                double tt;
                for (int i = 0; i < m; i++)
                {
                    fr.ReadLine();
                    tt = Convert.ToDouble(fr.ReadLine());



                    chart7.Series[0].Points.AddXY(tt, Convert.ToDouble(fr.ReadLine()));
                    chart7.Series[1].Points.AddXY(tt, Convert.ToDouble(fr.ReadLine()));
                    chart7.Series[2].Points.AddXY(tt, Convert.ToDouble(fr.ReadLine()));
                    chart7.Series[3].Points.AddXY(tt, Convert.ToDouble(fr.ReadLine()));

                }

                fr.Close();
            }

            initial_Data();
            
        }

        private void сохранитьДиаграммуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamWriter fr = new StreamWriter(saveFileDialog1.FileName);
                for (int i = 0; i < chart7.Series[0].Points.Count; i++)
                    fr.WriteLine("{0, 12:e3},{1,12:e3}, {2,12:e3}, {3,12:e3}", chart7.Series[0].Points[i].XValue, chart7.Series[0].Points[i].YValues[0],
                      chart7.Series[1].Points[i].YValues[0], chart7.Series[2].Points[i].YValues[0], chart7.Series[3].Points[i].YValues[0]);
                
                fr.Close();
            }

        }   

        private void закрытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void сохранитьФормуДляТПToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamWriter fr = new StreamWriter(saveFileDialog1.FileName);

                fr.WriteLine("{0}  ", num);
                fr.WriteLine("{0, 18:e9}  ", time);
                fr.WriteLine("{0, 18:e9}  ", path);
                
                fr.WriteLine("{0}  ", mf);
                fr.WriteLine("{0}  ", mh);
                fr.WriteLine("{0}  ", nf);
                fr.WriteLine("{0}  ", mc);
                fr.WriteLine("{0}  ", md);

                for (int i = 1; i < nf; i++)
                    fr.WriteLine("{0, 18:e9}  ", x[i]-x[1]);
                for (int i = 1; i < nf; i++)
                    fr.WriteLine("{0, 18:e9} ", y[i]);

                fr.WriteLine("{0}  ", n);
                fr.WriteLine("{0}  ", m1);
                fr.WriteLine("{0}  ", m2);

                for (int i = 1; i <= n; i++)
                    fr.WriteLine("{0, 15}  ", jmax[i]);

                for (int i = 1; i <= n; i++)
                    fr.WriteLine("{0, 18:e9}  ", hxj[i]);
                for (int j = 1; j <= m1; j++)
                    fr.WriteLine("{0, 18:e9} ", hyj1[j]);
                for (int j = 1; j <= m2; j++)
                    fr.WriteLine("{0, 18:e9} ", hyj2[j]);




                fr.Close();
            }
        }

        private void открытьФормуДляМПToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader fr = new StreamReader(openFileDialog1.FileName);

                num0 = Convert.ToInt32(fr.ReadLine());
                time0 = Convert.ToDouble(fr.ReadLine());
                path0 = Convert.ToDouble(fr.ReadLine());


                mf = Convert.ToInt32(fr.ReadLine());
                mh = Convert.ToInt32(fr.ReadLine());
                nf = Convert.ToInt32(fr.ReadLine());
                mc = Convert.ToInt32(fr.ReadLine());
                md = Convert.ToInt32(fr.ReadLine());

                x = new double[nf + 1];
                y = new double[nf + 1];

                for (int i = 1; i < nf; i++)
                    x[i] = Convert.ToDouble(fr.ReadLine())+ path0;

                for (int i = 1; i < nf; i++)
                    y[i] = Convert.ToDouble(fr.ReadLine());

                
                n = 2 * mf + mh;

                T = new double[n + 2];
                for (int i = 1; i <= n; i++)
                    T[i] = Convert.ToDouble(fr.ReadLine());

                fr.Close();

                comboBox2.Text = "расчет по ТП";
            }
            initial_Data();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            clearC(); DrawCXYdll((double)numericUpDown1.Value, xc, yc1, yc2, C);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }



        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            chart7.Series[2].Enabled = checkBox1.Checked;
            if (chart7.Series[2].Enabled)
            {
                chart7.ChartAreas[0].AxisY.Minimum = chart7.Series[2].Points.FindMinByValue().YValues[0];
                chart7.ChartAreas[0].AxisY.Maximum = chart7.Series[2].Points.FindMaxByValue().YValues[0];
            }
        }
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            chart7.Series[1].Enabled = checkBox4.Checked;
            if (chart7.Series[1].Enabled)
            {
                chart7.ChartAreas[0].AxisY.Minimum = chart7.Series[1].Points.FindMinByValue().YValues[0];
                chart7.ChartAreas[0].AxisY.Maximum = chart7.Series[1].Points.FindMaxByValue().YValues[0];
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            chart7.Series[3].Enabled = checkBox2.Checked;
            
            if (chart7.Series[3].Enabled)
            {
                chart7.ChartAreas[0].AxisY.Minimum = chart7.Series[3].Points.FindMinByValue().YValues[0];
                chart7.ChartAreas[0].AxisY.Maximum = chart7.Series[3].Points.FindMaxByValue().YValues[0];
            }
        }
        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            chart7.Series[0].Enabled = checkBox5.Checked;
            if (chart7.Series[0].Enabled)
            {
                chart7.ChartAreas[0].AxisY.Minimum = chart7.Series[0].Points.FindMinByValue().YValues[0];
                chart7.ChartAreas[0].AxisY.Maximum = chart7.Series[0].Points.FindMaxByValue().YValues[0];
            }
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox6.Checked)
            { 
            chart7.ChartAreas[0].AxisY.Minimum =0;
            chart7.ChartAreas[0].AxisY.Maximum = Double.NaN;
            }
        }

    }

}

