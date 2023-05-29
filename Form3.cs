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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        public Form3(Form1 f)
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int number = 10; number < 100; number++)
            {
                int numberX = number;
                int numberY = number;

                if (((numberX * 2) % 10 == 8) || ((numberY * 3) % 10 == 4))
                {
                    label2.Text += number.ToString() + " ";
                }

            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
