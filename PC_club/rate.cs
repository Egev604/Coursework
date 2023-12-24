using System;
using System.Linq;
using System.Windows.Forms;
using System.Text.RegularExpressions;
namespace PC_club
{
    public partial class rate : Form
    {
        bool result;
        string rdr = "rate";
        int lol;
        public void Laodd()
        {
            dataGridView1.DataSource = LoadData.Load("Select id_rate,  name_rate as 'Тарифы', price_onemin as 'Цена за 1 минуту' from rate");
            dataGridView1.Columns["id_rate"].Visible = false;
        }
        public rate(bool resul)
        {
            result = resul;
            InitializeComponent();
            Laodd();
            button2.Enabled = false;
            button3.Enabled = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            vedenieBD q = new vedenieBD(result);
            q.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (TextBox q in this.Controls.OfType<TextBox>())
            {
                if (q.Tag.ToString() == "vaz")
                {
                    if (q.Text == string.Empty)
                    {
                        MessageBox.Show("Заполните поля");
                        foreach (TextBox w in this.Controls.OfType<TextBox>())
                        {
                            w.Clear();
                        }
                        return;
                    }
                }
            }
            LoadData.Ins(String.Format("Insert into rate (name_rate, price_onemin) values('{0}', {1});", textBox1.Text, textBox2.Text));
            Laodd();
            foreach (TextBox w in this.Controls.OfType<TextBox>())
            {
                w.Clear();
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            string Symbol = e.KeyChar.ToString();
            if(Symbol ==".")
            {
                if(textBox2.Text.Contains(".") || textBox2.Text==String.Empty)
                {
                    e.Handled = true;
                }
            }

            if (!Regex.Match(Symbol, @"[1-9 .]|[\b]").Success)
            {
                e.Handled = true;
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string code;
            int rowIndex = e.RowIndex;
            try
            {
                code = dataGridView1.Rows[rowIndex].Cells["id_" + rdr].Value.ToString();
            }
            catch
            {
                return;
            }
            string strCmd = "Select * from rate Where id_" + rdr + "=" + '"' + code + '"' + ";";
            lol = Convert.ToInt32(code);
            LoadData.Reder(strCmd);
            string[] arr = new string[LoadData.mas.Length];
            arr = LoadData.mas;
            int i = 1;
            foreach (TextBox w in this.Controls.OfType<TextBox>())
            {
                w.Text = arr[i];
                i--;
            }
            textBox2.Text = textBox2.Text.Replace(',' , '.');
            button2.Enabled = true;
            button3.Enabled = true;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            string Symbol = e.KeyChar.ToString();

            if (!Regex.Match(Symbol, @"[а-яА-Я]|[a-zA-Z]|[\b]").Success)
            {
                e.Handled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (TextBox q in this.Controls.OfType<TextBox>())
            {
                if (q.Tag.ToString() == "vaz")
                {
                    if (q.Text == string.Empty)
                    {
                        MessageBox.Show("Заполните поля");
                        foreach (TextBox w in this.Controls.OfType<TextBox>())
                        {
                            w.Clear();
                        }
                        return;
                    }
                }
            }
            LoadData.Ins(String.Format("Update rate SET name_rate='{0}', price_onemin={1} where id_rate={2};", textBox1.Text, textBox2.Text, lol));
            Laodd();
            foreach (TextBox w in this.Controls.OfType<TextBox>())
            {
                w.Clear();
            }
            button2.Enabled = false;
            button3.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Вы уверены что хотите удалить эту запись", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (result == DialogResult.Yes)
            {
                LoadData.Ins("Delete from rate where id_" + rdr + "=" + '"' + lol + '"' + ';');
            }
            else
            {
                return;
            }
            Laodd();
            button2.Enabled = false;
            button3.Enabled = false;
        }
    }
}
