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

namespace ProjectFB
{
    public partial class Form11 : Form
    {
        private string previousTextBoxValue; // เก็บค่าที่ถูกส่งมาก่อนหน้านี้

        public Form11(string textBox7Text)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            previousTextBoxValue = textBox7Text; // กำหนดค่าเริ่มต้นให้กับ previousTextBoxValue
            textBox1.Text = textBox7Text;
            GenerateQRCode(); // เรียกใช้งานฟังก์ชันสร้าง QR Code ทันทีหลังจากกำหนดค่า TextBox
        }

        // ฟังก์ชันสร้าง QR Code
        private void GenerateQRCode()
        {
            string phoneNumber = "0873744983";  // เบอร์โทรศัพท์ที่ใช้ในตัวอย่าง

            // ตรวจสอบว่า TextBox มีข้อมูลหรือไม่
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                // ใช้ Regular Expression เพื่อกรองข้อมูลให้เป็นตัวเลขเท่านั้น
                string amount = Regex.Replace(textBox1.Text, "[^0-9.]", "");

                // ตรวจสอบว่าข้อมูลไม่ว่างหลังจากการกรองด้วย Regular Expression
                if (!string.IsNullOrEmpty(amount))
                {
                    // ตัดข้อมูลเพื่อเก็บเฉพาะเลขทศนิยมที่อยู่หลังจุดทศนิยมและเฉพาะ 2 ตำแหน่ง
                    int decimalIndex = amount.IndexOf('.');
                    if (decimalIndex != -1 && amount.Length > decimalIndex + 3)
                    {
                        amount = amount.Substring(0, decimalIndex + 3);
                    }

                    string url = "https://promptpay.io/" + phoneNumber + "/" + amount;

                    try
                    {
                        // ดาวน์โหลดรูปภาพจาก URL
                        WebClient webClient = new WebClient();
                        byte[] imageBytes = webClient.DownloadData(url);

                        // แปลง bytes เป็นรูปภาพ
                        Image qrCodeImage;
                        using (var ms = new System.IO.MemoryStream(imageBytes))
                        {
                            qrCodeImage = Image.FromStream(ms);
                        }

                        // แสดงรูปภาพใน PictureBox
                        pictureBox1.Image = qrCodeImage;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Please enter a valid numeric amount.");
                }
            }
            else
            {
                MessageBox.Show("Please enter an amount.");
            }
        }

        // เมื่อข้อความใน TextBox เปลี่ยน
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // เช็คว่าค่าที่ถูกส่งมาไม่ใช่ค่าล่าสุด
            if (textBox1.Text != previousTextBoxValue)
            {
                // กำหนดค่าใหม่ให้กับ previousTextBoxValue
                previousTextBoxValue = textBox1.Text;

                // สร้าง QR Code ใหม่
                GenerateQRCode();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            
            this.Close();

          
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

            }
}
