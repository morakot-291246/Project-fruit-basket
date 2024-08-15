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
    public partial class Form15 : Form
    {
        private MySqlConnection databaseConnection() // เชื่อมต่อกับฐานข้อมูล
        {
            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=mor's_fruit_basket;charset=utf8;";
            MySqlConnection conn = new MySqlConnection(connectionString);
            return conn;
        }
        public Form15()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            label1_Click(sender, e);
            label2_Click(sender, e);
            ShowSelectedDateHistory();
        }


        private void label1_Click(object sender, EventArgs e)
        {
            string connectionString = "server=localhost;user id=root;password=;database=mor's_fruit_basket;";
            string query = "SELECT SUM(REPLACE(sum, ',', '')) FROM history WHERE DATE(date) = @selectDate";

            using (var connection = new MySqlConnection(connectionString))
            {
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@selectDate", dateTimePicker1.Value.Date.ToString("yyyy-MM-dd"));
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        // แสดงข้อมูลโดยเพิ่มเครื่องหมาย ',' ระหว่างจำนวนทุก 3 ตัว
                        label1.Text = Convert.ToDecimal(result).ToString("#,##0.00") + " บาท";
                    }
                    else
                    {
                        label1.Text = "0 บาท";
                    }
                }
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            string connectionString = "server=localhost;user id=root;password=;database=mor's_fruit_basket;";
            string query = "SELECT SUM(REPLACE(sum, ',', '')) FROM history WHERE YEAR(date) = YEAR(@selectDate) AND MONTH(date) = MONTH(@selectDate)";

            using (var connection = new MySqlConnection(connectionString))
            {
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@selectDate", dateTimePicker1.Value.Date.ToString("yyyy-MM-dd"));
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        // แสดงข้อมูลโดยเพิ่มเครื่องหมาย ',' ระหว่างจำนวนทุก 3 ตัว
                        label2.Text = Convert.ToDecimal(result).ToString("#,##0.00") + " บาท";
                    }
                    else
                    {
                        label2.Text = "0 บาท";
                    }
                }
            }
        }

        private void ShowSelectedDateHistory() //ดึงข้อมูลประวัติจากฐานข้อมูลโดยค้นหาด้วยวันที่ที่ผู้ใช้เลือกจาก DateTimePicker และแสดงผลใน DataGridView สำหรับการแสดงประวัติที่มีวันที่ตรงกับที่เลือกไว้
        {
            string selectedDate = dateTimePicker1.Value.Date.ToString("yyyy-MM-dd");
            string connectionString = "server=localhost;user id=root;password=;database=mor's_fruit_basket;";
            string query = "SELECT * FROM history WHERE DATE(date) = @selectedDate";

            using (var connection = new MySqlConnection(connectionString))
            {
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@selectedDate", selectedDate);
                    connection.Open();

                    DataTable dataTable = new DataTable();
                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    adapter.Fill(dataTable);

                    dataGridView1.DataSource = dataTable;
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {
            Form5 form5 = new Form5();
            form5.Show();
            this.Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }



}
