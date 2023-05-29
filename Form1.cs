using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AllLabs
{
    public partial class Form1 : Form
    {
        // Отображение
        ToolStripLabel dateLabel;
        ToolStripLabel timeLabel;
        ToolStripLabel infoLabel;
        Timer timer;

        void timerTik(object sender, EventArgs e)
        {
            dateLabel.Text = DateTime.Now.ToLongDateString();
            timeLabel.Text = DateTime.Now.ToLongTimeString();
        }

        public Form1()
        {
            InitializeComponent();
            timer = new Timer() { Interval = 1000 };
            timer.Start();

            infoLabel = new ToolStripLabel();
            infoLabel.Text = "Время: ";
            dateLabel = new ToolStripLabel();
            timeLabel = new ToolStripLabel();

            statusStrip1.Items.Add(infoLabel);
            statusStrip1.Items.Add(dateLabel); 
            statusStrip1.Items.Add(timeLabel);
            
            timer.Tick += timerTik;
        }

        private void обАвтореToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Дарья Дубровина, гр. 3132");
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "Text files(*, txt)|*.txt|All files(*.*)|*.*";
            // Если не нажата "Отмена"
            if (saveFileDialog1.ShowDialog() != DialogResult.Cancel)
            {
                string filename = saveFileDialog1.FileName;
                System.IO.File.WriteAllText(filename, textBox1.Text);
                MessageBox.Show("Файл сохранен");
            }
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Text files(*, txt)|*.txt|All files(*.*)|*.*";
            if (openFileDialog1.ShowDialog() != DialogResult.Cancel)
            {
                string filename = openFileDialog1.FileName;
                string fileText = System.IO.File.ReadAllText(filename);
                textBox1.Text = fileText;
                MessageBox.Show("Файл открыт");
            }
        }

        private void создатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
        }

        private void myNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // this - ссылка на текущий объект
            Form2 newForm = new Form2(this);
            newForm.ShowDialog();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Form3 newForm = new Form3(this);
            newForm.ShowDialog();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Form4 newForm = new Form4(this);
            newForm.ShowDialog();
        }

        // Лаба 2
        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            Form5 newForm = new Form5(this);
            newForm.ShowDialog();
            // Вывод ошибок с Формы 5 на главную Форму
            textBox1.Text = Data.Exct;
        }
        // Доп лаба 2
        private void допToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form6 newForm = new Form6(this);
            newForm.ShowDialog();
        }
        private void лР2ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        
        private void лР3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void labsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }
        // Лаба 3
        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            Form7 newForm = new Form7(this);
            newForm.ShowDialog();
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            Draw newForm = new Draw(this);
            newForm.ShowDialog();
        }

        private void лР4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form8 newForm = new Form8(this);
            newForm.Show();
        }

        private void распарситьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string s = File.ReadAllText("../../exc.txt");
            textBox1.Text = s;

            string result = "";

            string[] signs = { "Лабораторная работа 1", "Лабораторная работа 2", "Лабораторная работа 3", "Лабораторная работа 4" };
            for (int i = 0; i < signs.Length; i++)
            {
                // парсим для каждой сигнатуры, выделяем в тексте, считаем вхождения, хаписываем в резулт
                Regex regex = new Regex(signs[i]);
                MatchCollection matches = Regex.Matches(s, signs[i], RegexOptions.IgnoreCase);

                foreach (Match match in matches)
                {
                    int p = match.Index;
                    textBox1.Find(signs[i], p, RichTextBoxFinds.None);
                    textBox1.SelectionColor = Color.FromArgb(255, 0, 0);
                }

                result += $"Вхождений \"{signs[i]}\" : {matches.Count}\n\n";
            }
            MessageBox.Show(result);
        }
    }
}
