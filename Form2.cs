using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AllLabs
{
    public partial class Form2 : Form
    {
        // Конструктор по умолчанию
        public Form2()
        {
            // Вызов метода LoadComponent() - использует его для построения пользовательского интерфейса
            InitializeComponent();
        }

        // Получаем первую форму
        public Form2(Form1 f)
        {
            InitializeComponent();
        }

        public bool ErrorName(string name)
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

        private void button1_Click(object sender, EventArgs e)
        {
            string firstName = textBox1.Text;
            string secondName = textBox2.Text;

            if (ErrorName(firstName))
            {
                if (ErrorName(secondName))
                {
                    byte[] bytesFName = Encoding.GetEncoding(1251).GetBytes(firstName);
                    byte[] bytesSName = Encoding.GetEncoding(1251).GetBytes(secondName);

                    int lenMin = Math.Min(firstName.Length, secondName.Length);
                    int lenMax = Math.Max(firstName.Length, secondName.Length);
                    byte[] newStr = new byte[lenMax];

                    for (int i = 0; i < lenMax; ++i)
                    {
                        if (i < lenMin)
                        {
                            newStr[i] = (byte)(bytesFName[i] | bytesSName[i]);
                            label3.Text += $"{firstName[i]} + {secondName[i]} = {newStr[i]} \n";
                        }
                        else
                        {
                            if (firstName.Length > secondName.Length)
                            {
                                newStr[i] = bytesFName[i];
                                label3.Text += $"{firstName[i]} = {newStr[i]} \n";
                            }
                            else
                            {
                                newStr[i] = bytesSName[i];
                                label3.Text += $"{secondName[i]} = {newStr[i]} \n";
                            }
                        }
                    }

                    string result = Encoding.GetEncoding(1251).GetString(newStr);
                    label5.Text = result;
                }
                else
                {
                    label7.Text = "Следует ввести строку!";
                    textBox2.Text = "";
                }
            }
            else
            {
                label6.Text = "Следует ввести строку!";
                textBox1.Text = "";
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
