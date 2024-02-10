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
    public partial class yonetici : Form
    {
        public yonetici()
        {
            InitializeComponent();


        }

        List<Form> forms = new List<Form>();

        //OpenForm adlı bir yardımcı metodu tanımlıyoruz. Bu metod, her butona tıklandığında çağrılacak ve yeni formu açacak.
        //OpenForm metodu, önceki formu kapatır ve yeni formu açar. Ayrıca, forms adlı bir List<Form> kullanarak her açılan formu kaydeder.
        //Her butonun Click olayı, doğru formu açmak için OpenForm metodunu çağırır.


        private void OpenForm(Form form)
        {
            // Önceki formu kapat
            if (forms.Count > 0)
            {

                Form prevForm = forms.Last();
                prevForm.Close();
                forms.Remove(prevForm);
            }

            

            // Yeni formu aç
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            this.panel2.Controls.Add(form);
            this.panel2.Tag = form;
            form.BringToFront();
            form.Show();

            // Yeni formu listede kaydet
            forms.Add(form);
        }

        private void button1_Click(object sender, EventArgs e) // Çalışan Yönetimi
        {
            calısanDuzenle cd = new calısanDuzenle();
            OpenForm(cd);
        }

        
        private void button3_Click(object sender, EventArgs e)  //İlaç Yönetimi
        {
            OpenForm(new ilacYonetim());
        }

        private void button4_Click(object sender, EventArgs e)  //Müşteri Yönetimi
        {
            OpenForm(new musteriYonetim());
        }

        private void button5_Click(object sender, EventArgs e) //Çıkış
        {
            yoneticiGiris oncekimenu = new yoneticiGiris();
            oncekimenu.Show();
            this.Hide();    //ÖNCEKİ MENÜYE DÖNMEK İÇİN.
        }

        private void button6_Click(object sender, EventArgs e)  //Satış Yönetimi
        {
            OpenForm(new satisYönetim());
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {
            label2.Text = DateTime.Now.ToLongTimeString();

        }

        private void yonetici_Load(object sender, EventArgs e)
        {
            // Timer oluşturun ve intervalini 1 saniye (1000 milisaniye) olarak ayarlayın
            Timer timer = new Timer();
            timer.Interval = 1000;

            // Timer'ın Tick olayının olay işleyicisini tanımlayın
            timer.Tick += Timer_Tick;

            // Timer'ı başlatın
            timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            // Etiketin metnini güncel saat ve tarihle değiştirin
            label2.Text = DateTime.Now.ToString();
        }
    }
}
