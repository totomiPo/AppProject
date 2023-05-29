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
    public partial class SizeHolst : Form
    {
        public SizeHolst()
        {
            InitializeComponent();
        }

        // Применить размер холста
        internal void button1_Click(object sender, EventArgs e)
        {
            int width = (int)numericUpDown1.Value;
            int height = (int)numericUpDown2.Value;
            Image im = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(im);

            g.Clear(Color.White);
            g.Dispose();

            PictureBox p = Owner.Controls["panel1"].Controls["pictureBox1"] as PictureBox;
            if (p.Image != null)
                p.Image.Dispose();
            p.Image = im;
        }

        private void SizeHolst_Load(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
