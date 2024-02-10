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


namespace eczanemanage
{
    public partial class satisYönetim : Form
    {
        public satisYönetim()
        {
            InitializeComponent();
        }


        OleDbConnection baglanti = new OleDbConnection("Provider= Microsoft.ACE.OLEDB.12.0;Data Source = C:\\eczane\\eczanemanage\\eczanemanage\\bin\\Debug\\eczaneyonetim.mdb");
        DataSet daset = new DataSet();

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }


        private void sepetlistele()
        {
            baglanti.Open();
            OleDbDataAdapter adtr = new OleDbDataAdapter("select *from sepet", baglanti);
            adtr.Fill(daset, "sepet");
            dataGridView1.DataSource = daset.Tables["sepet"];
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
            dataGridView1.Columns[2].Visible = false;

            baglanti.Close();
        }


        private void hesapla ()
        {
            try
            {
                baglanti.Open();
                OleDbCommand komut = new OleDbCommand("select sum(toplamfiyat) from sepet", baglanti);
                lblGenelToplam.Text = komut.ExecuteScalar() + " TL ";
                baglanti.Close();
            }

            catch
            {


            }
        }

        private void satisYönetim_Load(object sender, EventArgs e)
        {
            textBox3.ReadOnly = true;
            textBox4.ReadOnly = true;
            sepetlistele();
        }

