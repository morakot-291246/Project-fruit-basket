using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace ProjectFB
{
    public partial class Form13 : Form
    {
        private MySqlConnection databaseConnection() // เชื่อมต่อกับฐานข้อมูล
        {
            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=mor's_fruit_basket;charset=utf8;";
            MySqlConnection conn = new MySqlConnection(connectionString);
            return conn;
        }

        public Form13()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
           
        }

        
        private void LoadDataIntoDataGridView()
        {
            MySqlConnection conn = databaseConnection();
            conn.Open();
            MySqlCommand cmd;
            try
            {
                cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM register";
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

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Form13_Load(object sender, EventArgs e)
        {
            LoadDataIntoDataGridView();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {
            Form5 form5 = new Form5();
            form5.Show();
            this.Hide();
        }
    }
}
