using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;
using iTextSharp.text.pdf.draw;
using System.Net.Mail;
using System.Windows.Shell;
using MySqlX.XDevAPI.Common;
using System.Drawing.Imaging;
using Ghostscript.NET.Rasterizer;





namespace ProjectFB
{
    public partial class Form12 : Form
    {
        private MySqlConnection databaseConnection() //เชื่อมต่อกับฐานข้อมูล 
        {
            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=mor's_fruit_basket;charset=utf8;";
            MySqlConnection conn = new MySqlConnection(connectionString);
            return conn;
        }
        public Form12()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }
        public void SetData(string username) //รับและกำหนดค่าที่ถูกส่งมา
        {
            label9.Text = username;
        }

        private void Form12_Load(object sender, EventArgs e)
        {
            Checkusername();
            button4.Hide(); //ซ่อนปุ่ม pdf

            DataTable dataTable = new DataTable();

            // เรียกใช้เมธอด GetData เพื่อดึงข้อมูลจากฐานข้อมูลและเก็บไว้ใน DataTable
            GetData(dataTable);

            // กำหนดข้อมูลใน DataTable เป็นแหล่งข้อมูลให้กับ DataGridView
            dataGridView1.DataSource = dataTable;

            CalculateVAT();
            CalculateQuantity();
            CalculateTotalSum();
            UpdateLatestDateTime();
        }
        private void UpdateLatestDateTime() //ดึงข้อมูลเวลาล่าสุดจาก history ใส่ใน pdf
        {
            using (MySqlConnection conn = databaseConnection())
            {
                conn.Open();
                string sql = "SELECT MAX(date) AS date FROM history"; // ถ้าคอลัมน์ที่มีวันที่และเวลาชื่อ "date_time"

                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        textBox8.Text = Convert.ToDateTime(result).ToString(); // อัปเดต textBox7 ด้วยวันที่และเวลาล่าสุด
                    }
                    else
                    {
                        textBox8.Text = "ไม่มีข้อมูล"; // หากไม่พบข้อมูล แสดงข้อความนี้
                    }
                }
            }
        }
        private void GetData(DataTable dataTable)//ดึงข้อมูลจากฐานข้อมูลและเตรียมข้อมูลให้กับ DataTable ที่ถูกส่งเข้ามา
        {
            // สร้างการเชื่อมต่อฐานข้อมูล
            using (MySqlConnection conn = databaseConnection())
            {
                conn.Open();
                // สร้างคำสั่ง SQL เพื่อดึงข้อมูลจากตาราง bill
                string sql = "SELECT code, name, " +
                    "FORMAT(CAST(REGEXP_REPLACE(price, '[^0-9]+', '') AS DECIMAL), '#,0.00') AS price, " +
                    "CAST(REGEXP_REPLACE(quantity, '[^0-9]+', '') AS DECIMAL) AS quantity, " +
                    "FORMAT((CAST(REGEXP_REPLACE(price, '[^0-9]+', '') AS DECIMAL) * " +
                    "CAST(REGEXP_REPLACE(quantity, '[^0-9]+', '') AS DECIMAL)), 'N2') AS sum " +
                    "FROM bill";


                // สร้าง MySqlCommand เพื่อดำเนินการคำสั่ง SQL
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    // สร้าง MySqlDataAdapter เพื่อดึงข้อมูลจากฐานข้อมูล
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        // เตรียม DataTable เพื่อรับข้อมูล
                        adapter.Fill(dataTable);
                    }
                }
            }
        }
        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e) //เมื่อเกิดการเปลี่ยนแปลงใน TextBox2 เพื่อคนหาข้อมูลสมาชิก
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

        private void button2_Click(object sender, EventArgs e) //ไปที่หน้าสร้าง qr
        {
            if ((textBox2.Text.Length == 10 && textBox2.Text.All(char.IsDigit)))
            {
                if (!string.IsNullOrEmpty(textBox1.Text) && !string.IsNullOrEmpty(textBox2.Text) && !string.IsNullOrEmpty(textBox3.Text) && !string.IsNullOrEmpty(textBox4.Text))
                {

                    Form11 form11 = new Form11(textBox7.Text);
                    form11.Show();
                    button4.Show();
                }
                else
                {
                    MessageBox.Show("Please fill out all fields.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid phone number (must contain 10 characters and no letters).", "", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }
       


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        private void CalculateVAT()//ผลรวมของราคาสินค้าทั้งหมดที่แสดงใน DataGridView
        {
            decimal totalSum = 0; //สร้างตัวแปรเก็บเก็บผลรวมของราคาสินค้าทั้งหมดใน DataGridView1 
            foreach (DataGridViewRow row in dataGridView1.Rows) //ค้นหาค่า sum
            {
                if (row.Cells["sum"].Value != null) //ตรวจสอบว่ามีค่าหรือไม่
                {
                    decimal sum; //แปลงค่าเป็นตัวเลข
                    if (decimal.TryParse(row.Cells["sum"].Value.ToString(), out sum))
                    {
                        totalSum += sum; //ค่า sum จะถูกเพิ่มใน totalSum ด้วยการใช้ totalSum += sum ซึ่งเป็นการบวกค่า sum เข้ากับค่า totalSum
                    }
                }
            }

            decimal vat = totalSum * (decimal)0.07; // หรือ (7/100)
            textBox5.Text = vat.ToString("N2"); // แสดงผลลัพธ์ในรูปแบบเลขทศนิยม 2 ตำแหน่ง
        }
        private void CalculateQuantity() //ค่าส่ง ดึงข้อมูลจำนวนสินค้า
        {
            decimal totalQuantity = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["quantity"].Value != null)
                {
                    decimal quantity;
                    if (decimal.TryParse(row.Cells["quantity"].Value.ToString(), out quantity))
                    {
                        totalQuantity += quantity;
                    }
                }
            }

            decimal multipliedQuantity = totalQuantity * 100;
            textBox6.Text = multipliedQuantity.ToString("N2");
        }
        private void CalculateTotalSum() //คำนวณยอดรวม
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

            decimal vat; //บวก vat
            if (decimal.TryParse(textBox5.Text, out vat))
            {
                totalSum += vat;
            }

            decimal multipliedQuantity; //บวกค่ส่ง
            if (decimal.TryParse(textBox6.Text, out multipliedQuantity))
            {
                totalSum += multipliedQuantity;
            }
            if (totalSum > 6000)
            {
                // ลดราคา 10% 100% - 10% หรือ 1 - 0.1 ซึ่งเท่ากับ 0.9
                totalSum *= (decimal)0.9;
            }

            textBox7.Text = totalSum.ToString("N2");
        }
       
        private void button4_Click(object sender, EventArgs e) //สร้าง pdf เก็บประวัติใน history
        {
             MySqlConnection conn = databaseConnection();
            conn.Open();

            //ดึงข้อมูล ไอดี จำนวน ทั้งคอลัมน์จากตารางบิลมา
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT name, quantity,sum FROM bill";
            //string.Join() เพื่อรวมข้อมูลใน List<string> เป็นสตริงที่คั่นด้วยเครื่องหมาย ,
            MySqlDataReader reader = cmd.ExecuteReader();
            List<string> name = new List<string>();
            List<string> quntity = new List<string>();
            List<string> sumList = new List<string>();
            while (reader.Read())
            {
                string idProduct = reader.GetString("name");
                name.Add(idProduct); //ดึงค่าของคอลัมน์ "name" จากแถวปัจจุบันของผู้อ่านและเพิ่มไปยังรายการ name


                string amount = reader.GetString("quantity");
                quntity.Add(amount);

                string sum = reader.GetString("sum");
                sumList.Add(sum);
            }
            reader.Close();

            // ต่อเชื่อม idProducts ให้เป็นสตริงที่คั่นด้วยเครื่องหมาย ','
            string allname = string.Join(", ", name);

            // ต่อเชื่อม amounts ให้เป็นสตริงที่คั่นด้วยเครื่องหมาย ','
            string allquantity = string.Join(", ", quntity);
            string allSum = string.Join(", ", sumList);
            string tel = textBox2.Text;


            string insertQuery = "INSERT INTO history (username, name, tel, id_product, quantity, price, sum, address, e_mail) VALUES (@username, @name, @tel, @id_product, @quantity, @price, @sum, @address, @e_mail)";
            MySqlCommand insertCmd = new MySqlCommand(insertQuery, conn);
            insertCmd.Parameters.AddWithValue("@username", label9.Text);
            insertCmd.Parameters.AddWithValue("@name", textBox1.Text);
            insertCmd.Parameters.AddWithValue("@tel", textBox2.Text);
            insertCmd.Parameters.AddWithValue("@id_product", allname);
            insertCmd.Parameters.AddWithValue("@quantity", allquantity);
            insertCmd.Parameters.AddWithValue("@price", allSum);
            insertCmd.Parameters.AddWithValue("@sum", textBox7.Text);
            insertCmd.Parameters.AddWithValue("@address", textBox4.Text);
            insertCmd.Parameters.AddWithValue("@e_mail", textBox3.Text);
            insertCmd.ExecuteNonQuery();


            ////สร้าง pdf
            string getLastOrderIDQuery = "SELECT MAX(IDOrder) FROM history";
            MySqlCommand getLastOrderIDCmd = new MySqlCommand(getLastOrderIDQuery, conn);
            int lastOrderID = Convert.ToInt32(getLastOrderIDCmd.ExecuteScalar());
            string lastOrderIDString = lastOrderID.ToString();

            string fileName = $"Bill_{lastOrderID}.pdf";
            string filePath = Path.Combine(@"D:\Year 2 Semester 2 2566\ED251007\Bill", fileName); // กำหนดตำแหน่งและชื่อไฟล์ PDF

            iTextSharp.text.Document doc = new iTextSharp.text.Document(PageSize.A4);

            try
            {
                PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));

                // กำหนดฟอนต์ภาษาไทย
                BaseFont thaiFont = BaseFont.CreateFont(@"D:\Year 2 Semester 2 2566\ED251007\C#\ProjectFB\THSarabunNew.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                iTextSharp.text.Font font = new iTextSharp.text.Font(thaiFont, 18); // ใช้ขนาดฟอนต์เล็กลงจาก 20 เป็น 12
                iTextSharp.text.Font font1 = new iTextSharp.text.Font(thaiFont, 30);
                iTextSharp.text.Font font2 = new iTextSharp.text.Font(thaiFont, 26);
                iTextSharp.text.Font boldFont = new iTextSharp.text.Font(thaiFont, 18, iTextSharp.text.Font.BOLD); // ฟอนต์ตัวหนา

                doc.Open(); // เรียกให้เปิดเอกสาร PDF ก่อน

                Chunk receiptChunk = new Chunk("RECEIPT", font1);// เพิ่มเส้นใต้ให้กับข้อความ "RECEIPT"
                receiptChunk.SetUnderline(1f, -1f); // ปรับขนาดเส้นใต้ตามต้องการ // สร้าง Paragraph และเพิ่ม Chunk ลงไป
                Paragraph receiptHeader = new Paragraph();
                receiptHeader.Add(receiptChunk);// กำหนดให้ข้อความ "RECEIPT" อยู่กึ่งกลางหน้ากระดาษ
                receiptHeader.Alignment = Element.ALIGN_CENTER;// เพิ่ม Paragraph ลงในเอกสาร PDF
                doc.Add(receiptHeader);

                doc.Add(new Paragraph("\n"));

                Chunk companyChunk = new Chunk("Mor's_Fruit_Basket", font2);
                Paragraph companyHeader = new Paragraph();
                companyHeader.Add(companyChunk);
                companyHeader.Alignment = Element.ALIGN_CENTER;
                doc.Add(companyHeader);

                LineSeparator line = new LineSeparator(1f, 100f, BaseColor.BLACK, Element.ALIGN_CENTER, -1);
                Paragraph lineSeparator = new Paragraph("--------------------------------------------------------------------------------------------");
                lineSeparator.Alignment = Element.ALIGN_CENTER;
                doc.Add(lineSeparator);


                string lastIdOrder = "";
                MySqlCommand getLastIdOrderCmd = new MySqlCommand("SELECT idOrder FROM history ORDER BY idOrder DESC LIMIT 1", conn);
                object result = getLastIdOrderCmd.ExecuteScalar();
                if (result != null)
                {
                    lastIdOrder = result.ToString();
                }

                // เพิ่มข้อมูล idOrder ล่าสุดลงใน PDF
                doc.Add(new Paragraph("ID Order: " + lastIdOrder, font));
                // เพิ่มข้อมูลลูกค้าลงใน PDF
                string lastDate = "";
                MySqlCommand getLastDateCmd = new MySqlCommand("SELECT date FROM history ORDER BY date DESC LIMIT 1", conn);
                object dateResult = getLastDateCmd.ExecuteScalar();
                if (dateResult != null)
                {
                    lastDate = dateResult.ToString();
                }

                // เพิ่มข้อมูล Date ล่าสุดลงใน PDF
                doc.Add(new Paragraph("Date: " + lastDate, font));
                doc.Add(new Paragraph("Name: " + textBox1.Text, font));
                doc.Add(new Paragraph("Tel: " + textBox2.Text, font));
                doc.Add(new Paragraph("Email: " + textBox3.Text, font));
                doc.Add(new Paragraph("Address: " + textBox4.Text, font));

                doc.Add(new Paragraph("")); // เพิ่มบรรทัดว่าง

                // เพิ่มข้อมูลรายการสินค้าลงใน PDF
                Paragraph orderHeader = new Paragraph("ORDER", font); // สร้าง Paragraph สำหรับข้อความ "ORDER"
                orderHeader.Alignment = Element.ALIGN_CENTER; // กำหนดให้ข้อความ "ORDER" อยู่ตรงกลาง
                doc.Add(orderHeader); // เพิ่มข้อความ "ORDER" ลงในเอกสาร PDF

                doc.Add(new Paragraph("\n"));// เพิ่มบรรทัดว่าง

                PdfPTable table = new PdfPTable(3); // 3 คอลัมน์
                table.WidthPercentage = 100; // กำหนดความกว้างของตารางให้เต็มหน้ากระดาษ

                // สร้างคอลัมน์และเพิ่มหัวข้อ
               
                table.AddCell("Product ID");
                table.AddCell("Quantity");
                table.AddCell("Price");



                using (MySqlConnection connn = databaseConnection())
                {
                    connn.Open();

                    // ดึงข้อมูลล่าสุดจากตาราง history โดยเรียงตามวันที่ล่าสุดก่อนและจำกัดให้เป็นรายการเดียว
                    MySqlCommand cmdd = new MySqlCommand("SELECT name, quantity, REPLACE(sum, ',', '') AS sum FROM bill", connn);
                    MySqlDataReader readerr = cmdd.ExecuteReader();

                    while (readerr.Read())
                    {
                        string idProduct = readerr.GetString("name");
                        string quantity = readerr.GetString("quantity");
                        string sum = readerr.GetString("sum");

                        // แยกรายการของ id_product และ quantity ที่คั่นด้วย ',' เพื่อเพิ่มแต่ละข้อมูลในแถวใหม่ของตาราง PDF
                        string[] idProducts = idProduct.Split(',');
                        string[] quantities = quantity.Split(',');
                        string[] sums = sum.Split(',');

                        // วนลูปเพื่อเพิ่มข้อมูลในแต่ละแถวของตาราง PDF
                        for (int i = 0; i < idProducts.Length; i++)
                        {
                            // สร้าง Paragraph และกำหนดฟอนต์ก่อนเพิ่มข้อความลงในเซลล์
                            Paragraph idProductParagraph = new Paragraph(idProducts[i], font);
                            Paragraph quantityParagraph = new Paragraph(quantities[i], font);

                            // จัดรูปแบบคอลัมน์ sum ให้แสดงเป็น "5,000.00"
                            string formattedSum = float.Parse(sums[i]).ToString("N2");
                            Paragraph sumParagraph = new Paragraph(formattedSum, font);

                            // เพิ่มเซลล์ลงในตารางพร้อมกับข้อความที่มีฟอนต์ที่กำหนด
                            table.AddCell(idProductParagraph);
                            table.AddCell(quantityParagraph);
                            table.AddCell(sumParagraph);
                        }
                    }

                    readerr.Close(); // ปิด reader หลังจากใช้งานเสร็จสิ้น
                }





                // เพิ่มตารางลงใน PDF document
                doc.Add(table);
                Paragraph vatParagraph = new Paragraph("Vat 7%: " + textBox5.Text, font);
                vatParagraph.Alignment = Element.ALIGN_RIGHT;
                doc.Add(vatParagraph);

                Paragraph portageParagraph = new Paragraph("Portage: " + textBox6.Text, font);
                portageParagraph.Alignment = Element.ALIGN_RIGHT;
                doc.Add(portageParagraph);

                Paragraph totalParagraph = new Paragraph("Total: " + textBox7.Text, font);
                totalParagraph.Alignment = Element.ALIGN_RIGHT;
                doc.Add(totalParagraph);
                //เพิ่มรูปใน pdf
                string imagePath = @"D:\Year 2 Semester 2 2566\ED251007\C#\ProjectFB\PhotoFB\FB19.png"; // ตัวอย่างชื่อไฟล์ภาพ
                iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imagePath); // สร้างรูปภาพจากไฟล์ภาพ
                img.ScaleAbsolute(100f, 100f); // กำหนดขนาดของรูปภาพ


                img.SetAbsolutePosition(400f, 50f);
                doc.Add(img); // เพิ่มรูปภาพลงในเอกสาร PDF

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (doc != null) // ตรวจสอบว่า Document ไม่เป็น null ก่อนเรียก Close()
                {
                    doc.Close(); // ปิดเอกสารเพื่อบันทึก
                }
            }

            string updateQuery = "UPDATE history SET bill = @bill WHERE idOrder = (SELECT MAX(idOrder) FROM history);";
            MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn);
            updateCmd.Parameters.AddWithValue("@bill", filePath); // เพิ่มพารามิเตอร์สำหรับที่อยู่ของไฟล์ PDF ที่ต้องการอัปเดต
            updateCmd.ExecuteNonQuery();




            // เมื่อไฟล์ PDF ถูกสร้างและบันทึกเรียบร้อยแล้ว ให้ทำการส่งอีเมล
            SendEmail(filePath, textBox3.Text);

            


            //ลบข้อมูลในตาราง bill
            string deleteQuery = "DELETE FROM bill";
            MySqlCommand deleteCmd = new MySqlCommand(deleteQuery, conn);
            deleteCmd.ExecuteNonQuery();
           
           
        }

        private async void SendEmail(string filePath, string recipientEmail) //สำหรับส่งอีเมลด้วยการแนบไฟล์ PDF เป็น attachment ไปยังผู้รับที่ระบุใน text
        {
            try
            {
                string fromMail = "morakot.e@kkumail.com"; //กำหนดที่อยู่อีเมลและรหัสผ่านของผู้ส่งอีเมล 
                string fromPassword = "gunv dfoo ndbt kgxb";

                MailMessage message = new MailMessage();
                message.From = new MailAddress(fromMail);
                message.Subject = "RECEIPT"; // ชื่อหัว
                message.To.Add(new MailAddress(recipientEmail)); // อีเมลผู้รับจากข้อมูล id member

                // เพิ่มไฟล์ PDF เป็น attachment
                Attachment attachment = new Attachment(filePath);
                message.Attachments.Add(attachment);

                // สร้าง SMTP client
                using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com"))
                {
                    smtpClient.Port = 587; // พอร์ตของ SMTP server
                    smtpClient.Credentials = new NetworkCredential(fromMail, fromPassword); // ข้อมูลการเข้าสู่ระบบ SMTP server
                    smtpClient.EnableSsl = true; // เปิดใช้งาน SSL (ถ้าจำเป็น)

                    // ส่งอีเมล
                    await smtpClient.SendMailAsync(message); // ส่งอีเมลแบบ asynchronous
                }
                
                MessageBox.Show("Sent email successfully");
                string username = label9.Text;
                Form16 form16 = new Form16();
                form16.SetData(username);
                form16.Show();
                this.Hide();


            }
            catch (Exception ex)
            {
                MessageBox.Show("Error sending email: " + ex.Message);
            }

        }

        private void label8_Click(object sender, EventArgs e)
        {
            string username = label9.Text;
            // สร้าง instance ของ Form10
            Form10 form10 = new Form10();

            // ส่งค่าจาก TextBox6 ใน Form12 ไปยัง Form10
            form10.SetData(username);

            // แสดง Form10
            form10.Show();

            // ซ่อน Form12
            this.Hide();
        }


        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {
            
        }
        private void Checkusername()
        {
            string searchText = label9.Text; // รับข้อมูลที่ป้อน

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

                // เมื่อพบข้อมูลใหม่ใน TextBox2 ให้ทำการคำนวณยอดรวมใหม่
                CalculateTotalSum();
            }
            else
            {
                // หากไม่พบข้อมูล ให้ล้าง textbox ที่ต้องการให้เป็นค่าว่าง
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";

                
            }
        }
    }
}
