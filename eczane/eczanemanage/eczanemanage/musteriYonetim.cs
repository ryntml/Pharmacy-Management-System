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
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Web.UI.WebControls;

namespace eczanemanage
{
    public partial class musteriYonetim : Form
    {



        public musteriYonetim()
        {
            InitializeComponent();
        }


        OleDbConnection baglantim = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\eczane\\eczanemanage\\eczanemanage\\bin\\Debug\\eczaneyonetim.mdb");
        
        
        private void musteriYonetim_Load(object sender, EventArgs e)
        {

            dataGridView1.ReadOnly = true;
            musterileri_goster();
        }

        private void musterileri_goster()
        {
            try
            {
                baglantim.Open();
                //OleDbDataAdapter musterilistele = new OleDbDataAdapter("select * from musteriListesi", baglantim);
                OleDbDataAdapter musterilistele = new OleDbDataAdapter("select tcno AS [TC Kimlik No],ad AS [Ad], soyad AS [Soyad], tlfno AS [Telefon No],cinsiyeti AS [Cinsiyeti], adresi AS [Adresi], receteno AS [Reçete No], dogumtarih AS [Doğum Tarihi] from musteriListesi" , baglantim);

                DataSet dshafiza = new DataSet();
                musterilistele.Fill(dshafiza);
                dataGridView1.DataSource = dshafiza.Tables[0];
                baglantim.Close();
            }

            catch (Exception hatamsj)
            {

                MessageBox.Show(hatamsj.Message);
                baglantim.Close();

            }
        }

        private void button5_Click(object sender, EventArgs e)  //REÇETEYİ GÖR BUTONU
        {
            string tcKimlik = textBox1.Text;

            // TC kimlik numarasına göre reçeteyi getir
            string receteDosyaYolu = GetReceteDosyaYolu(tcKimlik);

            if (receteDosyaYolu != null)
            {
                // Yeni form oluştur
                receteGor receteFormu = new receteGor(receteDosyaYolu, tcKimlik);
                receteFormu.ShowDialog();
            }
            else
            {
                MessageBox.Show("Belirtilen TC kimlik numarasına ait reçete bulunamadı.");
            }
        }

        private string GetReceteDosyaYolu(string tcKimlik)
        {
            // Reçete dosyalarının bulunduğu dizini belirtin
            string receteDizin = @"C:\eczane\receteler";

            // TC kimlik numarasına göre reçete dosyasının adını oluştur
            string receteDosyaAdi = tcKimlik + ".jpg";

            // Reçete dosyasının tam yolunu oluştur
            string receteDosyaYolu = Path.Combine(receteDizin, receteDosyaAdi);

            // Reçete dosyasının var olup olmadığını kontrol et
            if (File.Exists(receteDosyaYolu))
            {
                return receteDosyaYolu;
            }
            else
            {
                return null;
            }
        }

