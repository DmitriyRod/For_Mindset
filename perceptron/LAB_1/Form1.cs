using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;

namespace LAB_1
{
    public partial class Form1 : Form
    {
        const double theta = -1;
        const int N = 10;
        int[,] Xi = new int[N, N];
        double[,] Wi = new double[N, N];
        Bitmap bmp;
        Bitmap b2;
        Graphics g;
        bool isPressed = false;
        double y1 = 0;
        int X;
        int Y;
        int X1;
        int Y1;
        public Form1()
        {
            InitializeComponent();
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(bmp);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
        }
        private void READ()
        {
            string str = File.ReadAllText("Inp.txt");
            char[] separatingChars = { ' ','\r','\n','\t' }; //список знаков разделителей
            int K = 0;
            string[] words = str.Split(separatingChars, System.StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    Wi[i, j] = Convert.ToDouble(words[K]);
                    K++;
                }
            }
        }
        private void SAVE()
        {
            string str = "";
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    str += Wi[i, j].ToString() + " ";
                }
                str += "\r";
            }
            File.WriteAllText("Inp.txt",str);
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Draw()
        {
            Pen p = new Pen(Color.Black,2);
            g.DrawLine(p,X1,Y1,X,Y);
            pictureBox1.Image = bmp;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            isPressed = true;
            X = e.X;
            Y = e.Y;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isPressed)
            {
                X1 = X;
                Y1 = Y;
                X = e.X;
                Y = e.Y;
                Draw();
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            isPressed = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            pictureBox1.Image = null;
            pictureBox2.Image = null;
            Graphics.FromImage(bmp).Clear(Color.White);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            READ();
            //границы обрезки изображения
            int top = 0;
            int left = 0;
            int right = 0;
            int bottom = 0;
            bool flag;
            //top
            flag = true;
            for (int i = 0; i < pictureBox1.Height; i++)
            {
                if (flag == false)
                {
                    break;
                }
                for (int j = 0; j < pictureBox1.Width; j++)
                {
                    if (flag == false)
                    {
                        break;
                    }
                    if (bmp.GetPixel(j, i).R == 0 && bmp.GetPixel(j, i).G == 0 && bmp.GetPixel(j, i).B == 0)
                    {
                        top = i;
                        flag = false;
                        break;
                    }
                }
            }
            //left
            flag = true;
            for (int i = 0; i < pictureBox1.Width; i++)
            {
                if (flag == false)
                {
                    break;
                }
                for (int j = 0; j < pictureBox1.Height; j++)
                {
                    if (bmp.GetPixel(i, j).R == 0 && bmp.GetPixel(i, j).G == 0 && bmp.GetPixel(i, j).B == 0)
                    {
                        left = i;
                        flag = false;
                        break;
                    }
                }
            }
   
            //right
            flag = true;
            for (int i = pictureBox1.Width - 1; i >= 0; i--)
            {
                if (flag == false)
                {
                    break;
                }
                for (int j = pictureBox1.Height - 1; j >= 0; j--)
                {
                    if (bmp.GetPixel(i, j).R == 0 && bmp.GetPixel(i, j).G == 0 && bmp.GetPixel(i, j).B == 0)
                    {
                        right = i;
                        flag = false;
                        break;
                    }
                }
            }
            //bottom
            flag = true;
            for (int i = pictureBox1.Height - 1; i >= 0; i--)
            {
                if (flag == false)
                {
                    break;
                }
                for (int j = pictureBox1.Width - 1; j >= 0; j--)
                {
                    if (flag == false)
                    {
                        break;
                    }
                    if (bmp.GetPixel(j, i).R == 0 && bmp.GetPixel(j, i).G == 0 && bmp.GetPixel(j, i).B == 0)
                    {
                        bottom = i;
                        flag = false;
                        break;
                    }
                }
            }
            //----------------------------
            //обнуляем матрицу
            //----------------------------
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    Xi[i,j] = 0;
                }
            }
            //вырезаем часть картинки с символом
            b2 = bmp.Clone(new Rectangle(left, top, right - left, bottom - top), bmp.PixelFormat);
            pictureBox2.Image = b2;
            //b2.Save("Test.bmp");
            //составляем матрицу Xi
            flag = false;
            int h1 = b2.Width / N;
            int h2 = b2.Height / N;
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    flag = false;
                    for (int x = (h1 * i); x < (h1 * (i + 1)); x++)
                    {
                        if (flag == true) break;
                        for (int y = (h2 * j); y < (h2 * (j + 1)); y++)
                        {
                            if (b2.GetPixel(x, y).R == 0 && b2.GetPixel(x, y).G == 0 && b2.GetPixel(x, y).B == 0)
                            {
                                flag = true;
                                Xi[j, i] = 1;
                                break;
                            }
                        }
                    }
                }
            }
            //перецептрон
            double FS = 0;
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    FS += Xi[i, j] * Wi[i, j] - theta;
                }
            }
            if ( FS >= 0 )
            {
                textBox1.Clear();
                textBox1.Text = "2";
                y1 = 1;
            }
            else
            {
                textBox1.Clear();
                textBox1.Text = "7";
                y1 = 0;
            }
            textBox1.Text += " " + FS.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            double tmp = 0;
            double n = 0.5;
            if (y1 == 1)
            {
                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < N; j++)
                    {
                        tmp = Wi[i, j] + n * -1 * Xi[i, j];
                        Wi[i, j] = tmp;
                    }
                }
            }
            else
            {
                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < N; j++)
                    {
                        tmp = Wi[i, j] + n * 1 * Xi[i, j];
                        Wi[i, j] = tmp;
                    }
                }
            }
            SAVE();
        }
    }
}
