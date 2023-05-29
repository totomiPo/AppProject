using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AllLabs
{
    public partial class Form6 : Form
    {
        public Form6()
        {
            InitializeComponent();
        }
        public Form6(Form1 f)
        {
            InitializeComponent();
        }

        public bool ErrorText(string name)
        {
            if (String.IsNullOrEmpty(name))
                return false;
            else if (name.Length > 1500)
                return false;
            else
            {
                foreach (var letter in name)
                {
                    if (!char.IsLetter(letter))
                        return false;
                }
            }
            return true;
        }

        public string CylinderJeffersonaEncode(string name, StringBuilder[] cylinder, int shift)
        {
            string result = "";
            for (int i = 0; i < name.Length; i++)
            {
                for (int j = 0; j < 26; j++)
                {
                    if (name[i] == cylinder[i][j])
                        result += cylinder[i][(j + shift) % 26];
                }
            }
            return result;
        }

        public string CylinderJeffersonaDencode(string name, StringBuilder[] cylinder, int shift)
        {
            string result = "";
            for (int i = 0; i < name.Length; i++)
            {
                for (int j = 0; j < 26; j++)
                {
                    if (name[i] == cylinder[i][j])
                    {
                        int r = (j - shift) % 26;
                        r = (r < 0) ? r + 26 : r;
                        result += (j - 1 < 0) ? cylinder[i][25] : cylinder[i][r];
                    }
                }
            }
            return result;
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            numericUpDown1.Value = 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ErrorText(textBox1.Text))
            {
                richTextBox1.Clear();
                label2.Text = "";
                int shift = 1;
                // Заданный сдвиг
                shift = ((int)numericUpDown1.Value > 1) ? (int)numericUpDown1.Value : 1;
                string text = textBox1.Text.ToLower();
                string alphabet = "qwertyuiopasdfghjklzxcvbnm";
                StringBuilder[] cylinder = new StringBuilder[36]; // 36 дисков цилиндра

                Random random = new Random();
                for (int i = 0; i < 36; i++)
                {
                    cylinder[i] = new StringBuilder(alphabet);
                    // Тасование Фишера-Йетса
                    for (int k = 0; k < 26; k++)
                    {
                        int j = random.Next(k + 1);
                        char temp = cylinder[i][j];
                        cylinder[i][j] = cylinder[i][k];
                        cylinder[i][k] = temp;
                    }
                    richTextBox1.Text += $"Disk {i + 1} [ " + cylinder[i].ToString() + " ]\n";
                }

                string encName = CylinderJeffersonaEncode(text, cylinder, shift);
                string decName = CylinderJeffersonaDencode(encName, cylinder, shift);
                label6.Text = encName;
                label7.Text = decName;
            }
            else
            {
                label2.Text = "Вы ввели не строку";
            }
        }
    }
}