        private void textBox2_TextChanged(object sender, EventArgs e) // TC GİRİLDİĞİ AN, HASTANIN AD VE SOYADI OTOMATİK GELSİN
        {

            if (textBox2.Text=="")
            {

                textBox3.Text = "";
                textBox4.Text = "";
            }
            if (textBox2.Text.Length > 11)
            {
                MessageBox.Show("TC Kimlik Numarası 11 haneden uzun olamaz!");
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
                return;
            }

            baglanti.Open();
            OleDbCommand komut = new OleDbCommand("select *from musteriListesi where tcno like '"+textBox2.Text+"'",baglanti);
            OleDbDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                textBox3.Text = read["ad"].ToString();
                textBox4.Text = read["soyad"].ToString();

            }
            baglanti.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e) //  BARKOD NO İLE ARAMA YAPILDIĞINDA BİLGİLER GELSİN. 
        {
            Temizle();

            baglanti.Open();
            OleDbCommand komut = new OleDbCommand("select *from ilaclar where barkodno like '" + textBox1.Text + "'", baglanti);
            OleDbDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                textBox5.Text = read["kategori"].ToString();
                textBox7.Text = read["ilacad"].ToString();
                textBox6.Text = read["satisfiyati"].ToString();

            }
            baglanti.Close();
        }

        private void Temizle()
        {
            if (textBox1.Text == "")
            {
                foreach (Control item in groupBox2.Controls)
                {
                    if (item is TextBox)
                    {
                        if (item != textBox8)
                        {
                            item.Text = "";
                        }
                    }
                }
            }
        }

        bool durum;
        private void barkodkontrol()
        {
            durum = true;
            baglanti.Open();
            OleDbCommand komut = new OleDbCommand("select *from sepet", baglanti);
            OleDbDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                if(textBox1.Text == read["barkodno"].ToString())
                {
                    durum = false;

                }
            }
            baglanti.Close();

        }





        private void button2_Click(object sender, EventArgs e) // EKLE BUTONU 
        {
            barkodkontrol();
            if ( durum ==true )
            {
                baglanti.Open();
                OleDbCommand komut = new OleDbCommand("insert into sepet(barkodno,kategori,ilacad,satisfiyati,miktari,toplamfiyat,tcno,ad,soyad) values(@barkodno,@kategori,@ilacad,@satisfiyati,@miktari,@toplamfiyat,@tcno,@ad,@soyad)", baglanti);
                komut.Parameters.AddWithValue("@barkodno", textBox1.Text);
                komut.Parameters.AddWithValue("@kategori", textBox5.Text);
                komut.Parameters.AddWithValue("@ilacad", textBox7.Text);
                komut.Parameters.AddWithValue("@satisfiyati", double.Parse(textBox6.Text));
                komut.Parameters.AddWithValue("@miktari", int.Parse(textBox8.Text));
                komut.Parameters.AddWithValue("@toplamfiyat", double.Parse(textBox9.Text));
                komut.Parameters.AddWithValue("@tcno", textBox2.Text);
                komut.Parameters.AddWithValue("@ad", textBox3.Text);
                komut.Parameters.AddWithValue("@soyad", textBox4.Text);
                komut.ExecuteNonQuery();
                baglanti.Close();

            }

            else
            {
                baglanti.Open();
                OleDbCommand komut2 = new OleDbCommand("update sepet set miktari = miktari+'"+int.Parse(textBox8.Text)+ "' where barkodno = '"+textBox1.Text+"'", baglanti);
                komut2.ExecuteNonQuery();
                OleDbCommand komut3= new OleDbCommand("update sepet set toplamfiyat = miktari*satisfiyati where barkodno = '"+textBox1.Text+"'", baglanti);
                komut3.ExecuteNonQuery();
                baglanti.Close();
            }



            
            textBox8.Text = "1";  // miktarı default 1 olsun
            daset.Tables["sepet"].Clear();
            sepetlistele();
            hesapla();
            foreach (Control item in groupBox2.Controls)
            {
                if (item is TextBox)
                {
                    if (item != textBox8)
                    {
                        item.Text = "";
                    }
                }
            }

        }

        private void textBox8_TextChanged(object sender, EventArgs e)  // MİKTARI KISMI
        {
            try
            {
                textBox9.Text = (double.Parse(textBox8.Text) * double.Parse(textBox6.Text)).ToString();

            }

            catch(Exception )
            {
                ;
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)  // SATIŞ FİYATI 
        {
            try
            {
                textBox9.Text = (double.Parse(textBox8.Text) * double.Parse(textBox6.Text)).ToString();

            }

            catch (Exception)
            {
                ;
            }
        }

        private void button5_Click(object sender, EventArgs e)  // SİL BUTONU 
        {
            baglanti.Open();
            OleDbCommand komut = new OleDbCommand("delete from sepet where barkodno='" + dataGridView1.CurrentRow.Cells["barkodno"].Value.ToString()+"'", baglanti);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("İlaç sepetten çıkarıldı.");
            daset.Tables["sepet"].Clear();
            sepetlistele();
            hesapla();
        }

        private void button7_Click(object sender, EventArgs e)  // SATIŞ İPTAL BUTONU
        {
            baglanti.Open();
            OleDbCommand komut = new OleDbCommand("delete from sepet", baglanti);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("İlaçlar sepetten çıkarıldı.");
            daset.Tables["sepet"].Clear();
            sepetlistele();
            hesapla();
        }

        private void button1_Click(object sender, EventArgs e) //  SATIŞLARI LİSTELE BUTONU
        {
            satisListele listele = new satisListele();
            listele.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)  // SATIŞ YAP BUTONU 
        {
            for(int i = 0; i < dataGridView1.Rows.Count-1;i++)
            {

                baglanti.Open();
                OleDbCommand komut = new OleDbCommand("insert into satis(barkodno,kategori,ilacad,satisfiyati,miktari,toplamfiyat,tcno,ad,soyad) values(@barkodno,@kategori,@ilacad,@satisfiyati,@miktari,@toplamfiyat,@tcno,@ad,@soyad)", baglanti);
                komut.Parameters.AddWithValue("@barkodno", dataGridView1.Rows[i].Cells["barkodno"].Value.ToString());
                komut.Parameters.AddWithValue("@kategori", dataGridView1.Rows[i].Cells["kategori"].Value.ToString());
                komut.Parameters.AddWithValue("@ilacad", dataGridView1.Rows[i].Cells["ilacad"].Value.ToString());
                komut.Parameters.AddWithValue("@satisfiyati", double.Parse(dataGridView1.Rows[i].Cells["satisfiyati"].Value.ToString()));
                komut.Parameters.AddWithValue("@miktari", int.Parse(dataGridView1.Rows[i].Cells["miktari"].Value.ToString()));
                komut.Parameters.AddWithValue("@toplamfiyat", double.Parse(dataGridView1.Rows[i].Cells["toplamfiyat"].Value.ToString()));
                komut.Parameters.AddWithValue("@tcno", textBox2.Text);
                komut.Parameters.AddWithValue("@ad", textBox3.Text);
                komut.Parameters.AddWithValue("@soyad", textBox4.Text);
                komut.ExecuteNonQuery();


                OleDbCommand komut2 = new OleDbCommand("UPDATE ilaclar SET miktari = miktari - @miktari WHERE barkodno = @barkodno", baglanti);
                komut2.Parameters.AddWithValue("@miktari", int.Parse(dataGridView1.Rows[i].Cells["miktari"].Value.ToString()));
                komut2.Parameters.AddWithValue("@barkodno", dataGridView1.Rows[i].Cells["barkodno"].Value.ToString());
                komut2.ExecuteNonQuery();

                baglanti.Close();
            }

            baglanti.Open();
            OleDbCommand komut3 = new OleDbCommand("delete from sepet", baglanti);
            komut3.ExecuteNonQuery();
            baglanti.Close();
            daset.Tables["sepet"].Clear();
            sepetlistele();
            hesapla();

            MessageBox.Show("Satış işlemi başarıyla gerçekleştirildi.");
        }
    }
}




