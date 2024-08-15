using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectFB
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void Form5_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form6 form6 = new Form6(); //stock
            form6.Show();
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4(); //รหัสถูก
            form4.Show();
            this.Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form13 form13 = new Form13();
            form13.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form14 form14 = new Form14(); //ประวัติการซื้อ
            form14.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form15 form15 = new Form15(); //สรุปยอด
            form15.Show();
            this.Hide();
        }
    }
}
