using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AllLabs
{
    public partial class Form7 : Form
    {
        public Form7()
        {
            InitializeComponent();
        }

        public Form7(Form1 f)
        {
            InitializeComponent();
        }

        private void Form7_Load(object sender, EventArgs e)
        {

        }

        public void DrowTree(int x, int y, int len, double deegre, Color start, Color end, int depth)
        {
            try
            {
                double x1, y1;
                x1 = x + len * Math.Sin(Math.PI * deegre / 180.0);
                y1 = y + len * Math.Cos(Math.PI * deegre / 180.0);

                LinearGradientBrush linGrBrush = new LinearGradientBrush(new Point(x, y), new Point((int)x1, (int)y1), start, end);
                start = end;
                end = Color.FromArgb((int)(end.R + depth) / 20, (end.G + 10), (int)(end.B + depth) / 20);

                g.DrawLine(new Pen(linGrBrush, len / 10), x, pictureBox1.Height - y, (int)x1, pictureBox1.Height - (int)y1);

                if (depth > 1)
                {
                    DrowTree((int)x1, (int)y1, (int)(len * 0.6), deegre + 35, start, end, depth - 1);
                    DrowTree((int)x1, (int)y1, (int)(len * 0.6), deegre - 35, start, end, depth - 1);
                    DrowTree((int)x1, (int)y1, (int)(len * 0.6), deegre + 65, start, end, depth - 1);
                    DrowTree((int)x1, (int)y1, (int)(len * 0.6), deegre - 65, start, end, depth - 1);
                }
                // Отрисовка цветов
                if (depth == 1)
                {
                    int x_n = (int)(x1 - 1);
                    int y_n = (int)(y1 - 1);
                    // Переворот изображения на 180, иначе цветы перевернуты
                    int m = pictureBox1.Height / 2;
                    int div = m - y_n;
                    y_n = m + div;
                    g.FillEllipse(new SolidBrush(Color.Red), x_n, y_n, 2, 2);
                }
                return;
            }
            catch (Exception ex)
            {
                using (FileStream file = new FileStream(@"C:\Users\daria\OneDrive\Документы\2 курс\4 семестр\Программирование\Все лабораторные\AllLabs\exc.txt", FileMode.Append))
                using (StreamWriter write = new StreamWriter(file))
                {
                    write.WriteLine("* --------------------------------------------------------------------- *");
                    write.WriteLine(DateTime.Now.ToString() + " Лабораторная работа 3\n" + ex.ToString());
                }
                Data.Exct += DateTime.Now.ToString() + " Лабораторная работа 3 " + ex.ToString() + '\n';
            }
        }

        private void SerpinskyCarpet(int x, int y, int size, int step, int Red, int Green, int Blue, int A)
        {
            if (step != 0)
            {
                int x1 = x - size / 2;
                int x2 = x + size / 2;
                int y1 = y - size / 2;
                int y2 = y + size / 2;

                if (Red > 255) Red = 255;
                if (Blue > 255) Blue = 255;
                if (Green > 255) Green = 255;
                if (A > 255) A = 255;

                Pen pen = new Pen(Color.FromArgb(A, Red, Green, Blue), (int)numericUpDown1.Value);
                g.DrawRectangle(pen, x - size / 2, y - size / 2, size, size);

                SerpinskyCarpet(x1, y1, size / 2, step - 1, Red + 5, Green + 5, Blue + 5, A + 5);
                SerpinskyCarpet(x1, y2, size / 2, step - 1, Red + 5, Green + 5, Blue + 5, A + 5);
                SerpinskyCarpet(x2, y1, size / 2, step - 1, Red + 5, Green + 5, Blue + 5, A + 5);
                SerpinskyCarpet(x2, y2, size / 2, step - 1, Red + 5, Green + 5, Blue + 5, A + 5);
            }

        }

        private float ArithmElem(float start, float step, int end)
        {
            if (end == 1)
                return start;
            return ArithmElem(start, step, end - 1) + step;
        }

        private float SummArithm(float start, float step, int end)
        {
            if (end == 1)
                return start;
            return SummArithm(start, step, end - 1) + ArithmElem(start, step, end);
        }

        Pen pen = new Pen(Color.Black);
        Graphics g;
        // для 2 части задания
        float start; //начальное значение
        float step; // шаг прогрессии
        int end; // N-ый элемент прогрессии
        private void button1_Click(object sender, EventArgs e)
        {
            g = pictureBox1.CreateGraphics();
            pen.Width = (int)numericUpDown1.Value;

            switch(comboBox1.SelectedIndex)
            {
                case 0: pen.Color = Color.Blue; break;
                case 1: pen.Color = Color.Red; break;
                case 2: pen.Color = Color.Green; break;
            }

            switch (comboBox2.SelectedIndex)
            {
                case 0:
                    g.Clear(Color.White);
                    SerpinskyCarpet(pictureBox1.Width / 2, pictureBox1.Height / 2, ((pictureBox1.Height / 3) + (pictureBox1.Width / 3)) / 2, 
                        (int)numericUpDown2.Value, trackBar2.Value, trackBar3.Value, trackBar4.Value, trackBar1.Value);
                    break;
                // Сумма арифметической прогрессии    
                case 1:
                    try
                    { 
                        start = (float)numericUpDown5.Value;
                        step = (float)numericUpDown3.Value;
                        end = (int)numericUpDown4.Value;
                        
                        using (Graphics g = pictureBox1.CreateGraphics())
                        {
                            g.Clear(Color.White);
                            g.DrawString($"Сумма арифметической прогрессии от {start} до {end} элемента\nс шагом {step} равен: " + SummArithm(start, step, end).ToString(), 
                                new Font("Times New Roman", 14.0F), new SolidBrush(pen.Color), new Point(pictureBox1.Width / 4, 130));
                            g.Dispose();
                        }
                    }
                    catch (Exception ex)
                    {
                        using (FileStream file = new FileStream(@"C:\Users\daria\OneDrive\Документы\2 курс\4 семестр\Программирование\Все лабораторные\AllLabs\exc.txt", FileMode.Append))
                        using (StreamWriter write = new StreamWriter(file))
                        {
                            write.WriteLine("* --------------------------------------------------------------------- *");
                            write.WriteLine(DateTime.Now.ToString() + " Лабораторная работа 3\n" + ex.ToString());
                        }
                        Data.Exct += DateTime.Now.ToString() + " Лабораторная работа 3 " + ex.ToString() + '\n';
                    }

                    break;

                case 2:
                    g.Clear(Color.White);
                    if ((int)numericUpDown2.Value > 0)
                        DrowTree(pictureBox1.Width / 2, 0, pictureBox1.Height / 3, 0, Color.FromArgb(77, 34, 14), Color.FromArgb(75, 40, 10), (int)numericUpDown2.Value);
                    break;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {

        }
        // Очистка поля
        private void button2_Click(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
        }

        private void trackBar4_Scroll(object sender, EventArgs e)
        {

        }

        private void Form7_Resize(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
            this.button1_Click(sender, e);
        }
    }
}
