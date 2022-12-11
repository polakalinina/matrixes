using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Task1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Draw();
        }

        private void Draw()
        {
            var bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            var times = new int[8];
            const int matrixSize = 5000;
            var matrixProcessor = new MatrixProcessor(matrixSize);
            
            var watch = new Stopwatch();

            watch.Start();
            matrixProcessor.SumSequentially(matrixSize);
            watch.Stop();
            
            times[0] = (int) watch.ElapsedMilliseconds;
            label1.Text = "Последовательно: " + times[0];
            
            for (var threadsCount = 2; threadsCount < 9; threadsCount++)
            {
                watch.Restart();
                matrixProcessor.SumParallel(threadsCount, matrixSize);
                watch.Stop();
                times[threadsCount - 1] = (int) watch.ElapsedMilliseconds;
            }

            label2.Text = "При 2 потоках: " + times[1];
            label3.Text = "При 3 потоках: " + times[2];
            label4.Text = "При 4 потоках: " + times[3];
            label5.Text = "При 5 потоках: " + times[4];
            label6.Text = "При 6 потоках: " + times[5];
            label7.Text = "При 7 потоках: " + times[6];
            label8.Text = "При 8 потоках: " + times[7];

            var graphic = Graphics.FromImage(bmp);
            graphic.TranslateTransform(50, 250);
            graphic.ScaleTransform(0.4f, 0.4f);
            var points = new PointF[8];

            var max = times.Max();
            
            var redPen = new Pen(Color.Brown, 8f);
            var blackPen = new Pen(Color.Black, 3f);
            
            graphic.DrawLine(blackPen, new Point(-100, 0), new Point(1000, 0));//ось X
            graphic.DrawLine(blackPen, new Point(0, -1000), new Point(0, 100));//ось Y

            var k = (double)pictureBox1.Height / max;
            for (var i = 0; i < 8; i++)
            {
                points[i] = new PointF(i * 100, -float.Parse((times[i] * k).ToString()));
            }
            
            // Рисуем линии графика
            graphic.DrawLines(redPen, points);

            pictureBox1.Image = bmp;
            
            var timesByThread = new string[7];
            for (var threadsCount = 2; threadsCount < 9; threadsCount++)
            {
                var parallelTimes = matrixProcessor.SumParallel(threadsCount, matrixSize);
                var timeAsString = "";
                foreach (var parallelTime in parallelTimes)
                {
                    timeAsString += parallelTime + ", ";
                }
                timesByThread[threadsCount - 2] = timeAsString;
            }
            
            label9.Text = "При 2 потоках: " + timesByThread[0];
            label10.Text = "При 3 потоках: " + timesByThread[1];
            label11.Text = "При 4 потоках: " + timesByThread[2];
            label12.Text = "При 5 потоках: " + timesByThread[3];
            label13.Text = "При 6 потоках: " + timesByThread[4];
            label14.Text = "При 7 потоках: " + timesByThread[5];
            label15.Text = "При 8 потоках: " + timesByThread[6];
        }
    }
}