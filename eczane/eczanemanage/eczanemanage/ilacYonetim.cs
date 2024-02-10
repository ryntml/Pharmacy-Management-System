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
    public partial class ilacYonetim : Form
    {
        public ilacYonetim()
        {
            InitializeComponent();
        }
        
        OleDbConnection baglantim = new OleDbConnection("Provider= Microsoft.ACE.OLEDB.12.0;Data Source = C:\\eczane\\eczanemanage\\eczanemanage\\bin\\Debug\\eczaneyonetim.mdb");
        
        private void ilacYonetim_Load(object sender, EventArgs e)
        {
            dataGridView1.ReadOnly = true;

            kayitlarilistele();
        }


        private void kayitlarilistele()
        {
            try
            {
                baglantim.Open();
                OleDbDataAdapter listele = new OleDbDataAdapter("select * from ilaclar", baglantim);
                DataSet dshafiza = new DataSet();
                listele.Fill(dshafiza);
                dataGridView1.DataSource = dshafiza.Tables[0];
                baglantim.Close();
            }

            catch (Exception hatamsj)
            {

                MessageBox.Show(hatamsj.Message);
                baglantim.Close();

            }
        }

        private void button1_Click_1(object sender, EventArgs e) //İLAÇ EKLEME
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text) || string.IsNullOrWhiteSpace(textBox3.Text) || string.IsNullOrWhiteSpace(textBox4.Text) || string.IsNullOrWhiteSpace(textBox5.Text) || comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Lütfen bütün alanları doldurunuz. ");
                return;
            }

            try
            {
                baglantim.Open();
                OleDbDataAdapter eklekomutu = new OleDbDataAdapter("insert into ilaclar (ilacad, barkodno, atcKod, firma, satisfiyati, kategori,miktari) values ('" + textBox1.Text + "', '" + textBox2.Text + "', '" + textBox3.Text + "', '" + textBox4.Text + "', '" + textBox5.Text + "', '" + comboBox1.SelectedItem.ToString() + "' ,'" +  textBox6.Text + "' )", baglantim);

                DataSet dshafiza = new DataSet();
                eklekomutu.Fill(dshafiza);
                baglantim.Close();
                MessageBox.Show("İlaç veri tabanına eklendi.");
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
                textBox5.Clear();
                textBox6.Clear();
                kayitlarilistele();
            }
            catch (Exception hatamsj)
            {
                MessageBox.Show(hatamsj.Message);
                baglantim.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)  // İLAÇ SİLME
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text) || string.IsNullOrWhiteSpace(textBox3.Text) || string.IsNullOrWhiteSpace(textBox4.Text) || string.IsNullOrWhiteSpace(textBox5.Text) || comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Lütfen bütün alanları doldurunuz. ");
                return;
            }
            
            try
            {
                baglantim.Open();
                OleDbCommand silkomutu = new OleDbCommand("DELETE FROM ilaclar WHERE barkodno = @barkodno AND atcKod = @atcKod", baglantim);
                silkomutu.Parameters.AddWithValue("@barkodno", int.Parse(textBox2.Text));
                silkomutu.Parameters.AddWithValue("@atcKod", textBox3.Text);
                silkomutu.ExecuteNonQuery();
                baglantim.Close();
                MessageBox.Show("İlaç veritabanından silindi.");
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
                textBox5.Clear();
                textBox6.Clear();
                kayitlarilistele();
            }
            catch (Exception hatamsj)
            {
                MessageBox.Show(hatamsj.Message);
                baglantim.Close();
            }

        }

        private void button3_Click(object sender, EventArgs e) // FİYAT GÜNCELLE
        {

            if (string.IsNullOrWhiteSpace(textBox5.Text) || string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Lütfen barkod numarası ve güncellenmek istenen fiyatı giriniz.");
                return;
            }
            try
            {
                baglantim.Open();
                OleDbCommand guncelleSorgusu = new OleDbCommand("UPDATE ilaclar SET satisfiyati = @satisfiyati WHERE barkodno = @barkodno", baglantim);
                guncelleSorgusu.Parameters.AddWithValue("@satisfiyati", int.Parse(textBox5.Text));
                guncelleSorgusu.Parameters.AddWithValue("@barkodno", textBox2.Text);
                guncelleSorgusu.ExecuteNonQuery();
                baglantim.Close();
                MessageBox.Show("İlacın fiyatı başarıyla güncellendi.");
                kayitlarilistele();
            }
            catch (Exception a)
            {
                MessageBox.Show(a.Message);
                baglantim.Close();
            }

        }

        private void button4_Click(object sender, EventArgs e)  //ARA BUTONU

        {

            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Lütfen barkod numarası giriniz.");
                return;
            }

            if (textBox2.Text.Length != 9)
            {
                MessageBox.Show("Barkod No 9 haneli olmalıdır!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox2.Clear();
                return;
            }

            try
            {
                baglantim.Open();
                DataSet dshafiza = new DataSet();
                OleDbCommand aramaSorgusu = new OleDbCommand("SELECT * FROM ilaclar WHERE barkodno = @barkodno", baglantim);
                aramaSorgusu.Parameters.AddWithValue("@barkodno", textBox2.Text);
                OleDbDataAdapter arama = new OleDbDataAdapter(aramaSorgusu);
                arama.Fill(dshafiza, "ilaclar");
                dataGridView1.DataSource = dshafiza.Tables["ilaclar"];
                baglantim.Close();
            }
            catch (Exception hatamsj)
            {
                MessageBox.Show(hatamsj.Message);
                baglantim.Close();
            }
        }
   
    }
}

