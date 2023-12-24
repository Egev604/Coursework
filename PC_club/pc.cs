using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
namespace PC_club
{
    public partial class pc : Form
    {
        bool result;
        string rdr = "pc";
        int lol;
        public void Loadd()
        {
            dataGridView1.DataSource = LoadData.Load("Select id_pc, number_pc as 'Номер ПК', lastdate as 'Последняя дата использования', name_rate as 'Тариф', cost as 'Состояние' from pc inner join rate on rate.id_rate=pc.id_rate");
            dataGridView1.Columns["id_pc"].Visible = false;
        }
        public pc(bool resul)
        {
            result = resul;
            InitializeComponent();
            Loadd();
            button2.Enabled = false;
            button3.Enabled = false;
            Array.Resize(ref LoadData.mas2, (LoadData.mas2.Length - (LoadData.mas2.Length - 1)));
            LoadData.ComboBox("Select name_rate from rate");
            for (int i = 0; i < LoadData.mas2.Length; i++)
            {
                comboBox1.Items.Add(LoadData.mas2[i]);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            vedenieBD q = new vedenieBD(result);
            q.Show();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            string Symbol = e.KeyChar.ToString();

            if (!Regex.Match(Symbol, @"[1-9]|[\b]").Success)
            {
                e.Handled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text==string.Empty)
            {
                MessageBox.Show("Заполните поля");
                textBox1.Clear();
                return;
            }
            if (comboBox1.Text == string.Empty)
            {
                MessageBox.Show("Заполните поля");
                comboBox1.SelectedIndex =-1;
                return;
            }
            LoadData.Ins(String.Format("Insert into pc (number_pc, lastdate, cost, id_rate) values({0}, CURRENT_DATE, 0, (select id_rate from rate where name_rate='{1}'));", textBox1.Text, comboBox1.Text));
            Loadd();
            comboBox1.SelectedIndex = -1;
            textBox1.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == string.Empty)
            {
                MessageBox.Show("Заполните поля");
                textBox1.Clear();
                return;
            }
            if (comboBox1.Text == string.Empty)
            {
                MessageBox.Show("Заполните поля");
                comboBox1.SelectedIndex = -1;
                return;
            }

            LoadData.Ins(String.Format("Update pc SET number_pc={0}, id_rate=(select id_rate from rate where name_rate='{1}') where id_pc={2};", textBox1.Text, comboBox1.Text, lol));
            Loadd();
            textBox1.Clear();
            comboBox1.SelectedIndex = -1;
            button2.Enabled = false;
            button3.Enabled = false;
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
            string strCmd = "Select id_pc, number_pc as 'Номер ПК', lastdate as 'Последняя дата использования', name_rate as 'Тариф', cost as 'Состояние' from pc Where id_" + rdr + "=" + code + ";";
            lol = Convert.ToInt32(code);
            LoadData.Reder(strCmd);
            string[] arr = new string[LoadData.mas.Length];
            arr = LoadData.mas;
            textBox1.Text = arr[0];
            comboBox1.Text = arr[1];
            button2.Enabled = true;
            button3.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Вы уверены что хотите удалить эту запись", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (result == DialogResult.Yes)
            {
                LoadData.Ins("Delete from pc where id_" + rdr + "=" + '"' + lol + '"' + ';');
            }
            else
            {
                return;
            }
            Loadd();
            button2.Enabled = false;
            button3.Enabled = false;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Loadd();
            Format();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = LoadData.Load("Select id_pc, number_pc as 'Номер ПК', lastdate as 'Последняя дата использования', name_rate as 'Тариф', cost as 'Состояние' from pc inner join rate on rate.id_rate=pc.id_rate where cost=0");
            dataGridView1.Columns["id_pc"].Visible = false;
            Format();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = LoadData.Load("Select id_pc, number_pc as 'Номер ПК', lastdate as 'Последняя дата использования', name_rate as 'Тариф', cost as 'Состояние' from pc inner join rate on rate.id_rate=pc.id_rate where cost=1");
            dataGridView1.Columns["id_pc"].Visible = false;
            Format();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (textBox4.Text.Length == 0)
            {
                dataGridView1.DataSource = LoadData.Load("Select id_pc, number_pc as 'Номер ПК', lastdate as 'Последняя дата использования', name_rate as 'Тариф', cost as 'Состояние' from pc inner join rate on rate.id_rate=pc.id_rate");
                dataGridView1.Columns["id_pc"].Visible = false;
                Format();
            }
            dataGridView1.DataSource = LoadData.Load("Select id_pc, number_pc as 'Номер ПК', lastdate as 'Последняя дата использования', name_rate as 'Тариф', cost as 'Состояние' from pc inner join rate on rate.id_rate=pc.id_rate Where number_pc like '%" + textBox4.Text + "%'");
            dataGridView1.Columns["id_pc"].Visible = false;
            Format();
        }

        private void pc_Load(object sender, EventArgs e)
        {
            Format();
        }
        public void Format()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if ((bool)dataGridView1.Rows[i].Cells["Состояние"].Value)
                {
                    dataGridView1.Rows[i].Cells["Состояние"].Style.BackColor = Color.Red;
                }
                else
                {
                    dataGridView1.Rows[i].Cells["Состояние"].Style.BackColor = Color.Green;
                }
            }
        }
    }
}
