using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace ProjectFB
{
    public partial class Form6 : Form //stock
    {
        private MySqlConnection databaseConnection() //เชื่อมต่อฐานข้อมูล
        {
            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=mor's_fruit_basket;charset=utf8;";
            MySqlConnection conn = new MySqlConnection(connectionString);
            return conn;
        }
        public Form6()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
           
        }
        

        private void Form6_Load(object sender, EventArgs e)
        {
            LoadDataIntoDataGridView();
            dataGridView1.CellClick += DataGridView1_CellClick;
        }
        private void LoadDataIntoDataGridView() //โหลดข้อมูลจากฐานข้อมูล มาแสดงใน DataGridView
        {
            MySqlConnection conn = databaseConnection();
            conn.Open();
            MySqlCommand cmd;
            try
            {
                cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM stock";
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

        private void label1_Click(object sender, EventArgs e)
        {
            Form5 form5 = new Form5(); //เมนูแอดมิน
            form5.Show();
            this.Hide();
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

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            // Set filter options and filter index.
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            openFileDialog1.FilterIndex = 1;

            openFileDialog1.Multiselect = false; // Allow only one file to be selected

            // Call the ShowDialog method to show the dialog box.
            DialogResult result = openFileDialog1.ShowDialog();

            // Process input if the user clicked OK.
            if (result == DialogResult.OK)
            {
                // Open the selected file to read.
                string file = openFileDialog1.FileName;
                // Display the image in the PictureBox.
                pictureBox2.Image = Image.FromFile(file);
            }
        }


        private void button1_Click(object sender, EventArgs e) // Add
        {
            //เก็บค่าไว้ในตัวแปร
            string code = textBox1.Text;
            string name = textBox2.Text;
            string quantity = textBox3.Text;
            string price = textBox4.Text;
            //ตัวสอบค่าว่าง
            if (code != "" && name != "" && quantity != "" && price != "")
            {
                // แปลงรูปภาพ
                byte[] imageBytes = ImageToByteArray(pictureBox2.Image);

                MySqlConnection conn = databaseConnection();
                conn.Open();
                MySqlCommand cmd;
                try
                {
                    //ตรวจสอบว่ารหัสสินค้าที่ป้อนมีอยู่ในฐานข้อมูลแล้วหรือไม่ เพื่อนับจำนวนรายการที่มีรหัสเดียวกับที่ป้อน.
                    cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT COUNT(*) FROM stock WHERE code = @code";
                    cmd.Parameters.AddWithValue("@code", code);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    //ถ้ามีรหัสสินค้านี้อยู่ในฐานข้อมูลแล้วจะแสดง MessageBox 
                    if (count > 0)
                    {
                        MessageBox.Show("Code already exists in the database.");
                    }
                    else
                    {
                        
                        cmd = conn.CreateCommand();
                        cmd.CommandText = "INSERT INTO stock (code, name, quantity, price, photo) VALUES (@code, @name, @quantity, @price, @photo)";
                        cmd.Parameters.AddWithValue("@code", code);
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@quantity", quantity);
                        cmd.Parameters.AddWithValue("@price", price);
                        cmd.Parameters.AddWithValue("@photo", imageBytes);
                        cmd.ExecuteNonQuery();
                   

                        //แสดงข้อมูลที่เพิ่ม
                        LoadDataIntoDataGridView(); 
                        
                        textBox1.Text = "";
                        textBox2.Text = "";
                        textBox3.Text = "";
                        textBox4.Text = "";

                        pictureBox2.Image = null;
                    }
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
            else
            {
               

                MessageBox.Show("Please fill in all fields.");
            }

        }

        // แปลงรูปภาพ สามารถใช้เก็บไว้ในฐานข้อมูล
        private byte[] ImageToByteArray(Image image)
        {
            MemoryStream ms = new MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            string code = textBox1.Text;
            string name = textBox2.Text;
            string quantity = textBox3.Text;
            string price = textBox4.Text;
            // ตรวจสอบค่าว่าง
            if (code != "" && name != "" && quantity != "" && price != "")
            {
                MySqlConnection conn = databaseConnection();
                conn.Open();
                MySqlCommand cmd;
                try
                {
                    cmd = conn.CreateCommand();
                    cmd.CommandText = "UPDATE stock SET name = @name, quantity = @quantity, price = @price WHERE code = @code";
                    cmd.Parameters.AddWithValue("@code", code);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@quantity", quantity);
                    cmd.Parameters.AddWithValue("@price", price);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    //ตรวจสอบว่ามีแถวที่ถูกอัปเดตได้หรือไม่ หากมีจะรีเฟรชข้อมูลใน DataGridView และล้างค่าใน TextBoxes และ PictureBox
                    if (rowsAffected > 0)
                    {
                       
                        LoadDataIntoDataGridView(); 
                        textBox1.Text = "";
                        textBox2.Text = "";
                        textBox3.Text = "";
                        textBox4.Text = "";

                        
                        pictureBox2.Image = null;
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
                finally
                {
                    conn.Close();
                }
            }
            else
            {
                MessageBox.Show("Please fill in all fields.");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string code = textBox1.Text;

            if (!string.IsNullOrEmpty(code))
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete this record?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    MySqlConnection conn = databaseConnection();
                    conn.Open();
                    MySqlCommand cmd;
                    try
                    {
                        cmd = conn.CreateCommand();
                        cmd.CommandText = "DELETE FROM stock WHERE code = @code";
                        cmd.Parameters.AddWithValue("@code", code);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            
                            LoadDataIntoDataGridView(); //โหลดข้อมูลจาก sql มาแสดง
                            ClearTextBoxes(); 
                        }
                        else
                        {
                            MessageBox.Show("No data deleted.");
                        }
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
            else
            {
                MessageBox.Show("Please select a record to delete.");
            }
        }

        // Clear all text boxes
        private void ClearTextBoxes()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            pictureBox2.Image = null;
        }


        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // ตรวจสอบว่าคลิกที่เซลล์ที่ถูกต้องหรือไม่
            if (e.RowIndex >= 0)
            {
                // จำแนกแถวที่ถูกคลิก
                dataGridView1.Rows[e.RowIndex].Selected = true;
                // แสดงข้อมูลใน TextBox ตามคอลัมน์ที่เลือก
                textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells["code"].FormattedValue.ToString();
                textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells["name"].FormattedValue.ToString();
                textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells["quantity"].FormattedValue.ToString();
                textBox4.Text = dataGridView1.Rows[e.RowIndex].Cells["price"].FormattedValue.ToString();
                // แสดงรูปใน PictureBox2 ของแถวที่ถูกคลิก
                byte[] imageBytes = (byte[])dataGridView1.Rows[e.RowIndex].Cells["photo"].Value;
                if (imageBytes != null)
                {
                    using (MemoryStream ms = new MemoryStream(imageBytes))
                    {
                        pictureBox2.Image = Image.FromStream(ms);
                    }
                }
                else
                {
                    pictureBox2.Image = null;
                }
            }
        }



        private void dataEquiment_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

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
    }
}
