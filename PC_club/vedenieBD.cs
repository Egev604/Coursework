using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PC_club
{
    public partial class vedenieBD : Form
    {
        bool result;
        public vedenieBD(bool resul)
        {
            result = resul;
            InitializeComponent();
            if(!resul)
            {
                button1.Enabled = false;
                button3.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            staff q = new staff(result);
            q.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Главная q = new Главная(result);
            q.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 q = new Form1(result);
            q.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            client q = new client(result);
            q.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();
            pc q = new pc(result);
            q.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Hide();
            rate q = new rate(result);
            q.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Hide();
            arendaPC q = new arendaPC(result);
            q.Show();
        }
    }
}
