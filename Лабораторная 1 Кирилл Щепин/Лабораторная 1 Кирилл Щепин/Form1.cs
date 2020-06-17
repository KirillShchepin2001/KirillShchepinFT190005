using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KirSchep

{
    public partial class Form1 : Form

    {
        class Point
        {
            public double x;
            public double y;
            public double z;
            public double H;

            static public Point Parse(string str)
            {
                Point point = new Point();

                string[] st = str.Split(' ');

                point.x = double.Parse(st[0]);
                point.y = double.Parse(st[1]);
                point.z = double.Parse(st[2]);
                point.H = double.Parse(st[3]);

                return point;
            }
        }

        class Line
        {
            public int begin;
            public int end;

            static public Line Parse(string str)
            {
                Line line = new Line();

                string[] a = str.Split(' ');

                line.begin = int.Parse(a[0]);
                line.end = int.Parse(a[1]);

                return line;
            }
        }

        List<Point> toch = new List<Point>();
        List<Line> line = new List<Line>();
     
            Graphics g;

        void Schitivanie()
        {
            StreamReader a = new StreamReader("C:\\Kirill2001\\kirill.txt.txt");

            var str = a.ReadLine();
            while (true)
            {

                if (a.EndOfStream) break;
                if (str == "*") break;
                toch.Add(Point.Parse(str));
                str = a.ReadLine();
            }
            str = a.ReadLine();
            while (true)
            {

                if (a.EndOfStream) break;
                if (str == "*") break;
                line.Add(Line.Parse(str));
                str = a.ReadLine();
            }

        }

        void Vmeshenie()
        {
            double max1 = toch[0].x;
            double min1 = toch[0].x;
            double max2 = toch[0].y;
            double min2 = toch[0].y;

            for (int i = 0; i < toch.Count; i++)
            {
                if (toch[i].x > max1) max1 = toch[i].x;
                if (toch[i].y > max2) max2 = toch[i].y;

                if (toch[i].x < min1) min1 = toch[i].x;
                if (toch[i].y < min2) min2 = toch[i].y;
            }

            double q;

            if (pictureBox1.Height / (max2 - min2) > pictureBox1.Width / (max1 - min1))
                q = pictureBox1.Width / (max1 - min1);
            else q = pictureBox1.Height / (max2 - min2);


            for (int i = 0; i < toch.Count; i++)
            {
                toch[i].x -= min1;
                toch[i].y -= min2;

                toch[i].x *= q;
                toch[i].y *= q;
                toch[i].z *= q;
            }
        }

        void Risunok()
        {
            g.Clear(Color.White);

            {
                for (int i = 0; i < line.Count; i++)
                {
                    g.DrawLine(new Pen(Color.Red, 2),
                        (float)toch[line[i].begin - 1].x, (float)toch[line[i].begin - 1].y,
                        (float)toch[line[i].end - 1].x, (float)toch[line[i].end - 1].y);
                }
            }
            pictureBox1.Invalidate();
        }

        void Sdvig(double x, double y, double z)
        {

            for (int i = 0; i < toch.Count; i++)
            {
                toch[i].x = toch[i].x + toch[i].H * x;
                toch[i].y = toch[i].y + toch[i].H * y;
                toch[i].z = toch[i].z + toch[i].H * z;
            }
        }

        void Masshtab(double X, double Y, double Z)
        {

            for (int i = 0; i < toch.Count; i++)
            {
                toch[i].x = toch[i].x * X;
                toch[i].y = toch[i].y * Y;
                toch[i].z = toch[i].z * Z;
            }
        }

        void Povorot(double alp, double bet, double gam)
        {

            double newX, newY, newZ;
            for (int i = 0; i < toch.Count; i++)
            {
                newY = toch[i].y * Math.Cos(alp * 180 / Math.PI) - toch[i].z * Math.Sin(alp * 180 / Math.PI);
                newZ = toch[i].y * Math.Sin(alp * 180 / Math.PI) + toch[i].z * Math.Cos(alp * 180 / Math.PI);
                toch[i].y = newY;
                toch[i].z = newZ;
            }
            for (int i = 0; i < toch.Count; i++)
            {
                newX = toch[i].x * Math.Cos(bet * 180 / Math.PI) + toch[i].z * Math.Sin(bet * 180 / Math.PI);
                newZ = -toch[i].x * Math.Sin(bet * 180 / Math.PI) + toch[i].z * Math.Cos(bet * 180 / Math.PI);
                toch[i].x = newX;
                toch[i].z = newZ;
            }
            for (int i = 0; i < toch.Count; i++)
            {
                newY = toch[i].x * Math.Sin(gam * 180 / Math.PI) + toch[i].y * Math.Cos(gam * 180 / Math.PI);
                newX = toch[i].x * Math.Cos(gam * 180 / Math.PI) - toch[i].y * Math.Sin(gam * 180 / Math.PI);
                toch[i].y = newY;
                toch[i].x = newX;
            }
        }

        void Dvig(double xy, double xz, double yz, double zx, double yx, double zy)
        {
            for (int i = 0; i < toch.Count; i++)
            {
                toch[i].x = toch[i].x + toch[i].y * xy;
            }
            for (int i = 0; i < toch.Count; i++)
            {
                toch[i].x = toch[i].x + toch[i].z * xz;
            }
            for (int i = 0; i < toch.Count; i++)
            {
                toch[i].y = toch[i].y + toch[i].x * yx;
            }
            for (int i = 0; i < toch.Count; i++)
            {
                toch[i].y = toch[i].y + toch[i].z * yz;
            }
            for (int i = 0; i < toch.Count; i++)
            {
                toch[i].z = toch[i].z + toch[i].x * zx;
            }
            for (int i = 0; i < toch.Count; i++)
            {
                toch[i].x = toch[i].z + toch[i].y * zy;
            }
        }

        public Form1()
        {
            InitializeComponent();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            g = Graphics.FromImage(pictureBox1.Image);

            Schitivanie();
            Vmeshenie();
            Risunok();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Masshtab(Double.Parse(x.Text),
                Double.Parse(y.Text),
                Double.Parse(z.Text));
            Risunok();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Povorot(Double.Parse(alp.Text),
                  Double.Parse(bet.Text),
                  Double.Parse(gam.Text));
            Risunok();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Dvig(Double.Parse(xy.Text),
                              Double.Parse(yx.Text),
                              Double.Parse(xz.Text),
                              Double.Parse(zx.Text),
                              Double.Parse(yz.Text),
                              Double.Parse(zy.Text));
           Risunok();
        }

        private void button5_Click(object sender, EventArgs e)
        {
           Sdvig(Double.Parse(t1.Text),
                Double.Parse(t2.Text),
                Double.Parse(t3.Text));
            Risunok();
        }

        private void button6_Click(object sender, EventArgs e)
        {
           
            Vmeshenie();
            Risunok();
            
        }

    }
}
