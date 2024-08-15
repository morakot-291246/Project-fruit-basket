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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }
        
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form7 form7 = new Form7(); //หน้าShop
            form7.Show();
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {

            Form1 form1 = new Form1(); //หน้าแรก
            form1.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3(); //สมัครสมาชิก
            form3.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form8 form8 = new Form8();  //ข้อมูลสมาชิก
            form8.Show();
            this.Hide();
        }
    }
}
