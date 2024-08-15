using MySql.Data.MySqlClient;
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
    public partial class Form16 : Form
    {
        private MySqlConnection databaseConnection() //เชื่อมต่อกับฐานข้อมูล 
        {
            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=mor's_fruit_basket;charset=utf8;";
            MySqlConnection conn = new MySqlConnection(connectionString);
            return conn;
        }
        public Form16()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }
        public void SetData(string username) //รับและกำหนดค่าที่ถูกส่งมา
        {
            label2.Text = username;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = label2.Text;
            // สร้าง instance ของ Form10
            Form9 form9 = new Form9();

            // ส่งข้อมูลไปยัง Form10
            form9.SetData(username);

            // แสดง Form10
            form9.Show();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string username = label2.Text;
            // สร้าง instance ของ Form10
            Form17 form17 = new Form17();

            // ส่งข้อมูลไปยัง Form10
            form17.SetData(username);

            // แสดง Form10
            form17.Show();
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Form16_Load(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {
            // เรียกใช้เมธอดเพื่อลบข้อมูลในตาราง "bill"
            ClearBill();

            // สร้างและแสดง Form1
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }

        private void ClearBill()
        {
            MySqlConnection conn = databaseConnection(); // เชื่อมต่อกับฐานข้อมูล

            try
            {
                conn.Open(); // เปิดการเชื่อมต่อ

                // สร้างคำสั่ง SQL เพื่อลบข้อมูลในตาราง "bill"
                string query = "DELETE FROM bill";
                MySqlCommand cmd = new MySqlCommand(query, conn);

                // ประมวลผลคำสั่ง SQL
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                MessageBox.Show("เกิดข้อผิดพลาดในการลบข้อมูลในตาราง 'bill': " + ex.Message);
            }
            finally
            {
                conn.Close(); // ปิดการเชื่อมต่อ
            }
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string username = label2.Text;
            // สร้าง instance ของ Form10
            Form8 form8 = new Form8();

            // ส่งข้อมูลไปยัง Form10
            form8.SetData(username);

            // แสดง Form10
            form8.Show();
            this.Close();
        }
    }
}
