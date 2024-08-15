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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace ProjectFB
{
    public partial class Form10 : Form
    {
        private MySqlConnection databaseConnection() // เชื่อมต่อกับฐานข้อมูล
        {
            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=mor's_fruit_basket;charset=utf8;";
            MySqlConnection conn = new MySqlConnection(connectionString);
            return conn;
        }

        public Form10()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
  
        }

        public void SetData(string username, string data1, string data2, string data3, Image image) //รับและกำหนดค่าที่ถูกส่งมา
        {
            textBox1.Text = data1;
            textBox2.Text = data2;
            textBox3.Text = data3;
            label7.Text = username;
            pictureBox2.Image = image;
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage; //ปรับขนาดให้พอดี
            string code = textBox1.Text;
            string quantity = GetQuantityFromDatabase(code);
            textBox5.Text = quantity;
        }
        private string GetQuantityFromDatabase(string code) //ดึงข้อมูลสินค้าจาก stockมาแสดง
        {
            string quantity = "";

            MySqlConnection conn = databaseConnection(); // เชื่อมต่อกับฐานข้อมูล

            try
            {
                conn.Open();

                // สร้างคำสั่ง SQL เพื่อดึงข้อมูล quantity จากตาราง stock โดยอ้างอิงจาก code
                string query = "SELECT quantity FROM stock WHERE code = @code";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@code", code);

                // อ่านข้อมูลจากฐานข้อมูล
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    quantity = reader["quantity"].ToString();
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                conn.Close(); // ปิดการเชื่อมต่อในส่วนของ finally
            }

            return quantity;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e) //เพิ่มจำนวนสินค้าใน bill ลดสินค้าใน stock
        {
            // เชื่อมต่อกับฐานข้อมูล
            MySqlConnection conn = databaseConnection();

            try
            {
                conn.Open();

                string code = textBox1.Text;
                string name = textBox2.Text;
                string price = textBox3.Text;
                string quantityToAdd = textBox4.Text;

                // ตรวจสอบว่าป้อนจำนวนไม่เกินจำนวนที่มีอยู่ใน stock
                string currentStockQuantity = textBox5.Text;
                int stockQuantity = Convert.ToInt32(currentStockQuantity);
                int requestedQuantity = Convert.ToInt32(quantityToAdd);

                if (requestedQuantity > stockQuantity)
                {
                    MessageBox.Show("Requested quantity exceeds stock quantity.");
                    return; // ออกจากเมทอดโดยไม่ดำเนินการต่อ
                }

                // ตรวจสอบว่ามีรายการสินค้านี้อยู่ใน Bill แล้วหรือไม่
                MySqlCommand checkCmd = new MySqlCommand("SELECT COUNT(*) FROM Bill WHERE code = @code", conn);
                checkCmd.Parameters.AddWithValue("@code", code);
                int existingItemCount = Convert.ToInt32(checkCmd.ExecuteScalar());
                // มีอยู่แล้วอัปเดตค่า quantity และ sum ในตาราง Bill 
                if (existingItemCount > 0)
                {
                    // ดึงค่า quantity และ sum ของสินค้าจากตาราง Bill
                    MySqlCommand getDataCmd = new MySqlCommand("SELECT quantity, sum FROM Bill WHERE code = @code", conn);
                    getDataCmd.Parameters.AddWithValue("@code", code);
                    MySqlDataReader reader = getDataCmd.ExecuteReader();

                    float lastQuantity = 0;
                    float lastSum = 0;

                    while (reader.Read())
                    {
                        lastQuantity = Convert.ToSingle(reader["quantity"]);
                        lastSum = Convert.ToSingle(reader["sum"]);
                    }
                    reader.Close();

                    // คำนวณค่า sum ใหม่โดยนำค่า quantity ที่ถูกเพิ่มเข้าไปมา * price
                    float newSum = (lastSum + (Convert.ToSingle(quantityToAdd) * Convert.ToSingle(price)));
                    string formattedSum = newSum.ToString("#,##0.00");

                    // อัปเดตค่า quantity และ sum ในตาราง Bill
                    MySqlCommand updateCmd = new MySqlCommand("UPDATE Bill SET quantity = @newQuantity, sum = @newSum WHERE code = @code", conn);
                    updateCmd.Parameters.AddWithValue("@newQuantity", lastQuantity + Convert.ToSingle(quantityToAdd));
                    updateCmd.Parameters.AddWithValue("@newSum", formattedSum); // ใช้ formattedSum แทน newSum
                    updateCmd.Parameters.AddWithValue("@code", code);



                    int rowsAffected = updateCmd.ExecuteNonQuery();
                    
                }


                else
                {
                    // ไม่มีรายการสินค้านี้อยู่ใน Bill ยัง ดังนั้นให้ทำการเพิ่มรายการใหม่
                    // สร้างคำสั่ง SQL เพื่อเพิ่มข้อมูลลงในตาราง Bill
                    string insertQuery = "INSERT INTO Bill (code, name, quantity, price, sum, photo) VALUES (@code, @name, @quantity, @price, @sum, @photo)";
                    MySqlCommand insertCmd = new MySqlCommand(insertQuery, conn);

                    // คำนวณค่า sum
                    float totalPrice = Convert.ToSingle(price);
                    int totalQuantity = Convert.ToInt32(quantityToAdd);
                    float totalSum = totalPrice * totalQuantity;
                    string formattedSum = totalSum.ToString("#,##0.00");

                    // นำรูปภาพจาก pictureBox2 และแปลงเป็น byte array
                    Image img = pictureBox2.Image;
                    byte[] photoData = ImageToByteArray(img);

                    // กำหนดค่าพารามิเตอร์
                    insertCmd.Parameters.AddWithValue("@code", code);
                    insertCmd.Parameters.AddWithValue("@name", name);
                    insertCmd.Parameters.AddWithValue("@quantity", quantityToAdd);
                    insertCmd.Parameters.AddWithValue("@price", price);
                    insertCmd.Parameters.AddWithValue("@sum", formattedSum);
                    insertCmd.Parameters.AddWithValue("@photo", photoData);

                    // ประมวลผลคำสั่ง SQL
                    int insertResult = insertCmd.ExecuteNonQuery();
                    

                }

                // อัพเดตจำนวนสินค้าในตาราง stock โดยการลบจำนวนที่เพิ่มเข้ามาออก
                MySqlCommand updateStockCmd = new MySqlCommand("UPDATE stock SET quantity = quantity - @quantity WHERE code = @code", conn);
                updateStockCmd.Parameters.AddWithValue("@quantity", quantityToAdd);
                updateStockCmd.Parameters.AddWithValue("@code", code);
                int stockUpdateResult = updateStockCmd.ExecuteNonQuery();
                if (stockUpdateResult > 0)
                {
                   
                    // ดึงข้อมูลจำนวนสินค้าใหม่จากตาราง stock และแสดงใน textbox5
                    string updatedQuantity = GetQuantityFromDatabase(code);
                    textBox5.Text = updatedQuantity;
                }
                else
                {
                    MessageBox.Show("Failed to update stock quantity.");
                }

                // แสดงข้อมูลในตาราง Bill ใหม่
                showbill(); // เรียกเมท็อด showbill() เพื่อแสดงข้อมูลใน DataGridView1
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
        private byte[] ImageToByteArray(Image img) //แปลงรูปเก็บใน sql
        {
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, img.RawFormat);
                return ms.ToArray();
            }
        }


        private void showbill() //แสดงรายการสินค้าในบิล bill
        {
            MySqlConnection conn = databaseConnection();
            conn.Open();
            MySqlCommand cmd;
            try
            {
                cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT code, name, " +
                    "FORMAT(CAST(REGEXP_REPLACE(price, '[^0-9]+', '') AS DECIMAL), '#,0.00') AS price, " +
                    "CAST(REGEXP_REPLACE(quantity, '[^0-9]+', '') AS DECIMAL) AS quantity, " +
                    "FORMAT((CAST(REGEXP_REPLACE(price, '[^0-9]+', '') AS DECIMAL) * " +
                    "CAST(REGEXP_REPLACE(quantity, '[^0-9]+', '') AS DECIMAL)), 'N2') AS sum " +
                    "FROM bill";
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


        private void pictureBox2_Click_1(object sender, EventArgs e)
        {

        }

        

        private void button3_Click_2(object sender, EventArgs e)
        {

            Form9 form9 = new Form9();
            form9.Show();
            this.Hide();
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

        }
        public void SetData(string data)
        {
            label7.Text = data;
        }

        private void Form10_Load_1(object sender, EventArgs e)
        {
            showbill();
        }

        
        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            string username = label7.Text;
            Form12 form12 = new Form12(); //จ่ายเงิน
            form12.SetData(username);
            form12.Show();
            this.Hide();
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click_2(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {
            string username = label7.Text;
            // สร้าง instance ของ Form10
            Form9 form9 = new Form9();

            // ส่งค่าจาก TextBox6 ใน Form12 ไปยัง Form10
            form9.SetData(username);

            // แสดง Form10
            form9.Show();

            // ซ่อน Form12
            this.Hide();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            MySqlConnection conn = databaseConnection();
            conn.Open();

            //มีไอดีใน Bill มั้ย
            string checkidQuery = "SELECT COUNT(*) FROM bill WHERE code = @code";
            MySqlCommand checkidCmd = new MySqlCommand(checkidQuery, conn);
            checkidCmd.Parameters.AddWithValue("@code", textBox1.Text);
            ////conn.Open();
            int existingCount = Convert.ToInt32(checkidCmd.ExecuteScalar());

            if (existingCount > 0)//ถ้ามีไอดีในลิสให้ทำ
            {
                //ดึง Amount จาก Bill มาเก็บไว้

                string getCurrentAmountQuery = "SELECT quantity FROM bill WHERE code = @code";
                MySqlCommand getCurrentAmountCmd = new MySqlCommand(getCurrentAmountQuery, conn);
                getCurrentAmountCmd.Parameters.AddWithValue("@code", textBox1.Text);
                int currentAmount = Convert.ToInt32(getCurrentAmountCmd.ExecuteScalar());

                // ดึงข้อมูลที่มีรหัสตรงกับ idproduct มาเก็บไว้ในตัวแปรแต่ละตัว
                string getCurrentDataQuery = "SELECT  name, quantity,price  FROM stock WHERE code = @code";
                MySqlCommand getCurrentDataCmd = new MySqlCommand(getCurrentDataQuery, conn);
                getCurrentDataCmd.Parameters.AddWithValue("@code", textBox1.Text);

                MySqlDataReader reader = getCurrentDataCmd.ExecuteReader();

                if (reader.Read())
                {
                    string productName = reader.GetString(0);
                    int stockQuantity = reader.GetInt32(1);
                    string price = reader.GetString(2);
                    reader.Close();


                    reader.Close();
                    //Numไม่เป็นตัวนสและเป็นบวก และ Numน้อยกว่า Amount มั้ย
                    if (int.TryParse(textBox4.Text, out int quantityToDecrease) && quantityToDecrease > 0 && quantityToDecrease < currentAmount)
                    {
                        //Numไม่เป็นตัวนสและเป็นบวก และ Numน้อยกว่า Amount ให้ทำ
                        //อัพเดตจำนวนใน Bill เมื่อลบ
                        string updateQuery = "UPDATE Bill SET quantity = quantity - @Num WHERE code = @code;";
                        MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn);
                        updateCmd.Parameters.AddWithValue("@Num", quantityToDecrease);
                        updateCmd.Parameters.AddWithValue("@code", textBox1.Text);
                        updateCmd.ExecuteNonQuery();

                        //ดึงจำนวนจากบิลมาเก็บไว้
                        string getCurrentQuantityQuery = "SELECT quantity FROM Bill WHERE code = @code;";
                        MySqlCommand getCurrentQuantityCmd = new MySqlCommand(getCurrentQuantityQuery, conn);
                        getCurrentQuantityCmd.Parameters.AddWithValue("@code", textBox1.Text);
                        int currentQuantity = Convert.ToInt32(getCurrentQuantityCmd.ExecuteScalar());

                        //อัพเเดตราคา
                        string updateprice = "UPDATE Bill SET sum = @quantity * @price WHERE code = @code;";
                        MySqlCommand updatepriceCmd = new MySqlCommand(updateprice, conn);
                        updatepriceCmd.Parameters.AddWithValue("@quantity", currentQuantity);
                        updatepriceCmd.Parameters.AddWithValue("@price", price);
                        updatepriceCmd.Parameters.AddWithValue("@code", textBox1.Text);
                        updatepriceCmd.ExecuteNonQuery();

                        //อัพเดตจำนวนสต็อก
                        string updateQuantity = "UPDATE stock SET quantity = @quantity + @Num WHERE code = @code;";
                        MySqlCommand updateQuantityCmd = new MySqlCommand(updateQuantity, conn);
                        updateQuantityCmd.Parameters.AddWithValue("@quantity", stockQuantity);
                        updateQuantityCmd.Parameters.AddWithValue("@Num", quantityToDecrease);
                        updateQuantityCmd.Parameters.AddWithValue("@code", textBox1.Text);
                        updateQuantityCmd.ExecuteNonQuery();

                        //โชว์จำนวนใน stock
                        
                        string getquantitystock = "SELECT quantity FROM stock WHERE code = @code;";
                        MySqlCommand getquantitystockCmd = new MySqlCommand(getquantitystock, conn);
                        getquantitystockCmd.Parameters.AddWithValue("@code", textBox1.Text);
                        int getQ = Convert.ToInt32(getquantitystockCmd.ExecuteScalar());

                        textBox5.Text = getQ.ToString() ;
                        conn.Close();

                    }
                    //Numไม่เป็นตัวนสและเป็นบวก และ Num มากกว่าเท่ากับ Amount ให้ทำ
                    else if (int.TryParse(textBox4.Text, out int quantityToDecrease2) && quantityToDecrease2 > 0 && quantityToDecrease2 >= currentAmount)
                    {

                        //อัพเดพสต็อก
                        string updateQuantity = "UPDATE stock SET quantity = @quantity + @Amount WHERE code = @code;";
                        MySqlCommand updateQuantityCmd = new MySqlCommand(updateQuantity, conn);
                        updateQuantityCmd.Parameters.AddWithValue("@quantity", stockQuantity);
                        updateQuantityCmd.Parameters.AddWithValue("@Amount", currentAmount);
                        updateQuantityCmd.Parameters.AddWithValue("@code", textBox1.Text);
                        updateQuantityCmd.ExecuteNonQuery();

                        //ลบข้อมูลนั้นออก
                        string deletebill = "DELETE FROM bill WHERE code = @code;";
                        MySqlCommand deleteCmd = new MySqlCommand(deletebill, conn);
                        deleteCmd.Parameters.AddWithValue("@code", textBox1.Text);
                        deleteCmd.ExecuteNonQuery();

                        string getquantitystock = "SELECT quantity FROM stock WHERE code = @code;";
                        MySqlCommand getquantitystockCmd = new MySqlCommand(getquantitystock, conn);
                        getquantitystockCmd.Parameters.AddWithValue("@code", textBox1.Text);
                        int getQ = Convert.ToInt32(getquantitystockCmd.ExecuteScalar());

                        textBox5.Text = getQ.ToString();
                        conn.Close();
                    }

                }
            }

            showbill();
            conn.Close();
        }

        private void textBox5_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e) //ข้อมูลจากตาราง bill
        {
            //ตรวจสอบแถวที่ถูกคลิก
            if (e.RowIndex >= 0)
            {
                
                dataGridView1.Rows[e.RowIndex].Selected = true;

                //นำข้อมูลมาแสดงใน textbox
                textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells["code"].FormattedValue.ToString();
                textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells["name"].FormattedValue.ToString();
                textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells["price"].FormattedValue.ToString();
                textBox4.Text = dataGridView1.Rows[e.RowIndex].Cells["quantity"].FormattedValue.ToString();
 
                MySqlConnection conn = databaseConnection(); //ดึงข้อมูลจาก sql
                try
                {
                    conn.Open();
                    //ดึงรหัสสินค้า (code) ที่ถูกเลือกจาก TextBox1 เพื่อนำมาใช้ในการดึงข้อมูลภาพ
                    string code = textBox1.Text;

                    //ดึข้อมูลภาพจากตาราง stock โดยใช้ INNER JOIN ระหว่างตาราง "bill" และ "stock" โดยเงื่อนไขคือรหัสสินค้าตรงกัน โดยใช้เงื่อนไข WHERE เพื่อระบุรหัสสินค้าที่ตรงกับที่ถูกเลือก
                    string query = "SELECT s.photo " +
                                   "FROM bill AS b " +
                                   "JOIN stock AS s ON b.code = s.code " +
                                   "WHERE b.code = @code";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@code", code);

                    
                    byte[] imageBytes = (byte[])cmd.ExecuteScalar();
                    //ตรวจสอบว่าภาพที่ได้ไม่เป็นค่าว่าง และแปลงข้อมูลภาพจาก byte array เป็นภาพและแสดงใน PictureBox2

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

        private void button3_Click_1(object sender, EventArgs e) //ลบสินค้าออกจากตะกร้า
        {
            MySqlConnection conn = databaseConnection();
            conn.Open();

            //ดึง Amount จาก Bill มาเก็บไว้ 
            string getCurrentAmountQuery = "SELECT quantity FROM bill WHERE code = @code";
            MySqlCommand getCurrentAmountCmd = new MySqlCommand(getCurrentAmountQuery, conn);
            getCurrentAmountCmd.Parameters.AddWithValue("@code", textBox1.Text);
            int currentAmount = Convert.ToInt32(getCurrentAmountCmd.ExecuteScalar());

            //ดึงจำนวน Quantity จาก listproduct มาเก็บไว้
            string getQuery = "SELECT quantity FROM bill WHERE code = @code";
            MySqlCommand getQueryCmd = new MySqlCommand(getQuery, conn);
            getQueryCmd.Parameters.AddWithValue("@code", textBox1.Text);
            int currentquantity = Convert.ToInt32(getQueryCmd.ExecuteScalar());

            //อัพเดพสต็อก 
            string updateQuantity = "UPDATE stock SET quantity = @Quantity + @Amount WHERE code = @code;";
            MySqlCommand updateQuantityCmd = new MySqlCommand(updateQuantity, conn);
            updateQuantityCmd.Parameters.AddWithValue("@Quantity", currentquantity);
            updateQuantityCmd.Parameters.AddWithValue("@Amount", currentAmount);
            updateQuantityCmd.Parameters.AddWithValue("@code", textBox1.Text);
            updateQuantityCmd.ExecuteNonQuery();

            //ลบข้อมูลนั้นออก 
            string deletebill = "DELETE FROM bill WHERE code = @code;";
            MySqlCommand deleteCmd = new MySqlCommand(deletebill, conn);
            deleteCmd.Parameters.AddWithValue("@code", textBox1.Text);
            deleteCmd.ExecuteNonQuery();

            showbill();
           
            pictureBox2.Image = null; // เซ็ตรูปภาพให้ว่าง
            textBox1.Text = ""; // เซ็ตข้อความในช่อง idproduct เป็นว่าง
            textBox2.Text = ""; // เซ็ตข้อความในช่อง idproduct เป็นว่าง
            textBox3.Text = ""; // เซ็ตข้อความในช่อง idproduct เป็นว่าง
            textBox4.Text = ""; // เซ็ตข้อความในช่อง idproduct เป็นว่าง
            textBox5.Text = "";

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}
