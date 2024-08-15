using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;


namespace ProjectFB
{
    public partial class Form7 : Form
    {
        private MySqlConnection databaseConnection() //เชื่อมต่อกับฐานข้อมูล 
        {
            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=mor's_fruit_basket;charset=utf8;";
            MySqlConnection conn = new MySqlConnection(connectionString);
            return conn;
        }
        private string telNumber;
        public Form7()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            
        }
        
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2(); //รหัสถูก
            form2.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                if (CheckIfPhoneAndPasswordMatch(username, password))
                {
                    // เบอร์โทรศัพท์และรหัสผ่านตรงกัน
                    telNumber = textBox1.Text;
                    Form16 form16 = new Form16();
                    form16.SetData(username);
                    form16.Show();
                    this.Hide();
                    
                }
                else
                {
                    // เบอร์โทรศัพท์หรือรหัสผ่านไม่ตรงกัน
                    MessageBox.Show("Phone number or password incorrect.");
                }
            }
            else
            {
                // แจ้งเตือนให้กรอกข้อมูลให้ครบถ้วน
                MessageBox.Show("Please enter phone number and password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private bool CheckIfPhoneAndPasswordMatch(string username, string password)
        {
            bool match = false;
            using (MySqlConnection conn = databaseConnection())
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT COUNT(*) FROM register WHERE username = @username AND password = @password";
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    if (count > 0)
                    {
                        match = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
            return match;
        }

        private void label4_Click(object sender, EventArgs e)
        {
            // เช็คว่า TextBox2.UseSystemPasswordChar เป็น True หรือไม่
            if (textBox2.UseSystemPasswordChar)
            {
                // กำหนดให้ TextBox2.UseSystemPasswordChar เป็น False
                textBox2.UseSystemPasswordChar = false;
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Form18 form18 = new Form18();
            form18.Show();
            this.Hide();
        }
    }
}
