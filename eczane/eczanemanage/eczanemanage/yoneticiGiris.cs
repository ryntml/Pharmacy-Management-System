using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using Guna.Charts.WinForms;
using System.Diagnostics;

namespace eczanemanage
{
    public partial class yoneticiGiris : Form
    {

        
        public yoneticiGiris()
        {
            InitializeComponent();
            
    }

        private void button4_Click_1(object sender, EventArgs e)  // ÖNCEKİ MENÜYE DÖN
        {
            secimFormu don = new secimFormu();
            don.Show();
            this.Hide();
        }

        private bool KullaniciDogrulama(string kullaniciAdi, string sifre)
        {
            string connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\eczane\\eczanemanage\\eczanemanage\\bin\\Debug\\eczaneyonetim.mdb";
            OleDbConnection conn = new OleDbConnection(connString);
            OleDbCommand cmd = new OleDbCommand("SELECT COUNT(*) FROM yoneticiGiris WHERE KullaniciAdi = ? AND Sifre = ?", conn);
            cmd.Parameters.AddWithValue("KullaniciAdi", kullaniciAdi);
            cmd.Parameters.AddWithValue("Sifre", sifre);

            conn.Open();
            int count = (int)cmd.ExecuteScalar();
            conn.Close();

            return count > 0;
        }

        private void button1_Click(object sender, EventArgs e)   // yönetici giriş 
        {
            string kullaniciAdi = textBox1.Text.Trim();
            string sifre = textBox2.Text.Trim();
            

            if (KullaniciDogrulama(kullaniciAdi, sifre))
            {
                MessageBox.Show("Giriş başarılı!", "Başarılı");
                // Giriş yapılması gereken sayfaya yönlendirme yapılıyor
                yonetici ys = new yonetici();  // yönetici sayfası
                ys.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Kullanıcı adı veya şifre yanlış!");
            }
        }

        private void yoneticiGiris_Load(object sender, EventArgs e)
        {

        }

       

        private void textBox2_TextChanged(object sender, EventArgs e)  // PAROLA TEXT
        {
            textBox2.PasswordChar = '*'; 
        }

        private void button2_Click(object sender, EventArgs e)  // SIFIRLA BUTONU 
        {
            textBox1.Clear();
            textBox2.Clear();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }






}
