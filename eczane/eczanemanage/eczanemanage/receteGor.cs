using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eczanemanage
{
    public partial class receteGor : Form
    {


        private string receteDosyaYolu;
        private string tcKimlikNo;


        public receteGor(string dosyaYolu,string tcKimlik)
        {
            InitializeComponent();
            receteDosyaYolu = dosyaYolu;
            tcKimlikNo = tcKimlik;
        }

        
        

        private void receteGor_Load(object sender, EventArgs e)
        {

            HastayiDoldur(tcKimlikNo);

            if (receteDosyaYolu != null)
            {
                // Reçeteyi PictureBox'a yükle
                pictureBox1.Image = Image.FromFile(receteDosyaYolu);

            }
            else
            {
                MessageBox.Show("Reçete dosyası bulunamadı.");
                this.Close();
            }
        }


        private void HastayiDoldur(string tcKimlik)
        {
            try
            {
                // TC Kimlik numarasına göre hastanın bilgilerini veritabanından alın

                OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\eczane\\eczanemanage\\eczanemanage\\bin\\Debug\\eczaneyonetim.mdb");
                string sorgu = "SELECT tcno, ad, soyad, dogumtarih, receteno FROM musteriListesi WHERE tcno = @TCKimlik";
                OleDbCommand komut = new OleDbCommand(sorgu, baglanti);
                komut.Parameters.AddWithValue("@TCKimlik", tcKimlik);
                

                baglanti.Open();
                OleDbDataReader okuyucu = komut.ExecuteReader();

                if (okuyucu.Read())
                {
                    // Hastanın bilgilerini form kontrollerine yerleştir
                    textBox1.Text = okuyucu["tcno"].ToString();
                    textBox2.Text = okuyucu["ad"].ToString();
                    textBox3.Text = okuyucu["soyad"].ToString();
                    textBox4.Text = okuyucu["dogumtarih"].ToString();
                    textBox5.Text = okuyucu["receteno"].ToString();


                    textBox1.ReadOnly = true;
                    textBox2.ReadOnly = true;
                    textBox3.ReadOnly = true;
                    textBox4.ReadOnly = true;
                    textBox5.ReadOnly = true;
                }
                else
                {
                    MessageBox.Show("Hasta bilgileri bulunamadı.");
                    this.Close(); // Formu kapat
                }

                baglanti.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
                this.Close(); // Formu kapat
            }
        }




        private void button1_Click(object sender, EventArgs e)  //ÇIKIŞ BUTONU OLACAK. 
        {
            this.Close();
        }

    }
}

