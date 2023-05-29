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
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        public Form4(Form1 f)
        {
            InitializeComponent();
        }

        public bool IsNumber(string number)
        {
            if (int.TryParse(number, out int result))
                return true;
            else
                return false;
        }

        public int NumberOnLeft(int number)
        {
            if (number < 10)
                return -1;

            while (number >= 100)
            {
                number = number / 10;
            }
            int answer = number % 10;
            return answer;
        }

        public string Divisor(int div, int number)
        {
            if (div == 0)
                return "На 0 делить нельзя!";
            if (number % div == 0)
            {
                int resDiv = number / div;
                return $"Кратно {div}, число {number} = {div} * {resDiv}\n";
            }
            else
                return $"Не кратно {div}\n";
        }

        public bool IsPrimeNumber(long number)
        {
            if (number <= 1)
                return false;
            if (number == 2 || number == 3)
                return true;
            if (number % 2 == 0 || number % 3 == 0)
                return false;
            for (int i = 5; i <= (long)(Math.Sqrt(number) + 0.5); i += 6)
                if (number % i == 0)
                    return false;
            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string numberStr = textBox1.Text;
            if (IsNumber(numberStr))
            {
                int number = Convert.ToInt32(numberStr);
                if (number <= 0)
                {
                    label7.Text = "Число должно быть\nнатуральным, больше нуля!";
                    textBox1.Text = "";
                }

                if (IsPrimeNumber(number))
                    label11.Text = "Простое";
                else
                    label11.Text = "Составное";

                if (NumberOnLeft(number) == -1)
                    label6.Text = "Число однозначное";
                else
                    label6.Text = NumberOnLeft(number).ToString();

                string strA = textBox2.Text;
                if (IsNumber(strA))
                {
                    int divA = Convert.ToInt32(strA);
                    if (IsPrimeNumber(divA))
                        label12.Text = "Простое";
                    else
                        label12.Text = "Составное";
                    label8.Text = Divisor(divA, number).ToString();
                }
                else
                {
                    textBox2.Text = "";
                    label8.Text = "Ведите число!";
                }

                string strB = textBox3.Text;
                if (IsNumber(strB))
                {
                    int divB = Convert.ToInt32(strB);
                    if (IsPrimeNumber(divB))
                        label13.Text = "Простое";
                    else
                        label13.Text = "Составное";
                    label9.Text = Divisor(divB, number).ToString();
                }
                else
                {
                    textBox3.Text = "";
                    label9.Text = "Ведите число!";
                }

                string strC = textBox4.Text;
                if (IsNumber(strC))
                {
                    int divC = Convert.ToInt32(strC);
                    if (IsPrimeNumber(divC))
                        label14.Text = "Простое";
                    else
                        label14.Text = "Составное";
                    label10.Text = Divisor(divC, number).ToString();
                }
                else
                {
                    textBox4.Text = "";
                    label10.Text = "Ведите число!";
                }
            }
            else
            {
                textBox1.Text = "";
                label7.Text = "Ведите число!";
                label6.Text = "";
            }
        }

        private void Form4_Load(object sender, EventArgs e)
        {

        }
    }
}
