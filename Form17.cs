using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PdfiumViewer;
using Newtonsoft.Json.Linq;

namespace ProjectFB
{
    public partial class Form17 : Form
    {
        private MySqlConnection databaseConnection() //เชื่อมต่อกับฐานข้อมูล 
        {
            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=mor's_fruit_basket;charset=utf8;";
            MySqlConnection conn = new MySqlConnection(connectionString);
            return conn;
        }
        public Form17()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            dataGridView1.CellClick += DataGridView1_CellClick;
        }
        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // ตรวจสอบว่าคลิกในแถวที่มีข้อมูล
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                int idOrder = Convert.ToInt32(row.Cells["idOrder"].Value); // แสดงเฉพาะคอลัม "idOrder"

                // ใช้ idOrder เพื่อค้นหาที่อยู่ของไฟล์ PDF จากฐานข้อมูล
                string bill = GetPdfFilePathFromDatabase(idOrder);

                if (!string.IsNullOrEmpty(bill))
                {
                    // เปิดไฟล์ PDF จากเครื่องผู้ใช้
                    LoadPdfFromFile(bill);
                }
                else
                {
                    MessageBox.Show("PDF file path not found!");
                }
            }
        }
        private string GetPdfFilePathFromDatabase(int idOrder)
        {
            string bill = string.Empty;
            MySqlConnection conn = databaseConnection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT bill FROM history WHERE idOrder = @idOrder";
            cmd.Parameters.AddWithValue("@idOrder", idOrder);

            try
            {
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    bill = result.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching PDF file path: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return bill;
        }
        private void LoadPdfFromFile(string filePath)
        {
            try
            {
                System.Diagnostics.Process.Start(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening PDF file: " + ex.Message);
            }
        }

        public void SetData(string username) //รับและกำหนดค่าที่ถูกส่งมา
        {
            label1.Text = username;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form17_Load(object sender, EventArgs e)
        {
            SearchHistoryByUsername(label1.Text);
            Sum();

            // กำหนดข้อความให้กับ Label 4 โดยใช้จำนวนแถวในตารางข้อมูล
            label4.Text = "ซื้อสินค้าทั้งหมด : " + dataGridView1.Rows.Count.ToString() + " ครั้ง";
        }

        private void Sum()
        {
            decimal totalSum = 0;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["sum"].Value != null)
                {
                    decimal sum;
                    if (decimal.TryParse(row.Cells["sum"].Value.ToString(), out sum))
                    {
                        totalSum += sum;
                    }
                }
            }

            label3.Text = totalSum.ToString("#,##0.00") + " Baht";
        }
        private void SearchHistoryByUsername(string username)
        {
            // สร้าง DataTable เพื่อเก็บข้อมูลที่ค้นหาได้
            DataTable dataTable = new DataTable();

            // เรียกใช้เมทอด databaseConnection เพื่อเชื่อมต่อกับฐานข้อมูล
            using (MySqlConnection conn = databaseConnection())
            {
                conn.Open();
                // สร้างคำสั่ง SQL สำหรับค้นหาข้อมูลในตาราง history
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM history WHERE username = @username", conn);
                cmd.Parameters.AddWithValue("@username", username);

                // สร้าง DataAdapter เพื่อเรียกข้อมูลจากฐานข้อมูล
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);

                // เติมข้อมูลจากฐานข้อมูลลงใน DataTable
                adapter.Fill(dataTable);
            }

            // กำหนดข้อมูลใน DataTable เป็นแหล่งข้อมูลของ DataGridView
            dataGridView1.DataSource = dataTable;
        }

        private void label2_Click(object sender, EventArgs e)
        {
            string username = label1.Text;
            Form16 form16 = new Form16();
            form16.SetData(username);
            form16.Show();
            this.Hide();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
