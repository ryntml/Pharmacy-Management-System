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
using System.Data.OleDb;

namespace eczanemanage
{
    public partial class eczaciGiris : Form
    {
        
        public eczaciGiris()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            secimFormu don = new secimFormu();  
            don.Show();
            this.Hide();

        }

        private void eczaci_Load(object sender, EventArgs e)
        {
            this.ClientSize = new Size(448, 631); // sabit boyut
        }

        OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\eczane\\eczanemanage\\eczanemanage\\bin\\Debug\\eczaneyonetim.mdb");
        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Bütün alanları doldurun. ");

            }

            else
            {
                baglanti.Open();
                OleDbCommand komut = new OleDbCommand("select * from eczaciGiris where KullaniciAdi='" + textBox1.Text + "'", baglanti);
                OleDbDataReader okuyucu = komut.ExecuteReader();
                if (okuyucu.Read() == true)
                {
                    if (textBox1.Text == okuyucu["KullaniciAdi"].ToString() && textBox2.Text == okuyucu["Sifre"].ToString())
                    {
                        MessageBox.Show("Hoşgeldin Sayın " + okuyucu["KullaniciAdi"].ToString());
                        eczaci es = new eczaci();  // eczaci sayfası  (es)
                        es.Show();
                        this.Hide();

                    }
                    else
                    {
                        MessageBox.Show("Giriş bilgilerinizi kontrol ediniz!");
                    }
                }
                else
                {
                    MessageBox.Show("Giriş bilgilerinizi kontrol ediniz!");
                }

                baglanti.Close();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox2.PasswordChar = '*';
        }

        private void button1_Click(object sender, EventArgs e)  // SIFIRLA BUTONU
        {
            textBox1.Clear();
            textBox2.Clear();
        }
    }
}
