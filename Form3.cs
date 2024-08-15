using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Text.RegularExpressions;


namespace ProjectFB
{
    public partial class Form3 : Form
    {
        private MySqlConnection databaseConnection() //เชื่อมต่อกับฐานข้อมูล 
        {
            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=mor's_fruit_basket;charset=utf8;";
            MySqlConnection conn = new MySqlConnection(connectionString);
            return conn;
        }
        public Form3()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
           
        }
       
        private void label1_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
            this.Hide();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }
        private void button1_Click(object sender, EventArgs e)
        {
            // เช็คข้อมูลในช่องที่ให้กรอก
            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(textBox3.Text) || string.IsNullOrEmpty(textBox4.Text) || string.IsNullOrEmpty(textBox6.Text) || string.IsNullOrEmpty(textBox7.Text) || string.IsNullOrEmpty(textBox8.Text) || string.IsNullOrEmpty(textBox10.Text) || string.IsNullOrEmpty(textBox11.Text) || string.IsNullOrEmpty(textBox12.Text) || string.IsNullOrEmpty(textBox13.Text))
            {
                MessageBox.Show("Please fill in all the required fields.", "Incomplete Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (!textBox2.Text.All(char.IsDigit) || textBox2.Text.Length != 10)
            {
                MessageBox.Show("Telephone number must be numeric and have 10 digits.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (!Regex.IsMatch(textBox12.Text, @"^(?=.*[a-zA-Z])(?=.*\d)[a-zA-Z\d]{8,}$"))
            {
                MessageBox.Show("Please enter password english numbers and letters must be at least 8 characters long", "Invalid Password", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            else
            {
                try
                {
                    MySqlConnection conn = databaseConnection();
                    conn.Open();

                    // เช็คความซ้ำซ้อนของเบอร์โทรศัพท์ (tel)
                    string checkDuplicateTelQuery = "SELECT COUNT(*) FROM register WHERE tel = @tel";
                    MySqlCommand checkDuplicateTelCmd = new MySqlCommand(checkDuplicateTelQuery, conn);
                    checkDuplicateTelCmd.Parameters.AddWithValue("@tel", textBox2.Text);
                    int telCount = Convert.ToInt32(checkDuplicateTelCmd.ExecuteScalar());

                    if (telCount > 0)
                    {
                        MessageBox.Show("This number has already been used for registration", "Duplicate Telephone Number", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // เช็คความซ้ำซ้อนของชื่อผู้ใช้ (username)
                    string checkDuplicateUsernameQuery = "SELECT COUNT(*) FROM register WHERE username = @username";
                    MySqlCommand checkDuplicateUsernameCmd = new MySqlCommand(checkDuplicateUsernameQuery, conn);
                    checkDuplicateUsernameCmd.Parameters.AddWithValue("@username", textBox13.Text);
                    int usernameCount = Convert.ToInt32(checkDuplicateUsernameCmd.ExecuteScalar());

                    if (usernameCount > 0)
                    {
                        MessageBox.Show("This username is already taken. Please choose another one.", "Duplicate Username", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // รวมช่องที่กรอกที่อยู่ให้อยู่ในช่องเดียวใน SQL
                    string address = string.Concat(textBox4.Text, " ", textBox8.Text, " ", textBox5.Text, "", textBox9.Text, " ", textBox6.Text, " ", textBox10.Text, " ", textBox7.Text, " ", textBox11.Text);

                    // เตรียมคำสั่ง SQL เพื่อเพิ่มข้อมูลผู้ใช้งาน
                    string sql = "INSERT INTO register (username, password, name, tel, e_mail, address) VALUES (@username, @password, @name, @tel, @e_mail, @address)";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@username", textBox13.Text);
                    cmd.Parameters.AddWithValue("@password", textBox12.Text);
                    cmd.Parameters.AddWithValue("@name", textBox1.Text);
                    cmd.Parameters.AddWithValue("@tel", textBox2.Text);
                    cmd.Parameters.AddWithValue("@e_mail", textBox3.Text);
                    cmd.Parameters.AddWithValue("@address", address);

                    int rows = cmd.ExecuteNonQuery(); // ระมวลผลคำสั่ง SQL และคืนค่า
                    conn.Close();
                }
                catch (Exception ex) // แสดงข้อผิดพลาด
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                Form2 form2 = new Form2();
                form2.Show();
                this.Hide();
            }
        }







        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox13_TextChanged(object sender, EventArgs e)
        {
            
        }

    }
}
