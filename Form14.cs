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


namespace ProjectFB
{
    public partial class Form14 : Form
    {
        private MySqlConnection databaseConnection() // เชื่อมต่อกับฐานข้อมูล
        {
            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=mor's_fruit_basket;charset=utf8;";
            MySqlConnection conn = new MySqlConnection(connectionString);
            return conn;
        }
        public Form14()
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

     
        private void Form14_Load(object sender, EventArgs e)
        {
            LoadDataIntoDataGridView();
            
        }
        
        private void LoadDataIntoDataGridView()
        {
            MySqlConnection conn = databaseConnection();
            conn.Open();
            MySqlCommand cmd;
            try
            {
                cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM history";
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            SearchHistory();
        }
        private void SearchHistory() //searchusername&tel
        {
            string searchText = textBox1.Text.Trim();

            if (string.IsNullOrEmpty(searchText))
            {
                LoadDataIntoDataGridView();
                return;
            }

            MySqlConnection conn = databaseConnection();
            conn.Open();
            MySqlCommand cmd;
            try
            {
                cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM history WHERE username LIKE @searchTextPattern OR tel LIKE @searchTextPattern";
                cmd.Parameters.AddWithValue("@searchTextPattern", "%" + searchText + "%");

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

    }
}
