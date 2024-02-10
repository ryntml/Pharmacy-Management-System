using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions; // regex kütüphanesinin tanımlanması
using System.Windows.Forms.VisualStyles;
using System.Drawing.Text;

namespace eczanemanage
{
    public partial class calısanDuzenle : Form
    {

        public calısanDuzenle()
        {
            InitializeComponent();
        }


        OleDbConnection baglantim = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\eczane\\eczanemanage\\eczanemanage\\bin\\Debug\\eczaneyonetim.mdb");
       

        private void calisanlari_goster()
        {
            try
            {
                baglantim.Open();
                OleDbDataAdapter kayitlistele = new OleDbDataAdapter("select * from calisanListesi", baglantim);
                DataSet dshafiza = new DataSet();
                kayitlistele.Fill(dshafiza);
                dataGridView1.DataSource = dshafiza.Tables[0];
                baglantim.Close();
            }

            catch (Exception hatamsj)
            {

                MessageBox.Show(hatamsj.Message);
                baglantim.Close();

            }

        }

        private void calısanDuzenle_Load(object sender, EventArgs e)
        {
            dataGridView1.ReadOnly = true;
            calisanlari_goster();

            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.Width = 100; pictureBox1.Height = 100;
            pictureBox1.BorderStyle = BorderStyle.Fixed3D;
            maskedTextBox1.Mask = "00000000000";
            maskedTextBox2.Mask = "LL????????????????";
            maskedTextBox3.Mask = "LL????????????????";
            maskedTextBox4.Mask = "000-000-00-00";
            maskedTextBox5.Mask = "LL????????????????";
            maskedTextBox5.MaxLength = 10;
            maskedTextBox6.Mask = "LL??????????????????????????????????????";
            maskedTextBox6.MaxLength = 20;
            maskedTextBox7.Mask = "000000";
            //maskedTextBox7.MaxLength = 10;
            maskedTextBox7.Mask = ">00000";
            



            DateTime zaman = DateTime.Now;
            int yil = int.Parse(zaman.ToString("yyyy"));
            int ay = int.Parse(zaman.ToString("MM"));
            int gun = int.Parse(zaman.ToString("dd"));

            dateTimePicker1.MinDate = new DateTime(1960,1,1);
            dateTimePicker1.MaxDate = new DateTime(yil - 18, ay, gun);
            dateTimePicker1.Format = DateTimePickerFormat.Short;
        }

        private void button6_Click(object sender, EventArgs e)   // GÖZAT BUTONU
        {
            OpenFileDialog resimsec = new OpenFileDialog();
            resimsec.Title = "Personel resmi seçiniz.";
            //resimsec.Filter = "JPG Dosyalar (*.jpg) | *.jpg ";
            if (resimsec.ShowDialog()==DialogResult.OK)
            {
                this.pictureBox1.Image = new Bitmap(resimsec.OpenFile());
            }
        }

