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
    public partial class sale : Form
    {
        bool result;
        int lol;
        int rowIndex;
        public sale(bool resul)
        {
            result = resul;
            InitializeComponent();
            Loadd();
            button1.Enabled = false;
            
        }
        public void Loadd()
        {
            dataGridView1.DataSource = LoadData.Load("Select id_sale,minuts as 'Минуты', sale as 'Скидка' from sale_minuts");
            dataGridView1.Columns["id_sale"].Visible = false;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string code;
            rowIndex = e.RowIndex;
            try
            {
                code = dataGridView1.Rows[rowIndex].Cells["id_sale"].Value.ToString();
            }
            catch
            {
                return;
            }
            lol = Convert.ToInt32(code);
            button1.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Вы уверены что хотите удалить эту запись", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (result == DialogResult.Yes)
            {
                if(lol==1)
                {
                    MessageBox.Show("Эту запись удалить нельзя");
                    return;
                }
                LoadData.Ins("Update clients Set idsale=1 where idsale="+lol);
                LoadData.Ins("Delete from sale_minuts where id_sale=" + '"' + lol + '"' + ';');
            }
            else
            {
                return;
            }
            Loadd();
            button1.Enabled = false;
        }

        private void sale_Load(object sender, EventArgs e)
        {
            dataGridView1.ClearSelection();
            foreach(DataGridViewRow row in dataGridView1.Rows)
            {
                if(row.Cells["Минуты"].Value.ToString()=="0")
                {
                    dataGridView1.Rows.Remove(row);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Главная q = new Главная(result);
            q.Show();
        }
    }
}
