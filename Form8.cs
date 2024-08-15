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
namespace ProjectFB
{
    public partial class Form8 : Form
    {
        private MySqlConnection databaseConnection() // เชื่อมต่อกับฐานข้อมูล
        {
            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=mor's_fruit_basket;charset=utf8;";
            MySqlConnection conn = new MySqlConnection(connectionString);
            return conn;
        }

        public Form8()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }
        public void SetData(string username) //รับและกำหนดค่าที่ถูกส่งมา
        {
            textBox5.Text = username;
        }

        private void Form8_Load(object sender, EventArgs e)
        {
            Checkusername();
            
        }
        private void Checkusername()
        {
            string searchText = textBox5.Text; 

            // สร้าง DataTable เพื่อทำการค้นหา
            DataTable dataTable = new DataTable();

            // สร้าง DataColumn เพื่อทำการค้นหา
            DataColumn telColumn = new DataColumn("username");
            dataTable.Columns.Add(telColumn);

            // เพิ่มข้อมูลจากฐานข้อมูลลงใน DataTable ด้วยการ query ฐานข้อมูล
            using (MySqlConnection conn = databaseConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM register", conn);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dataTable);
            }

            DataRow[] foundRows = dataTable.Select("username = '" + searchText + "'"); // ค้นหาข้อมูลในคอลัมนี้

            // ตรวจสอบว่าพบข้อมูลหรือไม่
            if (foundRows.Length > 0)
            {
                // ถ้าพบข้อมูล ให้นำข้อมูลในแถวนั้นไปแสดงใน textbox อื่น ๆ

                textBox1.Text = foundRows[0]["name"].ToString();
                textBox2.Text = foundRows[0]["tel"].ToString();
                textBox3.Text = foundRows[0]["e_mail"].ToString();
                textBox4.Text = foundRows[0]["address"].ToString();
                textBox6.Text = foundRows[0]["password"].ToString();

            }
           
        }
       

        private void label1_Click(object sender, EventArgs e)
        {
            string username = textBox5.Text;
            Form16 form16 = new Form16();
            form16.SetData(username);
            form16.Show();
            this.Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e) //แก้ไขข้อมูลสมาชิกลงใน sql
        {
            string name = textBox1.Text;
            string tel = textBox2.Text;
            string email = textBox3.Text;
            string address = textBox4.Text;
            string username = textBox5.Text;
            string password = textBox6.Text;
            //เช็คว่ามีข้อมูลมั๊ย
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(tel) && !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(address) && !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                using (MySqlConnection conn = databaseConnection())
                {
                    try
                    {
                        conn.Open();
                        MySqlCommand cmd = conn.CreateCommand();

                        // สร้างคำสั่ง SQL เพื่ออัปเดตข้อมูลในแถวที่มีเบอร์โทรศัพท์ตรงกับที่ระบุ
                        cmd.CommandText = "UPDATE register SET name = @name, tel = @tel, e_mail = @email, address = @address, username = @username, password = @password " +
                                          "WHERE username = @username"; // เพิ่ม WHERE เพื่อกำหนดเงื่อนไขให้เฉพาะแถวที่ตรงกับเบอร์โทรศัพท์ที่ระบุ
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@tel", tel);
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@address", address);
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);


                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Data updated successfully.");
                        }
                        else
                        {
                            MessageBox.Show("No data updated.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please fill in all fields.");
            }
        }
        private void textBox2_TextChanged(object sender, EventArgs e) //ค้นหาข้อมูลเบอร์จาก sql แล้วนำมาแสดงใน textbox
        {
           
        }

       
        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            Checkusername();
        }
    }
}
