using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AllLabs
{
    public partial class Draw : Form
    {
        // Режим рисования
        private int mode;
        private Point movePt;
        private Point nullPt = new Point(int.MaxValue, 0);

        private SolidBrush brush = new SolidBrush(Color.White);
        private Pen pen = new Pen(Color.Black);
        // Начальная точка с которой началась рисоваться линия
        private Point startPt;

        int k1, k2; 
        int A = 50;
        int F = 1;
        int nterms = 1;
        private SizeHolst makePanel = new SizeHolst();

        public Draw()
        {
            InitializeComponent();
            AddOwnedForm(makePanel);
            openFileDialog1.InitialDirectory = saveFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();
            
            makePanel.button1_Click(this, null);
            // Прорисовка плавной закругленной линии
            pen.StartCap = pen.EndCap = LineCap.Round;
            // Инверсивный контур показывает настоящие границы фигуры
            pen.Alignment = PenAlignment.Inset;
        }

        public Draw(Form1 f)
        {
            InitializeComponent();
            AddOwnedForm(makePanel);
            openFileDialog1.InitialDirectory = saveFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();
            
            makePanel.button1_Click(this, null);
            pen.StartCap = pen.EndCap = LineCap.Round;
            pen.Alignment = PenAlignment.Inset;
        }

        /*----------------------------------------Активные кнопки---------------------------------------------------*/

        // Открытие файла
        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Файлы изображений (*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png";
            openFileDialog1.FileName = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string s = openFileDialog1.FileName;
                try
                {
                    Image img = new Bitmap(s);
                    Graphics g = Graphics.FromImage(img);
                    g.Dispose();
                    if (pictureBox1.Image != null)
                        pictureBox1.Image.Dispose();
                    pictureBox1.Image = img;
                }
                catch
                {
                    MessageBox.Show("File " + s + " has a wrong format.", "Error");
                    return;
                }
                Text = "Image Editor - " + s;
                saveFileDialog1.FileName = Path.ChangeExtension(s, "png");
            }
        }

        // Сохранение изображения
        private void button3_Click(object sender, EventArgs e)
        {
            string s0 = saveFileDialog1.FileName;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string s = saveFileDialog1.FileName;
                if (s.ToUpper() == s0.ToUpper())
                {
                    s0 = Path.GetDirectoryName(s0);
                    pictureBox1.Image.Save(s0);
                    pictureBox1.Image.Dispose();
                    File.Delete(s);
                    File.Move(s0, s);
                    pictureBox1.Image = new Bitmap(s);
                }
                else
                    pictureBox1.Image.Save(s);
                Text = "Image Editor - " + s;
            }
        }

        // Очистка холста
        private void button4_Click(object sender, EventArgs e)
        {
            Graphics g = Graphics.FromImage(pictureBox1.Image);
            g.Clear(Color.White);
            pictureBox1.Invalidate();
        }

        // Создание холста с заданными шириной и длиной
        private void button1_Click(object sender, EventArgs e)
        {
            makePanel.ActiveControl = makePanel.numericUpDown1;
            if (makePanel.ShowDialog() == DialogResult.OK)
            {
                saveFileDialog1.FileName = "";
                Text = "Image Editor";
            }
        }

        // Изменение ширины кисти
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            pen.Width = (int)numericUpDown1.Value;
        }

        /*----------------------------------------Цвет кисти---------------------------------------------------*/

        // Изменение цвета линии
        private void label5_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = label5.BackColor;
            // Открытие палитры
            if (colorDialog1.ShowDialog() == DialogResult.OK)
                label5.BackColor = colorDialog1.Color;
        }

        // Изменение цвета кисти
        private void label5_BackColorChanged(object sender, EventArgs e)
        {
            pen.Color = label5.BackColor;
        }

        private void label6_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = label6.BackColor;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
                label6.BackColor = colorDialog1.Color;
        }
        // Заливка
        private void label6_BackColorChanged(object sender, EventArgs e)
        {
            brush.Color = label6.BackColor;
        }

        /*----------------------------------------Тип кистей---------------------------------------------------*/

        // Инверсируем цвет линии по отношению цвета под ней
        private void ReversibleDraw()
        {
            Point p1 = pictureBox1.PointToScreen(startPt),
                  p2 = pictureBox1.PointToScreen(movePt);
            if (mode == 1)
                ControlPaint.DrawReversibleLine(p1, p2, Color.Black);
            else
                ControlPaint.DrawReversibleFrame(PtToRect(p1, p2), Color.Black, FrameStyle.Thick);
        }

        private Rectangle PtToRect(Point p1, Point p2)
        {
            int x = Math.Min(p1.X, p2.X);
            int y = Math.Min(p1.Y, p2.Y);

            int width = Math.Abs(p2.X - p1.X);
            int height = Math.Abs(p2.Y - p1.Y);

            return new Rectangle(x, y, width, height);
        }

        private void DrawFigure(Rectangle r, Graphics g)
        {
            // Заливка прямоугольника
            g.FillRectangle(brush, r);
            // Обводка прямоугольника
            g.DrawRectangle(pen, r);
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            movePt = startPt = e.Location;
        }

        // Прорисовка линии при зажатой левой кнопке
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            // Если мышь не перемещалась
            if (startPt == nullPt)
                return;
            // Если зажата левая кнопка
            if (e.Button == MouseButtons.Left)
                switch (mode)
                {
                    // Выбор карандаша
                    case 0:
                        Graphics g = Graphics.FromImage(pictureBox1.Image);
                        g.DrawLine(pen, startPt, e.Location);
                        g.Dispose();
                        startPt = e.Location;
                        pictureBox1.Invalidate();
                        break;
                    // Выбор линии
                    case 1:
                        ReversibleDraw(); // Избегаю прорисовки за формой
                        movePt = e.Location;
                        ReversibleDraw();
                        break;
                    // Выбор прямоугольника
                    case 2:
                        ReversibleDraw();
                        movePt = e.Location;
                        ReversibleDraw();
                        break;
                }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (startPt == nullPt)
                return;
            if (mode > 0)
            {
                ReversibleDraw();
                Graphics g = Graphics.FromImage(pictureBox1.Image);
                switch (mode)
                {
                    case 1:
                        g.DrawLine(pen, startPt, movePt);
                        break;
                    case 2:
                        DrawFigure(PtToRect(startPt, movePt), g);
                        break;
                }
                g.Dispose();
                pictureBox1.Invalidate();
            }

        }

        // Установка режима в зависимости от выбранной кнопки (зависимость от radioButton1_CheckedChanged)
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton type = sender as RadioButton;
            // Mode == индекс активного режима
            mode = type.TabIndex;
        }

        /*----------------------------------------Фурье---------------------------------------------------*/

        // Прорисовка системы координат
        private void DrawAxis()
        {
            Graphics g = Graphics.FromImage(pictureBox1.Image);

            g.DrawLine(new Pen(Color.Black, 1), 0, pictureBox1.Height / 2, pictureBox1.Width, pictureBox1.Height / 2);
            g.DrawLine(new Pen(Color.Black, 1), pictureBox1.Width / 2, pictureBox1.Height, pictureBox1.Width / 2, 0);

            // Засечки - величина амплитуды (А)
            g.DrawLine(new Pen(Color.Red, 2), 0, ((int)pictureBox1.Height / 2) - A, 1 + (int)(pictureBox1.Width / 100), ((int)pictureBox1.Height / 2) - A);
            g.DrawLine(new Pen(Color.Red, 2), 0, ((int)pictureBox1.Height / 2) + A, 1 + (int)(pictureBox1.Width / 100), ((int)pictureBox1.Height / 2) + A);

            // Засечки - период
            for (int i = 0; i < pictureBox1.Width; i += 180)
            {
                g.DrawLine(new Pen(Color.Red, 2), i, (int)(((int)pictureBox1.Height / 2) * 0.97), i, (int)(((int)pictureBox1.Height / 2) * 1.03));
            }
        }

        private void redrawFourier()
        {
            try
            {
                Graphics g = Graphics.FromImage(pictureBox1.Image);
                g.Clear(Color.White);

                DrawAxis();

                int Interval = pictureBox1.Width;
                double yp = 0, yy1 = 0, yy2 = 0;
                int angle = 0;
                int xtemp = 0;
                int ytemp = pictureBox1.Height / 2;



                for (int i = 0; i < Interval; i++)
                {
                    if ((int)comboBox1.SelectedIndex == 1)
                    {
                        for (int j = 1; j <= nterms; j++)
                        {
                            yy1 = A / (((2 * j) - 1) * ((2 * j) - 1));
                            double arg = (j * 2 - 1) * angle * F * (Math.PI / 180);
                            yy2 = Math.Cos(arg);
                            yp = yp + yy1 * yy2;
                        }
                    }
                    else
                    {
                        for (int j = 1; j < nterms; j++)
                        {
                            yy1 = A / ((k1 * j) + k2);
                            double arg = ((j * k1) + k2) * F * 0.01397 * angle;
                            yy2 = Math.Sin(arg);
                            yp = yp + yy1 * yy2;
                        }
                    }

                    g.DrawLine(new Pen(label5.BackColor, (int)numericUpDown1.Value), xtemp, ytemp, i, pictureBox1.Height / 2 + (int)Math.Truncate(yp));
                    xtemp = i;
                    ytemp = pictureBox1.Height / 2 + (int)Math.Truncate(yp);
                    yp = 0;
                    angle++;
                }
                DrawAxis();
                g.Dispose();
                pictureBox1.Invalidate();
            } catch (Exception ex)
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch ((int)comboBox1.SelectedIndex)
            {
                // Пилообразный
                case 0:
                    k1 = 1;
                    k2 = 0;
                    break;
                // Прямоугольный
                case 2:
                    k1 = 2;
                    k2 = -1;
                    break;
            }
        }

        private void Draw_Resize(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
        }


        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            nterms = (int)numericUpDown2.Value;
            redrawFourier();
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            A = (int)numericUpDown3.Value;
            redrawFourier();
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            F = (int)numericUpDown4.Value;
            redrawFourier();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            redrawFourier();
        }

        private void Draw_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
        }
    }
}
