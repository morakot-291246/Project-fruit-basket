using MySql.Data.MySqlClient;
using System;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace ProjectFB
{
    public partial class Form9 : Form
    {
        private MySqlConnection databaseConnection() // เชื่อมต่อกับฐานข้อมูล
        {
            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=mor's_fruit_basket;charset=utf8;";
            MySqlConnection conn = new MySqlConnection(connectionString);
            return conn;
        }

        public Form9()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

            LoadImagesFromDatabase(); // เรียกเมทอดเพื่อโหลดรูปภาพจากฐานข้อมูล

        }

        private void LoadImagesFromDatabase() //โหลดข้อมูลรูปภาพจากฐานข้อมูล
        {
            using (MySqlConnection conn = databaseConnection())
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();

                    // สร้างคำสั่ง SQL เพื่อดึงข้อมูลรูปภาพทั้งหมดจากตาราง "stock"
                    cmd.CommandText = "SELECT photo,code, name, price FROM stock";

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        // ตรวจสอบว่ามีรูปภาพในตารางหรือไม่
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                // โหลดข้อมูลรูปภาพจากฐานข้อมูล
                                byte[] imageData = (byte[])reader["photo"];
                                string data1 = reader["code"].ToString();
                                string data2 = reader["name"].ToString();
                                string data3 = reader["price"].ToString();


                                // แปลงข้อมูลรูปภาพให้เป็นรูปภาพ
                                Image image = Image.FromStream(new System.IO.MemoryStream(imageData));

                                // สร้าง PictureBox ใหม่และกำหนดขนาด
                                PictureBox pictureBox = new PictureBox();
                                pictureBox.Image = image;
                                pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                                pictureBox.Width = 390;
                                pictureBox.Height = 290;

                                pictureBox.Margin = new Padding(10);

                                // เพิ่ม event handler สำหรับคลิกที่ PictureBox // ส่งออกข้อมูลไป
                                pictureBox.Click += (s, e) => PictureBox_Click(s, e, data1, data2, data3, image);

                                // เพิ่ม PictureBox ลงใน FlowLayoutPanel
                                flowLayoutPanel1.Controls.Add(pictureBox);
                            }
                        }
                        else
                        {
                            MessageBox.Show("No images found in the database.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
        public void SetData(string username) //รับและกำหนดค่าที่ถูกส่งมา
        {
           label2.Text = username;
        }

        private void PictureBox_Click(object sender, EventArgs e, string data1, string data2, string data3,  Image image)
        {
            string username = label2.Text;
            // สร้าง instance ของ Form10
            Form10 form10 = new Form10();

            // ส่งข้อมูลไปยัง Form10
            form10.SetData(username, data1, data2, data3, image);

            // แสดง Form10
            form10.Show();
            this.Close();
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
        public void SetDataa(string username)
        {
            label2.Text = username;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
           
        }

        private void label1_Click(object sender, EventArgs e) //ลบข้อมูลจากตาราง bill
        {
            string username = label2.Text;
            Form16 form16 = new Form16();
            form16.SetData(username);
            form16.Show();
            this.Hide();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
