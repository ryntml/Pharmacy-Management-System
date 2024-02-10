using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eczanemanage
{
    public partial class secimFormu : Form
    {

        private void secimFormu_SizeChanged(object sender, EventArgs e)
        {
            this.ClientSize = new Size(448, 631); // sabit boyut
        }
        public secimFormu()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            yoneticiGiris yg = new yoneticiGiris();
            yg.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            eczaciGiris eg = new eczaciGiris();
            eg.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();

        }

        private void secimFormu_Load(object sender, EventArgs e)
        {

        }
    }
}
