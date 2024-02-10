using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eczanemanage
{
    public partial class eczaci : Form
    {

        private Form currentForm; // Şu an gösterilen form
        public eczaci()
        {
            InitializeComponent();
        }

        // Yardımcı metot, sağ taraftaki paneli yeni bir form ile değiştirir
        private void ShowForm(Form form)
        {
            // Şu an gösterilen formu sakla veya kapat
            if (currentForm != null)
            {
                currentForm.Hide();
            }

            // Yeni formu sağ taraftaki panelde göster
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            panel1.Controls.Add(form);
            panel1.Tag = form;
            form.BringToFront();
            form.Show();

            // Şu an gösterilen formu kaydet
            currentForm = form;
        }

     
        
        private void eczaci_Load(object sender, EventArgs e)
        {
            // Timer oluşturdum ve intervalini 1 saniye (1000 milisaniye) olarak ayarladım
            Timer timer = new Timer();
            timer.Interval = 1000;

            // Timer'ın Tick olayının olay işleyicisini tanımladım
            timer.Tick += Timer_Tick;

            // Timer'ı başlat
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            
            label2.Text = DateTime.Now.ToString();
        }

        private void button2_Click(object sender, EventArgs e) //stok takibi
        {
            ilacYonetim ilac = new ilacYonetim();
            ShowForm(ilac);

        }

        private void button1_Click(object sender, EventArgs e)  // satış yap
        {
            satisYönetim satisYap = new satisYönetim();
            ShowForm(satisYap);
        }

      

        private void button4_Click(object sender, EventArgs e) // müşteri
        {
            musteriYonetim musteri = new musteriYonetim();  
            ShowForm(musteri);  
        }

        private void button5_Click(object sender, EventArgs e) // çıkış
        {
            eczaciGiris oncekimenu = new eczaciGiris();
            oncekimenu.Show();
            this.Hide();    //ÖNCEKİ MENÜYE DÖNMEK İÇİN.
        }

        
    }

}
