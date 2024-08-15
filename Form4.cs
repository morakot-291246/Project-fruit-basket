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
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }
        


        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string password = textBox1.Text; // รับค่ารหัสจาก TextBox
            if (password == "0000") // ตรวจสอบว่ารหัสถูกต้องหรือไม่
            {
                Form5 form5 = new Form5(); //รหัสถูก
                form5.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Incorrect password. Please enter a valid English password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox1.Clear(); // Clear the TextBox for re-entry of password
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }
    }
}