        private void button1_Click(object sender, EventArgs e) // KAYDET BUTONU
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text) || string.IsNullOrWhiteSpace(textBox3.Text) || string.IsNullOrWhiteSpace(textBox4.Text) || string.IsNullOrWhiteSpace(textBox5.Text) || comboBox1.SelectedItem == null || string.IsNullOrWhiteSpace(textBox6.Text))
            {
                MessageBox.Show("Lütfen bütün alanları doldurunuz. ");
                return;
            }

            try
            {
                baglantim.Open();
                OleDbDataAdapter eklekomutu = new OleDbDataAdapter("insert into musteriListesi (tcno, ad, soyad, tlfno, cinsiyeti, dogumtarih, receteno,adresi) values ('" + textBox1.Text + "', '" + textBox2.Text + "', '" + textBox3.Text + "', '" + textBox4.Text + "', '" + comboBox1.SelectedItem.ToString() + "','"+ dateTimePicker1.Value.ToString() + "', '" + textBox5.Text + "', '"+textBox6.Text+"')", baglantim);

                DataSet dshafiza = new DataSet();
                eklekomutu.Fill(dshafiza);
                baglantim.Close();
                MessageBox.Show("Hasta veri tabanına eklendi.");
                textBox1.Clear(); //tc
                textBox2.Clear(); //ad
                textBox3.Clear(); //soyad
                textBox4.Clear(); //numara
                textBox5.Clear(); //receteno
                textBox6.Clear(); //adresi
                comboBox1.Items.Clear(); //cinsiyet

                musterileri_goster();
            }
            catch (Exception hatamsj)
            {
                MessageBox.Show(hatamsj.Message);
                baglantim.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)  // GÜNCELLE BUTONU
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text) || string.IsNullOrWhiteSpace(textBox3.Text) || string.IsNullOrWhiteSpace(textBox4.Text) || string.IsNullOrWhiteSpace(textBox5.Text) ||  string.IsNullOrWhiteSpace(textBox6.Text))
            {
                MessageBox.Show("Lütfen bütün alanları doldurunuz. ");
                return;
            }

            try
            {
                baglantim.Open();
                OleDbCommand guncelleKomutu = new OleDbCommand("UPDATE musteriListesi SET ad=@ad, soyad=@soyad, tlfno=@tlfno, cinsiyeti=@cinsiyeti, adresi=@adresi, receteno=@receteno,dogumtarih=@dogumtarih WHERE tcno=@tcno", baglantim);
                guncelleKomutu.Parameters.AddWithValue("@ad", textBox2.Text);
                guncelleKomutu.Parameters.AddWithValue("@soyad", textBox3.Text);
                guncelleKomutu.Parameters.AddWithValue("@tlfno", textBox4.Text);
                guncelleKomutu.Parameters.AddWithValue("@cinsiyeti", comboBox1.SelectedItem.ToString());
                guncelleKomutu.Parameters.AddWithValue("@adresi", textBox6.Text);
                guncelleKomutu.Parameters.AddWithValue("@receteno", textBox5.Text);
                guncelleKomutu.Parameters.AddWithValue("@dogumtarih", dateTimePicker1.Value);
                guncelleKomutu.Parameters.AddWithValue("@tcno", textBox1.Text);
                
                guncelleKomutu.ExecuteNonQuery();

                baglantim.Close();
                MessageBox.Show("Hasta bilgileri güncellendi.");
                musterileri_goster();
            }

            catch (Exception a)

            {
                MessageBox.Show(a.Message);
                baglantim.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)  // SİL BUTONU
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Lütfen TC Kimlik Numarasını giriniz. ");
                return;
            }

            try
            {
                baglantim.Open();
                OleDbCommand silkomutu = new OleDbCommand("delete from musteriListesi where tcno='" + textBox1.Text + "'", baglantim);
                silkomutu.ExecuteNonQuery();
                baglantim.Close();
                MessageBox.Show("Hasta veri tabanından silindi.");
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
                textBox5.Clear();
                textBox6.Clear();
                comboBox1.Items.Clear(); //cinsiyet
                dateTimePicker1.Value = DateTime.Now;

                musterileri_goster();
            }
            catch (Exception hatamsj)
            {
                MessageBox.Show(hatamsj.Message);
                baglantim.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e) // TEMİZLE BUTONU
        {
            textBox1.Clear(); //tc
            textBox2.Clear(); //ad
            textBox3.Clear(); //soyad
            textBox4.Clear(); //numara
            textBox5.Clear(); //receteno
            textBox6.Clear(); //adresi
            comboBox1.Items.Clear(); //cinsiyet
            dateTimePicker1.Value = DateTime.Now; 
        }

        private void textBox1_TextChanged(object sender, EventArgs e)  // TC'YE GÖRE HASTA BİLGİLERİ OTOMATİK GELECEK.
        {

            if (textBox1.Text == "")
            {
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                textBox6.Text = "";
                comboBox1.SelectedItem = null;

            }
            if (textBox1.Text.Length > 11)
            {
                MessageBox.Show("TC Kimlik Numarası 11 haneden uzun olamaz!");
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
                textBox5.Clear();
                textBox6.Clear();
                comboBox1.Items.Clear();
                return;
            }

            baglantim.Open();
            OleDbCommand komut = new OleDbCommand("select *from musteriListesi where tcno like '" + textBox1.Text + "'", baglantim);
            OleDbDataReader read = komut.ExecuteReader();
            while (read.Read())
            { 
                textBox2.Text = read["ad"].ToString();
                textBox3.Text = read["soyad"].ToString();
                textBox4.Text = read["tlfno"].ToString();
                textBox5.Text = read["receteno"].ToString();
                textBox6.Text = read["adresi"].ToString();
                comboBox1.Text = read["cinsiyeti"].ToString();
                dateTimePicker1.Text = read["dogumtarih"].ToString();
            }
            baglantim.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