        private void button1_Click(object sender, EventArgs e)  // KAYDET BUTONU
        {
            bool kayitKontrol = false;

            baglantim.Open();

            OleDbCommand selectsorgu = new OleDbCommand("SELECT * FROM calisanListesi WHERE [TC No] = @tcno", baglantim);
            selectsorgu.Parameters.AddWithValue("@tcno", maskedTextBox1.Text);
            OleDbDataReader kayitokuma = selectsorgu.ExecuteReader();

            while (kayitokuma.Read())
            {
                kayitKontrol = true;
                break;
            }
            baglantim.Close();

            if (kayitKontrol ==false)
            {
                if (pictureBox1.Image == null)
                    button6.ForeColor =Color.Red;
                else button6.ForeColor =Color.Black;

                if (maskedTextBox1.MaskCompleted == false)
                    label3.ForeColor = Color.Red;
                else label3.ForeColor = Color.Black;

                if (maskedTextBox2.MaskCompleted == false)
                    label1.ForeColor = Color.Red;
                else label1.ForeColor = Color.Black;

                if (maskedTextBox3.MaskCompleted == false)
                    label2.ForeColor = Color.Red;
                else label2.ForeColor = Color.Black;

                if (maskedTextBox4.MaskCompleted == false)
                    label5.ForeColor = Color.Red;
                else label5.ForeColor = Color.Black;

                if (maskedTextBox5.MaskCompleted == false)
                    label4.ForeColor = Color.Red;
                else label4.ForeColor = Color.Black;

                if (maskedTextBox6.MaskCompleted == false)
                    label6.ForeColor = Color.Red;
                else label6.ForeColor = Color.Black;

                if (maskedTextBox7.MaskCompleted == false)
                    label7.ForeColor = Color.Red;
                else label7.ForeColor = Color.Black;


                if (pictureBox1.Image != null && maskedTextBox1.MaskCompleted != false && maskedTextBox2.MaskCompleted != false 
                    && maskedTextBox3.MaskCompleted != false && maskedTextBox4.MaskCompleted != false && maskedTextBox5.MaskCompleted != false
                    && maskedTextBox6.MaskCompleted != false && maskedTextBox7.MaskCompleted != false)

                {
                    try
                    {
                        baglantim.Open();
                        OleDbCommand eklekomutu = new OleDbCommand("insert into calisanListesi values ('" + maskedTextBox1.Text +
                            "' , '" +maskedTextBox2.Text+"' , '" +maskedTextBox3.Text+"' , '"+maskedTextBox4.Text+"', '"+maskedTextBox5.Text+ "','"+maskedTextBox6.Text+"','"+maskedTextBox7.Text+"', '"+dateTimePicker1.Text+"' )", baglantim);
                        eklekomutu.ExecuteNonQuery();
                        baglantim.Close();

                        if (!Directory.Exists(Application.StartupPath + "\\calisanresimler"))
                            Directory.CreateDirectory(Application.StartupPath + "\\calisanresimler");
                        else
                            pictureBox1.Image.Save(Application.StartupPath + "\\calisanresimler\\" + maskedTextBox1.Text +".jpg");
                        MessageBox.Show("Yeni çalışan kaydı oluşturuldu.", "Hayat Eczanesi",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);

                        calisanlari_goster();
                        maskedTextBox1.Clear();
                        maskedTextBox2.Clear();
                        maskedTextBox3.Clear();
                        maskedTextBox4.Clear();
                        maskedTextBox5.Clear();
                        maskedTextBox6.Clear();
                        maskedTextBox7.Text = "0";
                        pictureBox1.Image = null;
                        dateTimePicker1.Value = DateTime.MinValue;
                    }

                    catch (Exception hatamsj)
                    {
                        MessageBox.Show(hatamsj.Message, "Hayat Eczanesi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        baglantim.Close();
                    }
                }

                else
                    MessageBox.Show("Yazı rengi kırmızı olan alanları yeniden gözden geçiriniz!", "Hayat Eczanesi", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
            }
            else
                MessageBox.Show("Girilen TC Kimlik numarası daha önceden kayıtlıdır!", "Hayat Eczanesi", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
        }

        private void button5_Click(object sender, EventArgs e)  // ARA BUTONU
        {
            bool kayit_arama_durumu = false;
            if(maskedTextBox1.Text.Length ==11)
            {
                baglantim.Open();

                OleDbCommand selectsorgu = new OleDbCommand("SELECT * FROM calisanListesi WHERE [TC No] = @TCKimlik", baglantim);
                selectsorgu.Parameters.AddWithValue("@TCKimlik", maskedTextBox1.Text);


                OleDbDataReader kayitokuma = selectsorgu.ExecuteReader();   
                while(kayitokuma.Read())
                {
                    kayit_arama_durumu = true;
                    try
                    {
                        pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\calisanresimler\\" + kayitokuma.GetValue(2).ToString() + ".jpg");// tckimlik bizim tabomuzda 2. indexte

                    }

                    catch 
                    {
                        pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\calisanresimler\\resimyok.jpg");
                    }

                    maskedTextBox1.Text = kayitokuma.GetValue(0).ToString(); // TC
                    maskedTextBox2.Text = kayitokuma.GetValue(1).ToString(); // AD   
                    maskedTextBox3.Text = kayitokuma.GetValue(2).ToString(); // SOYAD
                    maskedTextBox4.Text = kayitokuma.GetValue(3).ToString(); // TELEFON NO
                    maskedTextBox5.Text = kayitokuma.GetValue(4).ToString(); // GÖREV
                    maskedTextBox6.Text = kayitokuma.GetValue(5).ToString(); // ADRES
                    maskedTextBox7.Text = kayitokuma.GetValue(6).ToString(); // MAAŞ
                    

                }

                if(kayit_arama_durumu==false)
                {
                    MessageBox.Show("Aranan kayıt bulunamadı.", "Hayat Eczanesi", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                }
                baglantim.Close();
            }

            else
            {

                MessageBox.Show("11 haneli TC kimlik numarasını giriniz!", "Hayat Eczanesi", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

            
        }

        private void sayfayi_temizle() 
        {
            maskedTextBox1.Clear();
            maskedTextBox2.Clear();
            maskedTextBox3.Clear();
            maskedTextBox4.Clear();
            maskedTextBox5.Clear();
            maskedTextBox6.Clear();
            maskedTextBox7.Text = "0";
            pictureBox1.Image = null;
            

        }

        private void button4_Click(object sender, EventArgs e)  // TEMİZLE BUTONU
        {
            sayfayi_temizle();
        }

        


        private void button3_Click(object sender, EventArgs e) //SİL BUTONU
        {
            if(maskedTextBox1.MaskCompleted == true)
            {
                bool kayit_arama_durumu = false;
                baglantim.Open();
                OleDbCommand arama_sorgusu = new OleDbCommand("select * from calisanListesi where [TC No] = '"+ maskedTextBox1.Text +"'", baglantim);
                OleDbDataReader kayitokuma = arama_sorgusu.ExecuteReader(); 
                while(kayitokuma.Read()) 
                {
                    kayit_arama_durumu = true;
                    OleDbCommand deletesorgu = new OleDbCommand("delete from calisanListesi where [TC No] = '"+ maskedTextBox1.Text +"'", baglantim);
                    deletesorgu.ExecuteNonQuery();
                    break;
                }

                if(kayit_arama_durumu == false) 
                {
                    MessageBox.Show("Silinecek kayıt bulunamadı!", "Hayat Eczanesi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                baglantim.Close() ;
                calisanlari_goster();
                maskedTextBox1.Clear();
                maskedTextBox2.Clear();
                maskedTextBox3.Clear();
                maskedTextBox4.Clear();
                maskedTextBox5.Clear();
                maskedTextBox6.Clear();
                maskedTextBox7.Text = "0";
                pictureBox1.Image = null;

            }

            else
            {
                MessageBox.Show("Lütfen 11 haneli bir TC kimlik numarası giriniz!", "Hayat Eczanesi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click_1(object sender, EventArgs e)  // GÜNCELLE BUTONU
        {
            if (pictureBox1.Image == null)
                button6.ForeColor = Color.Red;
            else
                button6.ForeColor = Color.Black;

            if (!maskedTextBox1.MaskCompleted)
                label3.ForeColor = Color.Red;
            else
                label3.ForeColor = Color.Black;

            if (!maskedTextBox2.MaskCompleted)
                label1.ForeColor = Color.Red;
            else
                label1.ForeColor = Color.Black;

            if (!maskedTextBox3.MaskCompleted)
                label2.ForeColor = Color.Red;
            else
                label2.ForeColor = Color.Black;

            if (!maskedTextBox4.MaskCompleted)
                label5.ForeColor = Color.Red;
            else
                label5.ForeColor = Color.Black;

            if (!maskedTextBox5.MaskCompleted)
                label4.ForeColor = Color.Red;
            else
                label4.ForeColor = Color.Black;

            if (!maskedTextBox6.MaskCompleted)
                label6.ForeColor = Color.Red;
            else
                label6.ForeColor = Color.Black;

            if (!maskedTextBox7.MaskCompleted)
                label7.ForeColor = Color.Red;
            else
                label7.ForeColor = Color.Black;

            if (pictureBox1.Image != null && maskedTextBox1.MaskCompleted && maskedTextBox2.MaskCompleted
                && maskedTextBox3.MaskCompleted && maskedTextBox4.MaskCompleted && maskedTextBox5.MaskCompleted
                && maskedTextBox6.MaskCompleted && maskedTextBox7.MaskCompleted)
            {
                try
                {
                    baglantim.Open();
                    OleDbCommand guncellekomutu = new OleDbCommand("UPDATE calisanListesi SET Ad=?, Soyad=?, [Telefon No]=?, Görevi=?, Adresi=?, Maaş=?, [Dogum Tarih]=? WHERE [TC No]=?", baglantim);
                    guncellekomutu.Parameters.AddWithValue("@p1", maskedTextBox2.Text);
                    guncellekomutu.Parameters.AddWithValue("@p2", maskedTextBox3.Text);
                    guncellekomutu.Parameters.AddWithValue("@p3", maskedTextBox4.Text);
                    guncellekomutu.Parameters.AddWithValue("@p4", maskedTextBox5.Text);
                    guncellekomutu.Parameters.AddWithValue("@p5", maskedTextBox6.Text);
                    guncellekomutu.Parameters.AddWithValue("@p6", maskedTextBox7.Text);
                    guncellekomutu.Parameters.AddWithValue("@p7", dateTimePicker1.Value);
                    guncellekomutu.Parameters.AddWithValue("@p8", maskedTextBox1.Text);

                    guncellekomutu.ExecuteNonQuery();
                    baglantim.Close();
                    calisanlari_goster();

                    maskedTextBox1.Clear();
                    maskedTextBox2.Clear();
                    maskedTextBox3.Clear();
                    maskedTextBox4.Clear();
                    maskedTextBox5.Clear();
                    maskedTextBox6.Clear();
                    maskedTextBox7.Text = "0";
                    pictureBox1.Image = null;
                }
                catch (Exception hatamsj)
                {
                    MessageBox.Show(hatamsj.Message, "Hayat Eczanesi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    baglantim.Close();
                }
            }
        }
    }
    }



